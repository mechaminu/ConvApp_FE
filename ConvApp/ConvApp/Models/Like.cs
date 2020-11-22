using System;
using System.Threading.Tasks;

namespace ConvApp.Models
{
    public class Like
    {
        public User Creator { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class LikeDTO
    {
        public int CreatorId { get; set; }
        public DateTime CreatedDate { get; set; }

        public async Task<Like> PopulateDTO()
        {
            return new Like
            {
                Creator = await ApiManager.GetUserData(this.CreatorId),
                CreatedDate = this.CreatedDate
            };
        }
    }
}
