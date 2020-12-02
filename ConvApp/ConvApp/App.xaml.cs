using ConvApp.Models;
using ConvApp.ViewModels;
using Xamarin.Forms;

namespace ConvApp
{

    public partial class App : Application
    {
        public static UserBriefModel User { get; set; }

        public App()
        {
            InitializeComponent();
            MainPage = new Login();
        }
    }
}
