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
            
            // 더미 유저 데이터. 이후 로그인 과정을 통해 이를 받아오도록 해야 할 것임.
            User = new User
            {
                Id = 1234567890,
                Name = "John Doe",
                ProfileImage = "https://convappdev.blob.core.windows.net/images/KurYIbkH0ie_0tQ"
            };

            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
