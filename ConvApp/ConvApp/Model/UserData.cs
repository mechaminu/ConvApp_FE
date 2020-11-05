using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ConvApp.Model
{
    public class UserData
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public ImageSource ProfileImage { get; set; }
    }
}
