using System.Collections.Generic;

namespace ConvApp.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ProfileImage { get; set; }    // 이미지 URL
        public List<Posting> CreatedPostings { get; set; }
    }
}