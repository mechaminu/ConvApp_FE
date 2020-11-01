using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using ConvApp.ViewModels;
using RestSharp;
using HeyRed.Mime;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ConvApp.Models;
using Plugin.Media.Abstractions;
using Xamarin.Forms.Internals;

namespace ConvApp
{
    public class ApiManager
    {
        
        private static string EndPointURL = "http://192.168.35.2:8080/api";
        private static RestClient client = new RestClient(EndPointURL) { Timeout = 60 };

        #region REST API : 포스트 CRUD 구현
        /// <summary>
        /// 포스트엔트리(포스트입력) 뷰모델을 받아 백엔드에 업로드하는 메소드 
        /// </summary>
        /// <param name="posting">포스트엔트리 객체</param>
        /// <returns>포스트 뷰모델 객체</returns>
        public async static Task<Post> UploadPosting(PostEntry posting)
        {
            var payloadObject = new Posting
            {
                pst_type = (byte)posting.Type,
                title = posting.PostTitle,
                text = posting.PostContent,
                images = await UploadImage(posting.PostImage),  // 이미지 업로드를 먼저 실시한다

                create_user_oid = 1234567890,    // TODO
                products = null    // TODO
            };

            try
            {
                // Restsharp 플러그인 사용
                var request = new RestRequest("postings",Method.POST)
                    .AddHeader("Content-Type", "application/json")
                    .AddJsonBody(JsonConvert.SerializeObject(payloadObject));

                var response = await client.ExecuteAsync<Posting>(request);

                if (!response.IsSuccessful)
                    throw new InvalidOperationException("포스팅 실패");

                // 응답으로 전달받은 포스트 모델 -> 뷰모델 변환하여 리턴
                return await Posting.ToPost(response.Data);
            }
            catch (Exception exp)
            {
                payloadObject.images.Split(',')
                .ForEach(async (e) => {
                    if (await DeleteImage(e))
                    {
                        throw new InvalidOperationException($"이미지 삭제에서 문제 발생 - {e}");
                    }
                });
                    
                throw exp;
            }
        }

        public async static Task<Posting> GetPostingModel(long id)
        {
            var request = new RestRequest($"postings/{id}", Method.GET);
            var response = await client.ExecuteAsync<Posting>(request);
            return response.Data;
        }

        public async static Task<Post> GetPosting(long id)
        {
            return await Posting.ToPost(await GetPostingModel(id));
        }

        public async static Task<List<Post>> GetPostingPage(int id)
        {
            var request = new RestRequest($"postings/page/{id}", Method.GET);
            var response = await client.ExecuteAsync<List<Posting>>(request);
            var resList = new List<Post>();
            response.Data.ForEach(async e => resList.Add(await Posting.ToPost(e)));
            return resList;
        }

        public async static Task<int> GetPostingLastPage()
        {
            var request = new RestRequest($"postings/page", Method.GET);
            var response = await client.ExecuteAsync<int>(request);
            return response.Data - 1;
        }

        //public static async Task<Post> PutPosting(PostEntry posting)
        //{
        //    return;
        //}

        //public static async Task<Post> PatchPosting(PostEntry posting)
        //{
        //    return;
        //}

        public async static Task<bool> DeletePosting(long id)
        {
            Posting posting = await GetPostingModel(id);

            // 이미지 삭제
            posting.images.Split(',').ForEach(async e => await DeleteImage(e));

            // 피드백 삭제
            // TODO

            //포스트 삭제
            var request = new RestRequest($"postings/{id}", Method.DELETE);
            var response = await client.ExecuteAsync(request);

            return response.IsSuccessful;
        }

        #endregion

