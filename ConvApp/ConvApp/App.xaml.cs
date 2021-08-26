using ConvApp.Models;
using ConvApp.ViewModels;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ConvApp
{

    public partial class App : Application
    {
        public static UserBriefModel User { get; set; }

        public App()
        {
            InitializeComponent();
            Application.Current.UserAppTheme = OSAppTheme.Light;
            Application.Current.MainPage = new LoginPage();
        }
    }
}
