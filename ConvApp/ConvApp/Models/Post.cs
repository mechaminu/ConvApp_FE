using System;
using System.Collections.Generic;
using Xamarin.Forms;
using ConvApp.ViewModels;

namespace ConvApp.Models
{
    public class Post
    { 
        public string UserName { get; set; }

        public DateTime Date { get; set; }

        public ImageSource UserImage { get; set; }

        public List<PostImageEntry> PostImage { get; set; }

        public string PostTitle { get; set; }

        public string PostContent { get; set; }
    }
}
