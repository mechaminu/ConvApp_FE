using ConvApp.Models;
using ConvApp.ViewModels;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;
using System;
using System.Collections.Generic;
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
        //private static string EndPointURL = "http://minuuoo.ddns.net:5000/api";
        private static string EndPointURL = "http://convappdev.azurewebsites.net/api";
        public static string ImageEndPointURL = "https://convappdev.blob.core.windows.net/images";
        private static RestClient client = new RestClient(EndPointURL) { Timeout = -1 }.UseNewtonsoftJson() as RestClient;
        private static RestClient client_img = new RestClient(ImageEndPointURL) { Timeout = -1 }.UseNewtonsoftJson() as RestClient;

        public async static Task<ProductDetailViewModel> GetProductInformation(int id)
        {
            var product = await GetProduct(id);
            var reviews = new List<ReviewPostingViewModel>();
            product.Postings.ForEach(async e =>
            {
                if (e.PostingType == (byte)PostingTypes.REVIEW)
                    reviews.Add((ReviewPostingViewModel)await e.PopulateDTO());
            });

            return new ProductDetailViewModel
            {
                Id = product.Id,
                CreatedDate = product.CreatedDate,
                ModifiedDate = product.ModifiedDate,
                Image = product.Image,
                Name = product.Name,
                Price = product.Price,
                Rank = "123123",
                Rate = "4.5",
                Calory = "9999",
                Reviewpostlist = reviews
            };
        }

        public async static Task<List<ProductDTO>> GetProducts(int? store = null, int? category = null)
        {
            var request = new RestRequest("products", Method.GET);

            if (store != null)
                request.AddQueryParameter("store", store + "");
            if (category != null)
                request.AddQueryParameter("category", category + "");

            return (await client.ExecuteAsync<List<ProductDTO>>(request)).Data;
        }

        public async static Task<ProductDTO> GetProduct(int id)
        {
            var request = new RestRequest($"products/{id}", Method.GET);

            var response = await client.ExecuteAsync<ProductDTO>(request);  // TODO NullException 발생하는 듯... 디버깅 할 것

            if (!response.IsSuccessful)
                return null;

            return response.Data;
        }

        #region FEEDBACK API
        public async static Task<FeedbackViewModel> GetFeedback(byte type, int id)
        {
            var request = new RestRequest($"feedbacks", Method.GET)
                .AddQueryParameter("type", $"{type}")
                .AddQueryParameter("id", $"{id}")
                .AddQueryParameter("time", $"{DateTime.UtcNow}");

            var response = await client.ExecuteAsync<FeedbackDTO>(request);

            var comments = new List<Comment>();

            foreach (var c in response.Data.comments)
            {
                comments.Add(await c.PopulateDTO());
            }

            var likes = new List<Like>();

            foreach (var l in response.Data.likes)
            {
                likes.Add(await l.PopulateDTO());
            }

            return new FeedbackViewModel(type, id);
        }

        public async static Task<List<Like>> GetLikes(byte type, int id)
        {
            var request = new RestRequest("feedbacks/like", Method.GET)
                    .AddQueryParameter("type", $"{type}")
                    .AddQueryParameter("id", $"{id}");

            var likes = new List<Like>();
            (await client.ExecuteAsync<List<LikeDTO>>(request)).Data
                .ForEach(async e => likes.Add(await e.PopulateDTO()));

            return likes;
        }

        public async static Task<List<Like>> PostLike(byte type, int id)
        {
            var request = new RestRequest("feedbacks/like", Method.POST)
                    .AddQueryParameter("type", $"{type}")
                    .AddQueryParameter("id", $"{id}")
                    .AddJsonBody(new LikeDTO { CreatorId = App.User.Id });

            var response = await client.ExecuteAsync<List<LikeDTO>>(request);

            var likes = new List<Like>();
            response.Data.ForEach(async e => likes.Add(await e.PopulateDTO()));

            return likes;
        }

        public async static Task<List<Like>> DeleteLike(byte type, int id)
        {
            var request = new RestRequest("feedbacks/like", Method.DELETE)
                .AddQueryParameter("type", $"{type}")
                .AddQueryParameter("id", $"{id}")
                .AddJsonBody(new LikeDTO { CreatorId = App.User.Id });

            var likes = new List<Like>();
            (await client.ExecuteAsync<List<LikeDTO>>(request)).Data
                .ForEach(async e => likes.Add(await e.PopulateDTO()));

            return likes;
        }

        public async static Task<List<Comment>> GetComments(byte type, int id, DateTime time, int page)
        {
            var request = new RestRequest("feedbacks/like", Method.GET)
                    .AddQueryParameter("type", $"{type}")
                    .AddQueryParameter("id", $"{id}")
                    .AddQueryParameter("time", $"{time}")
                    .AddQueryParameter("page", $"{page}");

            var comments = new List<Comment>();
            (await client.ExecuteAsync<List<CommentDTO>>(request)).Data
                .ForEach(async e => comments.Add(await e.PopulateDTO()));

            return comments;
        }

        class FeedbackDTO
        {
            public List<CommentDTO> comments { get; set; }
            public List<LikeDTO> likes { get; set; }
        }
        #endregion

        #region USER API
        // 유저 데이터 획득
        public async static Task<User> GetUserData(int userId)
        {
            try
            {
                var response = await client.ExecuteAsync<User>(new RestRequest($"users/{userId}", Method.GET));
                var result = response.Data;
                result.ProfileImage = result.ProfileImage != null ? Path.Combine(ImageEndPointURL, result.ProfileImage) : null;
                return result;
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region POSTING API
        /// <summary>
        /// 포스트엔트리(포스트입력) 뷰모델을 받아 백엔드에 업로드하는 메소드 
        /// </summary>
        /// <param name="post">포스트 뷰모델 객체</param>
        /// <returns>포스트 뷰모델 객체</returns>
        public async static Task PostPosting(PostingDTO post)
        {
            var payloadObject = post;
            try
            {
                var request = new RestRequest("postings", Method.POST)
                    .AddJsonBody(payloadObject);
                var response = await client.ExecuteAsync<PostingDTO>(request);

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
        public async static Task<List<PostingDetailViewModel>> GetPostings(byte? type = null)
        {
            try
            {
                var request = new RestRequest("postings", Method.GET);

                if (type != null)
                    request.AddQueryParameter("type", $"{type}");

                var response = await client.ExecuteAsync<List<PostingDTO>>(request);
                if (!response.IsSuccessful)
                    throw new InvalidOperationException($"Request Failed - CODE : {response.StatusCode}");

                var result = new List<PostingDetailViewModel>();
                foreach (var rawPosting in response.Data)
                {
                    result.Add(await rawPosting.PopulateDTO());
                }
                return result;
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region IMAGE API
        // 이미지 업로드
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
