using ConvApp.Views;
using System;
using Xamarin.Forms;
using ConvApp.Model;

namespace ConvApp
{

    public partial class App : Application
    {
        public static UserData User { get; private set; }

        public App()
        {
            InitializeComponent();
            
            // 더미 유저 데이터. 이후 로그인 과정을 통해 이를 받아오도록 해야 할 것임.
            User = new UserData
            {
                Id = 123456789,
                Name = "John Doe",
                ProfileImage = null
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
