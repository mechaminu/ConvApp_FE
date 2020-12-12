using ConvApp.Models;
using ConvApp.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ConvApp.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        public Command<OAuthProvider> LoginCommand =>
            new Command<OAuthProvider>(
                async (OAuthProvider loginProvider) =>
                {
                    IsBusy = true;

                    IOAuthService authService = loginProvider switch
                    {
                        OAuthProvider.Kakao => DependencyService.Get<IKakaoAuthService>(),
                        OAuthProvider.Facebook => DependencyService.Get<IFacebookAuthService>(),
                        OAuthProvider.Google => DependencyService.Get<IGoogleAuthService>(),
                        _ => throw new Exception("로그인 제공자 오류")
                    };

                    try
                    {
                        var context = Application.Current as App;
                        UserBriefModel user = null;

                        var token = await authService.DoLogin();
                        try
                        {
                            user = await ApiManager.LoginOAuthAccount(token, (byte)loginProvider);
                        }
                        catch (Exception ex) when (ex.Message == "회원정보없음")
                        {
                            var data = ex.Data["result"] as JContainer;
                            var id = data["oid"].ToString();
                            var type = data["type"].ToObject<OAuthProvider>();
                            var email = data["email"].ToString();
                            
                            Action action = ()=>IsBusy = false;

                            await context.MainPage.Navigation.PushModalAsync(new UserRegisterPage(id, type, email, action), false);
                            return;
                        }

                        // TODO proper successful login handler
                        if (user != null)
                            Preferences.Set("auth", JsonConvert.SerializeObject(new AuthContext { LastLogined = DateTime.UtcNow, Type = loginProvider }));

                        App.User = await ApiManager.GetUser(1);
                        if (!(context.MainPage is AppShell))
                            context.MainPage = new AppShell();
                    }
                    catch (Exception ex)
                    {
                        DisplayAlert("오류", ex.Message);
                        IsBusy = false;
                    }
                });

        private static void DisplayAlert(string title, string msg) =>
            (Application.Current as App).MainPage.DisplayAlert(title, msg, "확인");

        private static async void DisplayAlertAction(string title, string msg, Action action)
        {
            if (await (Application.Current as App).MainPage.DisplayAlert(title, msg, "예", "아니오"))
                action.Invoke();
        }
    }
}
