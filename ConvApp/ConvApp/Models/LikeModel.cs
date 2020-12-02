using System;
using System.Threading.Tasks;

namespace ConvApp.Models
{
    public class LikeModel
    {
        public int Id { get; set; }
        public byte ParentType { get; set; }
        public int ParentId { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedDate { get; set; }

        public static async Task<Like> Populate(LikeModel model)
        {
            return new Like
            {
                Creator = await ApiManager.GetUser(model.UserId),
                CreatedDate = model.CreatedDate
            };
        }
    }

    public class Like
    {
        public int Id { get; set; }
        public UserBriefModel Creator { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
