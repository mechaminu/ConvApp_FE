using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ConvApp.Services
{
    public interface IOAuthService
    {
        Task<string> DoLogin();
    }

    public interface IKakaoAuthService : IOAuthService { }
    public interface IFacebookAuthService : IOAuthService { }
    public interface IGoogleAuthService : IOAuthService { }
}
