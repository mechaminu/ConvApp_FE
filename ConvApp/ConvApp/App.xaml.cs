using ConvApp.Models;
using Xamarin.Forms;

namespace ConvApp
{

    public partial class App : Application
    {
        public static UserDetailModel User { get; set; }

        public App()
        {
            InitializeComponent();
            MainPage = new Login();
        }
    }
}
