using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace ConvApp.Model
{
    public class Post
    { 
        public string UserName { get; set; }

        public DateTime Date { get; set; }

        public ImageSource UserImage { get; set; }

        public List<ImageSource> PostImage { get; set; }

        public string PostTitle { get; set; }

        public string PostContent { get; set; }
    }
}
