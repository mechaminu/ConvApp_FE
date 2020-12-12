using ConvApp.Models;
using ConvApp.Services;
using ConvApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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