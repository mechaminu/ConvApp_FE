using ConvApp.Models;
using ConvApp.Models.Auth;
using ConvApp.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConvApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserRegisterPage : ContentPage
    {
        public UserRegisterPage()
        {
            InitializeComponent();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            var bc = BindingContext as UserRegisterViewModel;

            var dto = bc.GetDTO();
            dto.Name = username.Text;

            try
            {
                App.User = await ApiManager.RegisterUser(dto);
                Preferences.Set("auth", JsonConvert.SerializeObject(new AuthContext { LastLogined = DateTime.UtcNow, Type = (OAuthProvider)dto.OAuthProvider }));
                bc.isSuccessful = true;
                App.Current.MainPage = new AppShell();
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("오류", ex.Message, "확인");
            }
        }
    }
}