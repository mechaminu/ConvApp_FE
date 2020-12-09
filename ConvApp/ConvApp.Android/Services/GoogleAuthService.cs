using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ConvApp.Droid.Services;
using ConvApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(GoogleAuthService))]
namespace ConvApp.Droid.Services
{
    class GoogleAuthService : IGoogleAuthService
    {
        public async Task<string> DoLogin()
        {
            await Task.Delay(1000);

            return "google";
        }
    }
}