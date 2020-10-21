using System;
using Xamarin.Forms;

namespace ConvApp.Model
{
    public class Post
    {
        public ImageSource Image { get; set; }
        public string TextContent { get; set; }
        public DateTime Date { get; set; }
    }
}
