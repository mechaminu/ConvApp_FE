using Xamarin.Forms;
using ConvApp.Services;
using System.Threading.Tasks;
using System;
using Java.Lang;
using Java.Interop;
using Android.Content;
using Com.Kakao.Sdk.Common;
using Com.Kakao.Sdk.Common.Model;
using Com.Kakao.Sdk.Auth;
using Com.Kakao.Sdk.Auth.Model;
using Com.Kakao.Sdk.Common.Util;
using Constants = Com.Kakao.Sdk.Auth.Constants;
using Uri = Android.Net.Uri;
using Android.OS;
using Exception = System.Exception;
using Android.App;
using Java.Net;

#nullable enable

namespace ConvApp.Droid.Kakao
{
    class AuthCodeClient
    {
        // 싱글턴 Property
        private static AuthCodeClient? instance = null;
        public static AuthCodeClient Instance
        {
            get =>  instance ?? new AuthCodeClient();
        }

        private IntentResolveClient intentResolveClient = new IntentResolveClient();
        private IApplicationInfo applicationInfo = KakaoSdk.Instance.ApplicationContextInfo;
        private IContextInfo contextInfo = KakaoSdk.Instance.ApplicationContextInfo;

        private string CAPRI_LOGGED_IN_ACTIVITY = "com.kakao.talk.intent.action.CAPRI_LOGGED_IN_ACTIVITY";

        public bool IsKakaoTalkLoginAvailable(Context context)
        {
            return intentResolveClient.ResolveTalkIntent(context, BaseTalkLoginIntent()) != null;
        }

        public void AuthorizeWithKakaoTalk(Context context, Action<string?, Throwable?> callback)
        {
            var clientId = applicationInfo.AppKey;
            var redirectUri = $"kakao{clientId}://oauth";
            var kaHeader = contextInfo.KaHeader;
            var talkIntent = TalkLoginIntent(clientId, redirectUri, kaHeader, null);
            var resolvedIntent = intentResolveClient.ResolveTalkIntent(context, talkIntent);

            if (resolvedIntent == null)
            {
                var error = new ClientError(ClientErrorCause.NotSupported, "kakaotalk is not installed");
                callback.Invoke(null, error);
                return;
            }

            try
            {
                context.StartActivity(
                    new Intent(context, typeof(TalkAuthCodeActivity))
                        .PutExtra(Constants.KeyLoginIntent, resolvedIntent)
                        .PutExtra(Constants.KeyRequestCode, 10012)
                        .PutExtra(Constants.KeyResultReceiver, resultReceiver(callback))
                        .AddFlags(ActivityFlags.NewTask));
            }
            catch (Throwable startActivityError)
            {
                Console.Error.WriteLine(startActivityError.Message);
                callback(null, startActivityError);
            }
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
                    var uri = resultData?.GetParcelable(Constants.KeyUrl) as Uri;
                    var code = uri?.GetQueryParameter(Constants.Code);
                    if (code != null)
                    {
                        _callback(code, null);
                    }
                    else
                    {
                        var error = uri?.GetQueryParameter(Constants.Error) as string;
                        var errorDescription = uri?.GetQueryParameter(Constants.ErrorDescription) as string;

                        AuthErrorCause RunCatching()
                        {
                            try
                            {
                                return (AuthErrorCause)KakaoJson.Instance.FromJson(error, (Java.Lang.Reflect.IType)typeof(AuthErrorCause));
                            }
                            catch
                            {
                                return AuthErrorCause.Unknown;
                            }
                        }

                        _callback(null, new AuthError(
                            (int)HttpStatus.MovedTemp,
                            RunCatching(),
                            new AuthErrorResponse(error, errorDescription)
                        ));
                    }
                }
                else if (resultCode == (int)Result.Canceled)
                {
                    var error = resultData?.GetSerializable(Constants.KeyException) as KakaoSdkError;
                    _callback(null, error);
                }
                else
                {
                    var error = new IllegalArgumentException("Unknown resultCode in RxAuthCodeClient#onReceivedResult()");
                    _callback(null, error);
                }
            }
        }

        internal Intent BaseTalkLoginIntent() =>
        new Intent().SetAction(CAPRI_LOGGED_IN_ACTIVITY).AddCategory(Intent.CategoryDefault);

        internal Intent TalkLoginIntent(string clientId, string redirUri, string kaHeader, Bundle? extras)
        {
            var consts = Constants.Instance;

            return BaseTalkLoginIntent()
                .PutExtra(consts.EXTRA_APPLICATION_KEY, clientId)
                .PutExtra(consts.EXTRA_REDIRECT_URI, redirUri)
                .PutExtra(consts.EXTRA_KA_HEADER, kaHeader)
                .PutExtra(consts.EXTRA_EXTRAPARAMS, extras);
        }

        internal Intent AuthCodeIntent(Context context, Uri fullUri, string redirUri, ResultReceiver resultReceiver)
        {
            var bundle = new Bundle();
            bundle.PutParcelable(Constants.KeyResultReceiver, resultReceiver);
            bundle.PutParcelable(Constants.KeyFullUri, fullUri);
            bundle.PutString(Constants.KeyRedirectUri, redirUri);

            return new Intent(context, typeof(AuthCodeHandlerActivity))
                .PutExtra(Constants.KeyBundle, bundle)
                .AddFlags(ActivityFlags.NewTask);
        }
    }
}