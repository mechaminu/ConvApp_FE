using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using Com.Kakao.Sdk.Auth;
using Com.Kakao.Sdk.Common.Model;
using Com.Kakao.Sdk.Common.Util;
using System;
using Uri = Android.Net.Uri;

namespace ConvApp.Droid.Kakao
{
    [Activity(Label = "AuthCodeHandlerActivity")]
    public class AuthCodeHandlerActivity : AppCompatActivity
    {
        private Lazy<ResultReceiver> resultReceiver;
        private Lazy<Uri> fullUri;
        private IServiceConnection customTabsConnection = null;
        private bool customTabsOpened = false;        

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            try
            {
                Intent.Extras.GetBundle(Constants.KeyBundle);

                if (Intent.Extras != null)
                {
                    resultReceiver = new Lazy<ResultReceiver>(Intent.Extras.GetParcelable(Constants.KeyResultReceiver) as ResultReceiver);
                    fullUri = new Lazy<Uri>(Intent.Extras.GetParcelable(Constants.KeyFullUri) as Uri);
                }
                else
                    throw new Exception();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                throw;
            }
        }

        protected override void OnResume()
        {
            base.OnResume();

            if (!customTabsOpened)
            {
                customTabsOpened = true;
                if (fullUri.IsValueCreated)
                    openChromeCustomTab(fullUri.Value);
                else
                    sendError(new ClientError(ClientErrorCause.IllegalState, "/oauth/authorize url has been not initialized."));
            }
            else
                sendError(new ClientError(ClientErrorCause.Cancelled, "/oauth/authorize cancelled."));
        }

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);

            if (intent.Data != null)
                sendOk(intent.Data);
            else
                sendError(new ClientError(ClientErrorCause.Cancelled, "/oauth/authorize cancelled."));
        }

        private void openChromeCustomTab(Uri uri)
        {
            Console.WriteLine($"Authorize Uri : {uri}");
            try
            {
                customTabsConnection = KakaoCustomTabsClient.Instance.OpenWithDefault(this, uri);
            }
            catch (Java.Lang.UnsupportedOperationException ex)
            {
                Console.Error.WriteLine(ex.Message);
                KakaoCustomTabsClient.Instance.Open(this, uri);
            }
        }

        private void sendError(KakaoSdkError exception)
        {
            if (resultReceiver.IsValueCreated)
            {
                var bundle = new Bundle();
                bundle.PutSerializable(Constants.KeyException, exception);

                resultReceiver.Value.Send(Result.Canceled, bundle);
            }
            
            Finish();
        }

        private void sendOk(Uri uri)
        {
            if (resultReceiver.IsValueCreated)
            {
                var bundle = new Bundle();
                bundle.PutParcelable(Constants.KeyUrl, uri);

                resultReceiver.Value.Send(Result.Ok, bundle);
            }

            Finish();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            customTabsConnection?.Dispose();
        }
    }
}