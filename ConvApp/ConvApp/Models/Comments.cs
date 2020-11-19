using System;
using System.Collections.Generic;
using System.Text;

namespace ConvApp.Models
{
    public class Comment : CommentDTO
    {
        public User Creator { get; set; }
    }

    public class CommentDTO
    {
        public int CreatorId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Text { get; set; }
    }

    public class Like
    {
        public int CreatorId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
