using System.Collections.Generic;
using System.IO;

namespace ConvApp.Models
{
    public class UserBreifModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageFilename { get; set; }

        public string Image
        {
            get => ImageFilename != null
                ? Path.Combine(ApiManager.ImageEndPointURL, ImageFilename)
                : string.Empty;
        }
    }

    public class UserDetailModel : UserBreifModel
    {
        public List<PostingModel> LikedPostings { get; set; }
        public List<ProductModel> LikedProducts { get; set; }
        public List<UserBreifModel> FollowingUsers { get; set; }
        public List<UserBreifModel> FollowerUsers { get; set; }
    }
}