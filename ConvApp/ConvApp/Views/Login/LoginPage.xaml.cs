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
                var authInfo = JsonConvert.DeserializeObject<AuthContext>(authStr);
                if (authInfo.Type != OAuthProvider.None)
                    bc.LoginCommand.Execute(authInfo.Type);
            }
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await App.Current.MainPage.Navigation.PushModalAsync(new EmailLoginModal(), false);
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            (Application.Current as App).MainPage = new AppShell();
        }

    }
}