using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace ConvApp.ViewModels
{
    public class Post
    {
        public UserData User { get; set; }
        public DateTime Date { get; set; }
        public FeedBack FeedbackData { get; set; }
    }
}
