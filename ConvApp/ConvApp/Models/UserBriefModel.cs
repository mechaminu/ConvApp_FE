using ConvApp.ViewModels;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ConvApp.Models
{
    public class UserBriefModel
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

    public class UserDetailModel : UserBriefModel
    {
        public List<PostingModel> Postings { get; set; }
        public List<PostingModel> LikedPostings { get; set; }
        public List<ProductModel> LikedProducts { get; set; }
        public List<UserBriefModel> FollowingUsers { get; set; }
        public List<UserBriefModel> FollowerUsers { get; set; }

        public static async Task<UserDetailViewModel> Populate(UserDetailModel model)
        {
            var postings = new List<PostingViewModel>();
            foreach (var elem in model.Postings)
                postings.Add(await PostingModel.Populate(elem));

            var likedPostings = new List<PostingViewModel>();
            foreach (var elem in model.LikedPostings)
                likedPostings.Add(await PostingModel.Populate(elem));

            var likedProducts = new List<ProductViewModel>();
            foreach (var elem in model.LikedProducts)
                likedProducts.Add(await ProductModel.Populate(elem));

            return new UserDetailViewModel
            {
                Id = model.Id,
                Name = model.Name,
                Image = model.Image,
                Postings = postings,
                LikedPostings = likedPostings,
                LikedProducts = likedProducts,
                FollowingUsers = model.FollowingUsers,
                FollowerUsers = model.FollowerUsers
            };
        }
    }
}