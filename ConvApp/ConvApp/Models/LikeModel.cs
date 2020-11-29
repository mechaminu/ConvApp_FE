using System;
using System.Threading.Tasks;

namespace ConvApp.Models
{
    public class LikeModel
    {
        public int CreatorId { get; set; }
        public DateTime CreatedDate { get; set; }

        public static async Task<Like> Populate(LikeModel model)
        {
            return new Like
            {
                Creator = await ApiManager.GetUser(model.CreatorId),
                CreatedDate = model.CreatedDate
            };
        }
    }

    public class Like
    {
        public UserModel Creator { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
