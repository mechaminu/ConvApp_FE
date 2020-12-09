using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using AndroidX.AppCompat.App;
using Com.Kakao.Sdk.Auth;
using Com.Kakao.Sdk.Common.Model;
using Com.Kakao.Sdk.Common.Util;
using Java.Net;
using Uri = Android.Net.Uri;
using System;
using Java.Lang;

namespace ConvApp.Droid.Kakao
{

    [Activity(Label = "TalkAuthCodeActivity")]
    public class TalkAuthCodeActivity : AppCompatActivity
    {
        private ResultReceiver resultReceiver;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_talk_auth_code);

            try
            {
                var extras = Intent.Extras ?? throw new IllegalArgumentException("no extras");
                resultReceiver = extras.GetParcelable(Constants.KeyResultReceiver) as ResultReceiver;
                var requestCode = extras.GetInt(Constants.KeyRequestCode);
                Console.WriteLine($"requestCode: {requestCode}");
                var loginIntent = extras.GetParcelable(Constants.KeyLoginIntent) as Intent;
                Console.WriteLine($"loginIntent: {loginIntent}");

                StartActivityForResult(loginIntent, requestCode);
            }
            catch (Throwable e)
            {
                Console.Error.WriteLine(e.Message);
                var err = new ClientError(ClientErrorCause.Unknown, "Client-side error");
                err.InitCause(e);
                sendError(err);
            }
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            var bundle = new Bundle();

            if (data == null || resultCode == Result.Canceled)
            {
                sendError(exception: new ClientError(ClientErrorCause.Cancelled, "Client-side error"));
                return;
            }

            if (resultCode == Result.Ok)
            {
                var extras = data.Extras;
                if (extras == null)
                {
                    sendError(new ClientError(ClientErrorCause.Unknown, "No result from KakaoTalk."));
                    return;
                }

                var error = extras.GetString(EXTRA_ERROR_TYPE);
                var errorDescription = extras.GetString(EXTRA_ERROR_DESCRIPTION);
                if (error == "access_denied")
                {
                    sendError(new ClientError(ClientErrorCause.Cancelled, "client-side error"));
                    return;
                }
                if (error != null)
                {
                    var cause = (AuthErrorCause)KakaoJson.Instance.FromJson(error, (Java.Lang.Reflect.IType)typeof(AuthErrorCause)) ?? AuthErrorCause.Unknown;
                    sendError(new AuthError((int)HttpStatus.MovedTemp, cause, new AuthErrorResponse(error, errorDescription ?? "no error description")));
                    return;
                }
                bundle.PutParcelable(Constants.KeyUrl, Uri.Parse(extras.GetString(Constants.Instance.EXTRA_REDIRECT_URL)));

                resultReceiver.Send(Result.Ok, bundle);

                Finish();
                OverridePendingTransition(0, 0); 
                return;
            }

            throw new IllegalArgumentException("");
        }

        private void sendError(KakaoSdkError exception)
        {
            var bundle = new Bundle();
            bundle.PutSerializable(Constants.KeyException, exception);

            resultReceiver.Send(Result.Canceled, bundle);

            Finish();
        }

        readonly string NOT_SUPPORTED_ERROR = "NotSupportedError";
        readonly string UNKNOWN_ERROR = "UnknownError";
        readonly string PROTOCOL_ERROR = "ProtocolError";
        readonly string APPLICATION_ERROR = "ApplicationError";
        readonly string AUTH_CODE_ERROR = "AuthCodeError";
        readonly string CLIENT_INFO_ERROR = "ClientInfoError";

        readonly string EXTRA_ERROR_TYPE = "com.kakao.sdk.talk.error.type";
        readonly string EXTRA_ERROR_DESCRIPTION = "com.kakao.sdk.talk.error.description";


    }
}