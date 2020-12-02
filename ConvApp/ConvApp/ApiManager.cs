using ConvApp.Models;
using ConvApp.ViewModels;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace ConvApp
{
    // Repository pattern 적용사례에 해당되는듯?
    public class ApiManager
    {
        //private static readonly string EndPointURL = "http://minuuoo.ddns.net:5000/api";
        private static readonly string EndPointURL = "http://convappdev.azurewebsites.net/api";
        public static readonly string ImageEndPointURL = "https://convappdev.blob.core.windows.net/images";
        private static readonly RestClient client = new RestClient(EndPointURL) { Timeout = -1 }.UseNewtonsoftJson(new JsonSerializerSettings
        {
            PreserveReferencesHandling = PreserveReferencesHandling.All
        }) as RestClient;
        private static readonly RestClient client_img = new RestClient(ImageEndPointURL) { Timeout = -1 }.UseNewtonsoftJson(new JsonSerializerSettings
        {
            PreserveReferencesHandling = PreserveReferencesHandling.All
        }) as RestClient;

        public async static Task<UserBriefModel> Login(string email, string password)
        {
            var response = await client.ExecuteAsync<UserBriefModel>(new RestRequest("users/login", Method.POST)
                .AddJsonBody(new { Email = email, Password = password }));

            if (!response.IsSuccessful)
                throw new Exception("로그인 실패. 아이디와 비밀번호를 확인하세요");

            return response.Data;
        }

        public async static Task RefreshRank()
        {
            await client.ExecuteAsync(new RestRequest("ranking", Method.GET));
        }

        public async static Task<UserDetailModel> GetUserDetail(int id)
        {
            return (await client.ExecuteAsync<UserDetailModel>(new RestRequest($"users/detail/{id}", Method.GET))).Data;
        }

        public async static Task<SearchResultModel> GetSearch(string queryStr)
        {
            return (await client.ExecuteAsync<SearchResultModel>(new RestRequest("search", Method.GET).AddQueryParameter("search", queryStr))).Data;
        }

        #region Products
        public async static Task<ProductModel> GetProduct(int id)
        {
            var request = new RestRequest($"products/{id}", Method.GET);

            var response = await client.ExecuteAsync<ProductModel>(request);

            if (response == null || !response.IsSuccessful)
                throw new InvalidOperationException($"제품{id} 정보 획득 실패!");

            return response.Data;
        }

        public async static Task<List<ProductModel>> GetProducts(int? store = null, int? category = null)
        {
            var request = new RestRequest("products", Method.GET);

            if (store != null)
                request.AddQueryParameter("store", store + "");
            if (category != null)
                request.AddQueryParameter("category", category + "");

            return (await client.ExecuteAsync<List<ProductModel>>(request)).Data;
        }

        public async static Task<List<ProductModel>> GetHotProducts(int? store = null, int? category = null)
        {
            var request = new RestRequest("products/hot", Method.GET);

            if (store != null)
                request.AddQueryParameter("store", store + "");
            if (category != null)
                request.AddQueryParameter("category", category + "");

            return (await client.ExecuteAsync<List<ProductModel>>(request)).Data;
        }
        #endregion

        #region Feedbacks
        public async static Task<(List<CommentViewModel>, List<Like>)> GetFeedbacks(byte type, int id)
        {
            var request = new RestRequest("feedbacks", Method.GET)
                    .AddQueryParameter("type", $"{type}")
                    .AddQueryParameter("id", $"{id}");

            var response = await client.ExecuteAsync<FeedbackDTO>(request);

            var comments = new List<CommentViewModel>();
            foreach (var cmtModel in response.Data.Comments)
            {
                var cmt = await CommentModel.Populate(cmtModel);
                await cmt.Feedback.Refresh();
                comments.Add(cmt);
            }

            var likes = new List<Like>();
            foreach (var likeModel in response.Data.Likes)
            {
                var like = await LikeModel.Populate(likeModel);
                likes.Add(like);
            }

            return (comments, likes);
        }

        public class FeedbackDTO
        {
            public List<CommentModel> Comments { get; set; }
            public List<LikeModel> Likes { get; set; }
        }

        public async static Task<List<Like>> GetLikes(byte type, int id)
        {
            var request = new RestRequest("feedbacks/like", Method.GET)
                    .AddQueryParameter("type", $"{type}")
                    .AddQueryParameter("id", $"{id}");

            var response = await client.ExecuteAsync<List<LikeModel>>(request);

            var likes = new List<Like>();
            foreach (var likeDTO in response.Data)
                likes.Add(await LikeModel.Populate(likeDTO));

            return likes;
        }

        public async static Task PostLike(byte type, int id)
        {
            var request = new RestRequest("feedbacks/like", Method.POST)
                .AddJsonBody(new LikeModel { ParentType = type, ParentId = id, UserId = App.User.Id });

            var response = await client.ExecuteAsync<List<LikeModel>>(request);

            if (response == null || !response.IsSuccessful)
                throw new InvalidOperationException("like posting failed");
        }

        public async static Task DeleteLike(byte type, int id)
        {
            var response = await client.ExecuteAsync<List<LikeModel>>(new RestRequest("feedbacks/like", Method.DELETE)
                .AddJsonBody(new LikeModel { ParentType = type, ParentId = id, UserId = App.User.Id }));

            if (response == null || !response.IsSuccessful)
                throw new InvalidOperationException("like deletion failed");
        }

        public async static Task<List<CommentViewModel>> GetComments(byte type, int id)
        {
            var request = new RestRequest("feedbacks/comment", Method.GET)
                .AddQueryParameter("type", type + "")
                .AddQueryParameter("id", id + "");

            var response = await client.ExecuteAsync<List<CommentModel>>(request);

            var comments = new List<CommentViewModel>();
            foreach (var cmtDTO in response.Data)
                comments.Add(await CommentModel.Populate(cmtDTO));

            return comments;
        }

        public async static Task PostComment(byte type, int id, string text)
        {
            var request = new RestRequest("feedbacks/comment", Method.POST)
                .AddJsonBody(new CommentModel { ParentType = type, ParentId = id, UserId = App.User.Id, Text = text });

            var response = await client.ExecuteAsync<CommentModel>(request);

            if (response == null || !response.IsSuccessful)
                throw new InvalidOperationException("comment posting failed");
        }

        public async static Task DeleteComment(int id)
        {
            var response = await client.ExecuteAsync(new RestRequest("feedbacks/comment", Method.DELETE)
                .AddQueryParameter("id", $"{id}"));

            if (response == null || !response.IsSuccessful)
                throw new InvalidOperationException("comment deletion failed");
        }

        public async static Task AddView(byte type, int id)
        {
            await client.ExecuteAsync(new RestRequest("feedbacks/view", Method.POST)
                .AddJsonBody(new { ParentType = type, ParentId = id, UserId = App.User.Id }));
        }
        #endregion

        #region Users
        // 유저 데이터 획득
        public async static Task<UserBriefModel> GetUser(int userId)
        {
            try
            {
                var response = await client.ExecuteAsync<UserBriefModel>(new RestRequest($"users/{userId}", Method.GET));
                var result = response.Data;

                return result;
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region Postings
        /// <summary>
        /// 포스트엔트리(포스트입력) 뷰모델을 받아 백엔드에 업로드하는 메소드 
        /// </summary>
        /// <param name="post">포스트 뷰모델 객체</param>
        /// <returns>포스트 뷰모델 객체</returns>
        public async static Task PostPosting(PostingModel post)
        {
            var payloadObject = post;
            try
            {
                var request = new RestRequest("postings", Method.POST)
                    .AddJsonBody(payloadObject);
                var response = await client.ExecuteAsync<PostingModel>(request);

                if (!response.IsSuccessful)
                    throw new InvalidOperationException("포스팅 실패");
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 종류에 무방하게 포스팅 객체를 쿼리, 최신 순으로 리스트를 리턴.
        /// 최신순 정렬된 포스팅 목록에서 일부를 추출하기 위한 순번을 정해야 하며, 목록의 길이는 end-start개가 될 것임 
        /// </summary>
        /// <param name="start">시작 순번</param>
        /// <param name="end">끝 순번</param>
        /// <returns></returns>
        public async static Task<List<PostingViewModel>> GetPostings(DateTime? time = null, int? page = null, byte? type = null)
        {
            try
            {
                var request = new RestRequest("postings", Method.GET)
                    .AddParameter("time", time.HasValue ? time.Value.ToString("o") : DateTime.UtcNow.ToString("o"))
                    .AddParameter("page", page ?? 0);

                if (type.HasValue)
                    request.AddQueryParameter("type", $"{type.Value}");

                var response = await client.ExecuteAsync<List<PostingModel>>(request);
                if (!response.IsSuccessful)
                    throw new InvalidOperationException($"Request Failed - CODE : {response.StatusCode}");

                var result = new List<PostingViewModel>();
                foreach (var rawPosting in response.Data)
                {
                    result.Add(await PostingModel.Populate(rawPosting));
                }
                return result;
            }
            catch
            {
                throw;
            }
        }

        public async static Task<List<PostingViewModel>> GetHotPostings(DateTime? time = null, int? page = null, byte? type = null)
        {
            try
            {
                var request = new RestRequest("postings/hot", Method.GET)
                    .AddParameter("time", time.HasValue ? time.Value.ToString("o") : DateTime.UtcNow.ToString("o"))
                    .AddParameter("page", page ?? 0);

                if (type.HasValue)
                    request.AddQueryParameter("type", $"{type.Value}");

                var response = await client.ExecuteAsync<List<PostingModel>>(request);
                if (!response.IsSuccessful)
                    throw new InvalidOperationException($"Request Failed - CODE : {response.StatusCode}");

                var result = new List<PostingViewModel>();
                foreach (var rawPosting in response.Data)
                {
                    result.Add(await PostingModel.Populate(rawPosting));
                }
                return result;
            }
            catch
            {
                throw;
            }
        }

        public class PostingQueryOption
        {
            public DateTime baseTime;
            public int page;
        }

        public class PostingQueryResult
        {
            public int page;
            public int maxPage;
            public List<PostingModel> postings;
        }
        #endregion

        #region Images
        // 이미지 업로드
        public async static Task<string> UploadImage(IEnumerable<FileResult> images)
        {
            try
            {
                if (!images.Any())
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
                return await PostMultipart(Path.Combine(EndPointURL, "images"), tmpDict);
            }
            catch
            {
                throw;
            }
        }

        // 이미지는 직접 이미지 Azure storage로부터 REST API 활용하여 다운로드 가능
        public async static Task<Stream> GetImage(string filename)
        {
            try
            {
                return new MemoryStream((await client_img.ExecuteAsync(new RestRequest($"images/{filename.Trim()}", Method.GET))).RawBytes);
            }
            catch
            {
                throw;
            }
        }

        public async static Task<bool> DeleteImage(string filename)
        {
            try
            {
                return (await client.ExecuteAsync(new RestRequest($"images/{filename.Trim()}", Method.DELETE))).IsSuccessful;
            }
            catch
            {
                throw;
            }
        }
        #endregion

        // 인수로 주어진 FormFile 객체 배열(Dictionary)을 포함하는 multipart/form-data 요청 보내기
        // 이거 Restsharp로 못하나?
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
                    using Stream reqStream = request.GetRequestStream();
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

                using WebResponse res = request.GetResponse();
                using Stream resStream = res.GetResponseStream();
                using StreamReader reader = new StreamReader(resStream);
                return await reader.ReadToEndAsync();
            }
            catch
            {
                throw;
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
