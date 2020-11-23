using System;
using System.Threading.Tasks;

namespace ConvApp.Models
{
    public class Comment
    {
        public UserModel Creator { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Text { get; set; }
    }

    public class CommentDTO
    {
        public int CreatorId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Text { get; set; }

        public async Task<Comment> PopulateDTO()
        {
            return new Comment
            {
                Creator = await ApiManager.GetUser(this.CreatorId),
                CreatedDate = this.CreatedDate,
                Text = Text
            };
        }
    }
}