        #region REST API : 이미지 CR(U)D 구현
        // 이미지는 수정이 필요 없음
        public async static Task<string> UploadImage(List<MediaFile> mediafiles)
        {
            var tmpDict = new Dictionary<string, object>();
            int cnt = 0;
            foreach (var img in mediafiles)
            {
                tmpDict.Add("file"+cnt++, new FormFile
                {
                    Name = "uploadedimage",
                    ContentType = MimeTypesMap.GetMimeType(img.Path),
                    Stream = img.GetStream()
                });
            }

            // 업로드를 통해 생성된 이미지 파일명 목록(Joined with ',') 획득 및 리턴 
            return await PostMultipart(EndPointURL + "/images", tmpDict);
        }

        public async static Task<Stream> GetImage(string filename)
        {
            var request = new RestRequest($"images/{filename.Trim()}",Method.GET);
            
            var response = await client.ExecuteAsync(request);
            
            var result = new MemoryStream(response.RawBytes);
            return result;
        }

        public async static Task<bool> DeleteImage(string filename)
        {
            var request = new RestRequest($"images/{filename.Trim()}");
            request.Method = Method.DELETE;

            var response = await client.ExecuteAsync(request);

            return response.IsSuccessful;
        }
        #endregion

        public async static Task<string> PostMultipart(string url, Dictionary<string, object> parameters)
        {
            // Multipart 구분 boundary
            string boundary = "-----------------" + DateTime.Now.Ticks.ToString("x");
            byte[] boundaryBytes = Encoding.ASCII.GetBytes("--" + boundary + "\r\n");
            byte[] trailerBytes = Encoding.ASCII.GetBytes($"--{boundary}--\r\n");
            byte[] rnBytes = Encoding.ASCII.GetBytes("\r\n");

            // Header
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = "multipart/form-data; boundary=" + boundary;
            request.Method = "POST";
            request.KeepAlive = true;
            request.Credentials = CredentialCache.DefaultCredentials;

            if (parameters != null && parameters.Count > 0)
            {
                using (Stream reqStream = request.GetRequestStream())
                {
                    Console.Write($"formdata count : {parameters.Count}");
                    foreach (KeyValuePair<string, object> pair in parameters)
                    {
                        reqStream.Write(boundaryBytes, 0, boundaryBytes.Length);
                        if (pair.Value is FormFile)
                        {
                            FormFile file = pair.Value as FormFile;
                            string header = "Content-Disposition: form-data; name=\"" + pair.Key + "\"; filename=\"" + file.Name + "\"\r\nContent-Type: " + file.ContentType + "\r\n\r\n";
                            byte[] bytes = Encoding.UTF8.GetBytes(header);
                            reqStream.Write(bytes, 0, bytes.Length);
                            byte[] buffer = new byte[32768];
                            int bytesRead;
                            if (file.Stream == null)
                            {
                                // File로부터 업로드
                                using (FileStream fileStream = File.OpenRead(file.FilePath))
                                {
                                    while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                                        reqStream.Write(buffer, 0, buffer.Length);
                                    fileStream.Close();
                                }
                            }
                            else
                            {
                                // Stream으로부터 업로드
                                while ((bytesRead = file.Stream.Read(buffer, 0, buffer.Length)) != 0)
                                    reqStream.Write(buffer, 0, buffer.Length);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Not FormFile");
                            string data = $@"Content-Disposition: form-data; name=""{pair.Key}""" + "\r\n\r\n" + $"{pair.Value}";
                            byte[] bytes = Encoding.UTF8.GetBytes(data);
                            reqStream.Write(bytes, 0, bytes.Length);
                        }
                        reqStream.Write(rnBytes, 0, rnBytes.Length);
                    }

                    reqStream.Write(trailerBytes, 0, trailerBytes.Length);
                    reqStream.Close();
                }
            }

            using (WebResponse res = request.GetResponse())
            using (Stream resStream = res.GetResponseStream())
            using (StreamReader reader = new StreamReader(resStream))
                return await reader.ReadToEndAsync();
        }
    }

    public class FormFile
    {
        public string Name { get; set; }
        public string ContentType { get; set; }
        public string FilePath { get; set; }
        public Stream Stream { get; set; }
    }
}
