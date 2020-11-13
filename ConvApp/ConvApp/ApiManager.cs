using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;

using ConvApp.Models;
using ConvApp.ViewModels;

using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;


namespace ConvApp
{
    public class ApiManager
    {

        //private static string EndPointURL = "http://convappdev.azurewebsites.net/api";
        private static string EndPointURL = "http://minuuoo.ddns.net:5000/api";
        public static string ImageEndPointURL = "https://convappdev.blob.core.windows.net/images";
        private static RestClient client = new RestClient(EndPointURL){Timeout=-1}.UseNewtonsoftJson() as RestClient;
        private static RestClient client_img = new RestClient(ImageEndPointURL) { Timeout = -1 }.UseNewtonsoftJson() as RestClient;

        public async static Task<User> GetUserData(int userId)
        {
            try
            {
                var response = await client.ExecuteAsync<User>(new RestRequest($"users/{userId}", Method.GET));
                var result = response.Data;
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #region REST API : 포스트 CRUD 구현
        /// <summary>
        /// 포스트엔트리(포스트입력) 뷰모델을 받아 백엔드에 업로드하는 메소드 
        /// </summary>
        /// <param name="post">포스트 뷰모델 객체</param>
        /// <returns>포스트 뷰모델 객체</returns>
        public async static Task<Post> UploadPosting(Posting post)
        {
            var payloadObject = post;

            try
            {
                var request = new RestRequest("postings", Method.POST)
                    .AddJsonBody(payloadObject);
                var response = await client.ExecuteAsync<Posting>(request);

                if (!response.IsSuccessful)
                    throw new InvalidOperationException("포스팅 실패");

                return await Posting.ToPost(response.Data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async static Task<List<Post>> GetPostings(int start, int end)
        {
            try
            {
                var request = new RestRequest("postings/all", Method.GET)
                    .AddParameter("start", start, ParameterType.QueryString)
                    .AddParameter("end", end, ParameterType.QueryString);

                var response = await client.ExecuteAsync<List<Posting>>(request);

                if (!response.IsSuccessful)
                    throw new InvalidOperationException($"Request Failed - CODE : {response.StatusCode}");

                var result = new List<Post>();
                foreach (var rawPosting in response.Data)
                {
                    result.Add(await Posting.ToPost(rawPosting));
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async static Task<List<RecipePost>> GetRecipes(int start, int end)
        {
            try
            {

                var request = new RestRequest("postings", Method.GET)
                    .AddParameter("start", start, ParameterType.QueryString)
                    .AddParameter("end", end, ParameterType.QueryString)
                    .AddParameter("isRecipe", true, ParameterType.QueryString);

                var response = await client.ExecuteAsync<List<Posting>>(request);

                if (!response.IsSuccessful)
                    throw new InvalidOperationException($"Request Failed - CODE : {response.StatusCode}");

                var result = new List<RecipePost>();
                foreach (var rawPosting in response.Data)
                {
                    result.Add((RecipePost)await Posting.ToPost(rawPosting));
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async static Task<List<ReviewPost>> GetReviews(int start, int end)
        {
            try {
                var request = new RestRequest("postings", Method.GET)
                    .AddParameter("start", start, ParameterType.QueryString)
                    .AddParameter("end", end, ParameterType.QueryString)
                    .AddParameter("isRecipe", false, ParameterType.QueryString);

                var response = await client.ExecuteAsync<List<Posting>>(request);

                if (!response.IsSuccessful)
                    throw new InvalidOperationException($"Request Failed - CODE : {response.StatusCode}");

                var result = new List<ReviewPost>();
                foreach(var rawPosting in response.Data)
                {
                    result.Add((ReviewPost)await Posting.ToPost(rawPosting));
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public async static Task<bool> DeletePosting(long id)
        //{
        //}

        #endregion

        #region REST API : 이미지 CR(U)D 구현
        // 이미지는 수정이 필요 없음
        public async static Task<string> UploadImage(IEnumerable<FileResult> images)
        {
            try
            {
                if (images.Count() == 0)
                    return null;

                var tmpDict = new Dictionary<string, object>();
                foreach (var img in images)
                {
                    tmpDict.Add("file_" + tmpDict.Count, new FormFile
                    {
                        ContentType = img.ContentType,
                        Stream = await img.OpenReadAsync()
                    });
                }

                // 업로드를 통해 생성된 이미지 파일명 목록(Joined with ',') 획득 및 리턴 
                return await PostMultipart(Path.Combine(EndPointURL,"images"), tmpDict);
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
                return new MemoryStream((await client_img.ExecuteAsync(new RestRequest($"images/{filename.Trim()}", Method.GET))).RawBytes);
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
