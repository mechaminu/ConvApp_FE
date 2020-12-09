using Com.Kakao.Sdk.Auth;
using ConvApp.Services;
using Java.Interop;
using Java.Lang;
using Kotlin.Jvm.Functions;
using System;
using System.Threading.Tasks;
using Application = Android.App.Application;

using Xamarin.Forms;
using ConvApp.Droid.Services;
using Android.App;
using Android.OS;
using Android.Content;
using Com.Kakao.Sdk.Auth.Model;

#nullable enable

[assembly: Dependency(typeof(KakaoAuthService))]
namespace ConvApp.Droid.Services
{
    public class KakaoAuthService : IKakaoAuthService
    {
        private Context context = Application.Context;

        public async Task<string> DoLogin()
        {
            var tcs = new TaskCompletionSource<string>();

            Action<string?, Throwable?> callback = (p0, p1) =>
            {
                if (p0 != null)
                    tcs.SetResult(p0);
                else
                    tcs.SetResult("nope");
            };

            context.StartActivity(new Intent(context, typeof(KakaoAuthActivity))
                .PutExtra("resultHandler",resultReceiver(callback))
                .AddFlags(ActivityFlags.NewTask));

            return await tcs.Task;
        }

        internal ResultReceiver resultReceiver(Action<string?, Throwable?> callback)
        {
            return new MyResultReceiver(new Handler(Looper.MainLooper), callback);
        }

        class MyResultReceiver : ResultReceiver
        {
            private Action<string?, Throwable?> _callback;

            public MyResultReceiver(Handler? handler, Action<string?, Throwable?> callback) : base(handler)
            {
                _callback = callback;
            }

            protected override void OnReceiveResult(int resultCode, Bundle? resultData)
            {
                Console.WriteLine($"***** AUTH CODE RESULT: {resultData}");
                if (resultCode == (int)Result.Ok)
                {
                    var result = resultData?.GetString("result");
                    if (result != null)
                        _callback(result, null);
                    else
                        _callback(null, resultData?.GetSerializable("error") as Throwable);
                }
                else if (resultCode == (int)Result.Canceled)
                    _callback(null, resultData?.GetSerializable("error") as Throwable);
                else
                    _callback(null, new IllegalArgumentException("Unknown resultCode in RxAuthCodeClient#onReceivedResult()"));
            }
        }

        [Activity(Label = "KakaoAuthActivity")]
        public class KakaoAuthActivity : Activity
        {
            private ResultReceiver? resultReceiver { get; set; }

            protected override void OnCreate(Bundle? savedInstanceState)
            {
                base.OnCreate(savedInstanceState);
                var context = this;
                var client = LoginClient.Instance;
                var extras = Intent?.Extras ?? throw new IllegalArgumentException("no extras");
                resultReceiver = extras.GetParcelable("resultHandler") as ResultReceiver;

                var callback = new KakaoCallback
                {
                    Func = (p0, p1) =>
                    {
                        if (p1 != null)
                            sendError(p1.JavaCast<Throwable>());
                        OAuthToken? authToken = p0 as OAuthToken;
                        var bundle = new Bundle();
                        bundle.PutString("result",authToken?.AccessToken);
                        resultReceiver?.Send(Result.Ok, bundle);
                        Finish();
                        return null;
                    }
                };

                if (client.IsKakaoTalkLoginAvailable(context))
                {
                    client.LoginWithKakaoTalk(context, callback);
                }

            }

            private void sendError(Throwable exception)
            {
                var bundle = new Bundle();
                bundle.PutSerializable("error", exception);

                resultReceiver?.Send(Result.Canceled, bundle);

                Finish();
            }
        }

        private class KakaoCallback : Java.Lang.Object, IFunction2
        {
            public Func<Java.Lang.Object, Java.Lang.Object, Java.Lang.Object>? Func { get; set; }

            public Java.Lang.Object Invoke(Java.Lang.Object p0, Java.Lang.Object p1)
            {
                return Func?.Invoke(p0, p1);
            }
        }
    }
}