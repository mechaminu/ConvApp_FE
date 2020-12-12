using System;
using System.Threading.Tasks;

namespace ConvApp.Models
{
    public class LikeModel
    {
        public long Id { get; set; }
        public byte ParentType { get; set; }
        public long ParentId { get; set; }
        public long UserId { get; set; }
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
        public long Id { get; set; }
        public UserBriefModel Creator { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
