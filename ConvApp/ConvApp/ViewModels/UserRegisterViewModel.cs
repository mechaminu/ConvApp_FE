using ConvApp.Models;
using ConvApp.Models.Auth;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace ConvApp.ViewModels
{
    public class UserRegisterViewModel : ViewModelBase
    {
        public bool isSuccessful = false;

        public OAuthProvider OAuthType { get; set; }
        public string OAuthId { get; set; }
        public string Email { get; set;}
        public string Password { get; set; }

        public string imageFilename = null;
        public string Image
        {
            get => imageFilename != null ? Path.Combine(ApiManager.ImageEndPointURL, imageFilename) : null;
            set 
            {
                imageFilename = value;
                OnChange(new System.ComponentModel.PropertyChangedEventArgs("Image"));
            }
        }

        public RegisterDTO GetDTO()
        {
            return new RegisterDTO
            {
                Email = Email,
                Password = Password,
                OAuthProvider = (byte)OAuthType,
                OAuthId = OAuthId,
                Image = imageFilename
            };
        }
    }
}
