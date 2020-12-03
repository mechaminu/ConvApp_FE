using ConvApp.Models;
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
    public partial class Login : ContentPage
    {
        public Login()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (emailEntry.Text == null && pwdEntry.Text == null)
                {
                    emailEntry.Text = "1@paltoinfos.com";
                    pwdEntry.Text = "123456";
                }
                App.User = await ApiManager.Login(email: emailEntry.Text, password: pwdEntry.Text);
                Application.Current.MainPage = new AppShell();
            }
            catch (Exception ex)
            {
                await DisplayAlert("오류", ex.Message, "확인");
            }
            finally
            {
                emailEntry.Text = null;
                pwdEntry.Text = null;
            }
        }
    }
}