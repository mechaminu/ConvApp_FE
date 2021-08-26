using System;
using System.Collections.Generic;
using System.Text;

namespace ConvApp.Models
{
    class AuthContext
    {
        public DateTime LastLogined { get; set; }
        public OAuthProvider Type { get; set; }
        public IAuthdata Data { get; set; }
    }

    interface IAuthdata
    {

    }
}
