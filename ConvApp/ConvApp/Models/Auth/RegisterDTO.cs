using System;
using System.Collections.Generic;
using System.Text;

namespace ConvApp.Models.Auth
{
    public class RegisterDTO
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public byte OAuthProvider { get; set; }
        public string OAuthId { get; set; }
    }
}
