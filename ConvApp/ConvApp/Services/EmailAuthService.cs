using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ConvApp.Services
{
    class EmailAuthService : IOAuthService
    {
        public async Task<string> DoLogin()
        {
            await Task.Delay(1000);
            return "email";
        }
    }
}
