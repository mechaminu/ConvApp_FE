using Com.Kakao.Sdk.Auth;
using ConvApp.Services;
using Java.Interop;
using Java.Lang;
using Kotlin.Jvm.Functions;
using System;
using System.Threading.Tasks;

using Xamarin.Forms;
using ConvApp.Droid.Services;
using Android.App;
using Com.Kakao.Sdk.Auth.Model;

#nullable enable

[assembly: Dependency(typeof(KakaoAuthService))]
namespace ConvApp.Droid.Services
{
    public class KakaoAuthService : IKakaoAuthService
    {
        private Activity context;
        private LoginClient client;

        public KakaoAuthService()
        {
            context = MainActivity.Instance;
            client = LoginClient.Instance;
        }

        public async Task<string> DoLogin()
        {
            try
            {
                var tcs = new TaskCompletionSource<string>();

                Java.Lang.Object? callback(Java.Lang.Object p0, Java.Lang.Object p1)
                {
                    if (p1 != null)
                        tcs.SetException(p1.JavaCast<Throwable>());
                    else
                        tcs.SetResult(p0.JavaCast<OAuthToken>().AccessToken);

                    return null;
                }

                if (client.IsKakaoTalkLoginAvailable(context))
                    client.LoginWithKakaoTalk(context, new KakaoCallback { Func = callback });
                else
                    client.LoginWithKakaoAccount(context, new KakaoCallback { Func = callback });

                return await tcs.Task;
            }
            catch (System.Exception ex)
            {
                return ex.Message;
            }
        }

        class KakaoCallback : Java.Lang.Object, IFunction2
        {
            public Func<Java.Lang.Object, Java.Lang.Object, Java.Lang.Object?>? Func { get; set; }

            public Java.Lang.Object? Invoke(Java.Lang.Object p0, Java.Lang.Object p1)
            {
                return Func?.Invoke(p0, p1);
            }
        }
    }
}