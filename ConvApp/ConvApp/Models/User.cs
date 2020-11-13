using System.Collections.Generic;

namespace ConvApp.Models
{
    public class User
    {

        public long Id { get; set; }
        public string Name { get; set; }
        public string ProfileImage { get; set; }
        public List<Posting> CreatedPostings { get; set; }
    }
}