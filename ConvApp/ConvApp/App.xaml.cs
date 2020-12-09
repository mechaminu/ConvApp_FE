using ConvApp.Models;
using ConvApp.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ConvApp
{

    public partial class App : Application
    {
        private static bool IsLogined { get; set; }
        public static UserBriefModel User { get; set; }

        public App()
        {
            InitializeComponent();
            IsLogined = Preferences.Get("isLogined", false);
            if (!IsLogined)
                MainPage = new Login();
            else
                MainPage = new AppShell();
        }
    }
}
