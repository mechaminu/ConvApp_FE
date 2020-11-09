using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using ConvApp.Models;
using ConvApp.ViewModels;

using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;
using Newtonsoft.Json;
using HeyRed.Mime;

using Plugin.Media.Abstractions;
using Xamarin.Forms.Internals;

namespace ConvApp
{
    public class ApiManager
    {

        private static string EndPointURL = "http://convappdev.azurewebsites.net/api";
        private static RestClient client = new RestClient(EndPointURL){Timeout=-1}.UseNewtonsoftJson() as RestClient;

        #region REST API : 포스트 CRUD 구현
        /// <summary>
        /// 포스트엔트리(포스트입력) 뷰모델을 받아 백엔드에 업로드하는 메소드 
        /// </summary>
        /// <param name="post">포스트 뷰모델 객체</param>
        /// <returns>포스트 뷰모델 객체</returns>
        public async static Task<Post> UploadPosting(Post post)
        {
            try
            {
                var payloadObject = new Posting
                {
                    pst_type = (byte)posting.Type,
                    title = posting.PostTitle,
                    text = posting.PostContent,
                    images = await UploadImage(posting.PostImage),  // 이미지 업로드

                    create_user_oid = 1234567890,   // dummy data
                    products = null                 // dummy data
                };

                try
                {
                    var request = new RestRequest("postings",Method.POST)
                        .AddJsonBody(payloadObject);
                    var response = await client.ExecuteAsync<Posting>(request);

                    if (!response.IsSuccessful)
                        throw new InvalidOperationException("포스팅 실패");
                    
                    return await Posting.ToPost(response.Data); // 응답으로 전달받은 포스트 모델 -> 뷰모델 변환하여 리턴
                }
                catch (Exception ex)
                {
                    payloadObject.images.Split(',').ForEach(async (e) => await DeleteImage(e));
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async static Task<Posting> GetPostingModel(long id)
        {
            try
            { 
                return (await client.ExecuteAsync<Posting>(new RestRequest($"postings/{id}", Method.GET))).Data; 
            }
            catch (Exception e) { throw e; }
        }

        public async static Task<Post> GetPosting(long id)
        {
            try
            {
                return await Posting.ToPost(await GetPostingModel(id));
            }
            catch (Exception e) { throw e; }
        }

        public async static Task<List<Post>> GetPostingPage(int id)
        {
            try
            {
                var resList = new List<Post>();
                (await client.ExecuteAsync<List<Posting>>(new RestRequest($"postings/page/{id}", Method.GET))).Data.ForEach(async e => resList.Add(await Posting.ToPost(e)));
                return resList;
            }
            catch (Exception e) { throw e; }
        }

        public async static Task<int> GetPostingLastPage()
        {
            try
            {
                return (await client.ExecuteAsync<int>(new RestRequest($"postings/page", Method.GET))).Data - 1;
            }
            catch (Exception e) { throw e; }
        }

        public async static Task<bool> DeletePosting(long id)
        {
            try
            {
                (await GetPostingModel(id)).images.Split(',').ForEach(async e => await DeleteImage(e));     // 이미지 삭제
                var response = await client.ExecuteAsync(new RestRequest($"postings/{id}", Method.DELETE)); // 포스트 삭제
                                                                                                            // TODO : 피드백 삭제
                return response.IsSuccessful;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        #endregion

        #region REST API : 이미지 CR(U)D 구현
        // 이미지는 수정이 필요 없음
        public async static Task<string> UploadImage(List<MediaFile> mediafiles)
        {
            try
            {
                if (mediafiles.Count == 0)
                    return null;

                var tmpDict = new Dictionary<string, object>();
                foreach (var img in mediafiles)
                {
                    tmpDict.Add("file_" + tmpDict.Count, new FormFile
                    {
                        Name = "uploadedimage",
                        ContentType = MimeTypesMap.GetMimeType(img.Path),
                        Stream = img.GetStream()
                    });
                }

                // 업로드를 통해 생성된 이미지 파일명 목록(Joined with ',') 획득 및 리턴 
                return await PostMultipart(EndPointURL + "/images", tmpDict);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async static Task<Stream> GetImage(string filename)
        {
            try
            {
                return new MemoryStream((await client.ExecuteAsync(new RestRequest($"images/{filename.Trim()}", Method.GET))).RawBytes);
            } 
            catch (Exception e) { throw e; }
        }

        public async static Task<bool> DeleteImage(string filename)
        {
            try
            {
                return (await client.ExecuteAsync(new RestRequest($"images/{filename.Trim()}", Method.DELETE))).IsSuccessful;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion

        // 인수로 주어진 FormFile 객체 배열(Dictionary)을 포함하는 multipart/form-data 요청 보내기
        public async static Task<string> PostMultipart(string url, Dictionary<string, object> parameters)
        {
            try
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

                if (parameters != null && parameters.Count > 0)
                {
                    using (Stream reqStream = request.GetRequestStream())
                    {
                        foreach (KeyValuePair<string, object> pair in parameters)
                        {
                            reqStream.Write(boundaryBytes, 0, boundaryBytes.Length);

                            FormFile file = pair.Value as FormFile;
                            string header = "Content-Disposition: form-data; name=\"" + pair.Key + "\"; filename=\"" + file.Name + "\"\r\nContent-Type: " + file.ContentType + "\r\n\r\n";
                            byte[] bytes = Encoding.UTF8.GetBytes(header);
                            reqStream.Write(bytes, 0, bytes.Length);
                            byte[] buffer = new byte[32768];
                            int bytesRead;

                            while ((bytesRead = file.Stream.Read(buffer, 0, buffer.Length)) != 0)
                                reqStream.Write(buffer, 0, buffer.Length);

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
            catch (Exception e)
            {
                throw e;
            }
        }
    }

    public class FormFile
    {
        public string Name { get; set; }
        public string ContentType { get; set; }
        public Stream Stream { get; set; }
    }
}
