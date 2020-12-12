using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ConvApp.Droid.Services;
using ConvApp.Services;
using Org.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Facebook;
using Xamarin.Facebook.Login;
using Xamarin.Forms;

[assembly: Dependency(typeof(FacebookAuthService))]
namespace ConvApp.Droid.Services
{
    public class FacebookAuthService : IFacebookAuthService
    {
        private Activity context;
        private LoginManager client;

        public FacebookAuthService()
        {
            context = MainActivity.Instance;
            client = LoginManager.Instance;
        }

        public async Task<string> DoLogin()
        {
            var tcs = new TaskCompletionSource<string>();
            client.RegisterCallback(MainActivity.FacebookCallbackManager, new FacebookCallback<LoginResult>
            {
                HandleSuccess = (r) => tcs.SetResult(r.AccessToken.Token),
                HandleCancel = () => tcs.SetCanceled(),
                HandleError = (err) => tcs.SetException(err)
            });

            client.LogInWithReadPermissions(context, new string[] { "email" });
            
            return await tcs.Task;
        }

        class FacebookCallback<TResult> : Java.Lang.Object, IFacebookCallback where TResult : Java.Lang.Object
        {
            public Action HandleCancel { get; set; }
            public Action<FacebookException> HandleError { get; set; }
            public Action<TResult> HandleSuccess { get; set; }

            public void OnCancel() => HandleCancel?.Invoke();

            public void OnError(FacebookException error) => HandleError?.Invoke(error);

            public void OnSuccess(Java.Lang.Object result) => HandleSuccess?.Invoke(result.JavaCast<TResult>());
        }
    }
}