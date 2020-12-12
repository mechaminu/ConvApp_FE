using ConvApp.Models;
using ConvApp.Services;
using ConvApp.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConvApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();

            var bc = BindingContext as LoginViewModel;

            var authStr = Preferences.Get("auth", null);
            if (authStr != null)
            {
                var authData = JsonConvert.DeserializeObject<AuthContext>(authStr);
                bc.LoginCommand.Execute(authData.Type);
            }

        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            if ( idEntry.Text == null || pwdEntry == null )
            {
                await DisplayAlert("오류", "이메일과 패스워드를 입력해주세요", "확인");
                return;
            }

            try
            {
                (BindingContext as LoginViewModel).IsBusy = true;
                App.User = await ApiManager.LoginEmailAccount(id: idEntry.Text, pwd: pwdEntry.Text);
                (Application.Current as App).MainPage = new AppShell();
            }
            catch (Exception ex) when (ex.Message == "회원정보없음")
            {
                if (await DisplayAlert("가입", "회원 정보가 없습니다. 신규 가입을 진행할까요?", "예", "아니오"))
                    await Navigation.PushModalAsync(new UserRegisterPage
                    {
                        BindingContext = new UserRegisterViewModel
                        {
                            Email = idEntry.Text,
                            Password = pwdEntry.Text,
                            Image = "logo.jpg"
                        }
                    });
            }
            catch (Exception ex)
            {
                await DisplayAlert("오류", ex.Message, "확인");
            }
            finally
            {
                (BindingContext as LoginViewModel).IsBusy = false;
            }
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            (Application.Current as App).MainPage = new AppShell();
        }
    }
}