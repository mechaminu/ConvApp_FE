using System;
using System.Collections.Generic;
using System.Text;

namespace ConvApp.Models
{
    public class Comment
    {
        public int CreatorId { get; set; }
        public User Creator { get; set; }
        public string Text { get; set; }

        public List<Comment> Comments { get; set; }
        public List<Likes> Likes { get; set; }
    }

    public class Likes
    {

    }
}
