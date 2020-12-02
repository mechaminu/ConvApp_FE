using ConvApp.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConvApp.ViewModels
{
   public class ProfileViewModel
    {
        public int Id { get; set; }
        public UserModel User { get; set; }
        public bool Owned { get => User.Id == App.User.Id; }
    }
}
