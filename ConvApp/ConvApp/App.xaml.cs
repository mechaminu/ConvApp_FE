using ConvApp.Views;
using System;
using Xamarin.Forms;
using ConvApp.ViewModels;
using ConvApp.Models;

namespace ConvApp
{

    public partial class App : Application
    {
        public static User User { get; private set; }

        public App()
        {
            InitializeComponent();

            GetUser();  // 이후 로그인으로 대체

            MainPage = new AppShell();
        }

        private async void GetUser()
        {
            User = await ApiManager.GetUserData(1);
        }
    }
}
