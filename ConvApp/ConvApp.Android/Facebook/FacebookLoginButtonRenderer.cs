using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ConvApp.Droid.Facebook;
using ConvApp.Utilites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Facebook;
using Xamarin.Facebook.Login;
using Xamarin.Facebook.Login.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(FacebookLoginButton), typeof(FacebookLoginButtonRenderer))]
namespace ConvApp.Droid.Facebook
{
    class FacebookLoginButtonRenderer : ViewRenderer
    {
        readonly Context context;
        bool disposed;

        public FacebookLoginButtonRenderer(Context _context) : base(_context)
        {
            context = _context;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.View> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                var fbLoginBtnView = e.NewElement as FacebookLoginButton;
                var fbLoginBtnCtrl = new LoginButton(context)
                {
                    LoginBehavior = LoginBehavior.NativeWithFallback
                };

                fbLoginBtnCtrl.SetPermissions(fbLoginBtnView.Permissions);
                fbLoginBtnCtrl.RegisterCallback(MainActivity.CallbackManager, new FacebookCallback<LoginResult>
                {
                    HandleSuccess = (r) => fbLoginBtnView.OnSuccess?.Execute(r.AccessToken.Token),
                    HandleCancel = () => fbLoginBtnView.OnCancel?.Execute(null),
                    HandleError = (err) => fbLoginBtnView.OnError?.Execute(err.Message)
                });

                SetNativeControl(fbLoginBtnCtrl);
            }

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && !this.disposed)
            {
                if (Control != null)
                {
                    (Control as LoginButton).UnregisterCallback(MainActivity.CallbackManager);
                    Control.Dispose();
                }
                disposed = true;
            }
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