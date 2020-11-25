using System;
using System.Threading.Tasks;

namespace ConvApp.Models
{
    public class Like
    {
        public UserModel Creator { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class LikeDTO
    {
        public int CreatorId { get; set; }
        public DateTime CreatedDate { get; set; }

        public static async Task<Like> Populate(LikeDTO dto)
        {
            return new Like
            {
                Creator = await ApiManager.GetUser(dto.CreatorId),
                CreatedDate = dto.CreatedDate
            };
        }
    }
}
