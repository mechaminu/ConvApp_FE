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

        public static async Task<Comment> Populate(CommentDTO dto)
        {
            return new Comment
            {
                Creator = await ApiManager.GetUser(dto.CreatorId),
                CreatedDate = dto.CreatedDate,
                Text = dto.Text
            };
        }
    }
}
