﻿using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Auth.Api.SignIn;
using Android.OS;
using Android.Runtime;
using AndroidX.AppCompat.App;
using Com.Kakao.Sdk.Common;
using ConvApp.Droid.Services;
using FFImageLoading.Forms.Platform;
using Xamarin.Facebook;

namespace ConvApp.Droid
{
    [Activity(Label = "ConvApp", Icon = "@mipmap/icon", Theme = "@style/MainTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public static ICallbackManager FacebookCallbackManager;

        public static MainActivity Instance { get; private set; }

        public static GoogleSignInClient GoogleClient { get; private set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            AppCompatDelegate.DefaultNightMode = AppCompatDelegate.ModeNightNo; // 다크모드 강제 해제
            base.OnCreate(savedInstanceState);
            Instance = this;

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            FacebookCallbackManager = CallbackManagerFactory.Create();

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            Xamarin.Forms.Forms.Init(this, savedInstanceState);

            KakaoSdk.Init(ApplicationContext, GetString(Resource.String.server_client_id));

            GoogleClient = GoogleSignIn.GetClient(this, new GoogleSignInOptions.Builder(GoogleSignInOptions.DefaultSignIn)
                .RequestIdToken(GetString(Resource.String.server_client_id))
                .RequestEmail()
                .Build());

            CachedImageRenderer.Init(true);
            CachedImageRenderer.InitImageViewHandler();

            LoadApplication(new App());
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            FacebookCallbackManager.OnActivityResult(requestCode, (int)resultCode, data);
            GoogleAuthService.OnActivityResult(requestCode, resultCode, data);
        }


        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);

        }
    }
}