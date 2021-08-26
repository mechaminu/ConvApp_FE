using Android.App;
using Android.Content;
using Android.Gms.Auth.Api.SignIn;
using Android.Gms.Common.Apis;
using Android.Gms.Extensions;
using Android.Gms.Tasks;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ConvApp.Droid.Services;
using ConvApp.Services;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(GoogleAuthService))]
namespace ConvApp.Droid.Services
{
    class GoogleAuthService : IGoogleAuthService
    {
        const int RC_SIGN_IN = 9001;

        private Activity context;
        private GoogleSignInClient client;
        protected static TaskCompletionSource<string> tcs;

        public GoogleAuthService()
        {
            context = MainActivity.Instance;
            client = MainActivity.GoogleClient;
        }

        public async Task<string> DoLogin()
        {
            try
            {
                var silentResult = await client.SilentSignInAsync();
                if (silentResult != null)
                    return silentResult.IdToken;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            tcs = new TaskCompletionSource<string>();
            var signInIntent = client.SignInIntent;
            context.StartActivityForResult(signInIntent, RC_SIGN_IN);
            return await tcs.Task;

        }

        public static async void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            if (requestCode == RC_SIGN_IN)
            {
                try
                {
                    var account = await GoogleSignIn.GetSignedInAccountFromIntentAsync(data);
                    var token = account.IdToken;
                    tcs.SetResult(token);
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            }
        }
    }
}