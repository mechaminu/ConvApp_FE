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
