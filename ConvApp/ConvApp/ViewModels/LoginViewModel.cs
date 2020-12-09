using ConvApp.Models;
using ConvApp.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace ConvApp.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private Command<LoginProvider?> loginCommand;
        public Command<LoginProvider?> LoginCommand =>
            loginCommand ?? new Command<LoginProvider?>(
                async (LoginProvider? loginProvider) =>
                {
                    IsBusy = true;

                    IOAuthService authService = loginProvider switch
                    {
                        LoginProvider.Email => new EmailAuthService(),
                        LoginProvider.Kakao => DependencyService.Get<IKakaoAuthService>(),
                        LoginProvider.Facebook => DependencyService.Get<IFacebookAuthService>(),
                        LoginProvider.Google => DependencyService.Get<IGoogleAuthService>(),
                        _ => throw new Exception("로그인 제공자 오류")
                    };

                    try
                    {
                        var result = await authService.DoLogin();

                        DisplayAlert("인증정보", result);
                    }
                    catch (Exception ex)
                    {
                        DisplayAlert("오류", ex.Message);
                    }
                    finally
                    {
                        App.User = await ApiManager.GetUser(1);
                        (Application.Current as App).MainPage = new AppShell();
                    }
                });

        private static void DisplayAlert(string title, string msg) =>
            (Application.Current as App).MainPage.DisplayAlert(title, msg, "확인");
    }
}
