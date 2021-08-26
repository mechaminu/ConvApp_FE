using ConvApp.Models;
using ConvApp.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
    public partial class EmailLoginModal : ContentPage
    {
        public EmailLoginModal()
        {
            InitializeComponent();

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            BackgroundColor = new Color(0, 0, 0, 0.2);
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            if (idEntry.Text == null || pwdEntry.Text == null)
            {
                await DisplayAlert("오류", "이메일과 비밀번호를 입력해주세요", "확인");
                return;
            }

            try
            {
                var user = await ApiManager.LoginEmailAccount(id: idEntry.Text, pwd: pwdEntry.Text);

                App.User = user;
                (Application.Current as App).MainPage = new AppShell();
            }
            catch (Exception ex)
            {
                await DisplayAlert("오류", ex.Message, "확인");
            }
        }

        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            var registerPage = new UserRegisterPage
            {
                BindingContext = new UserRegisterViewModel
                {
                    OAuthType = Models.OAuthProvider.None,
                    OAuthId = null,
                    Email = null,
                    Image = null
                }
            };

            await App.Current.MainPage.Navigation.PushModalAsync(registerPage, false);
            return;
        }
    }
}