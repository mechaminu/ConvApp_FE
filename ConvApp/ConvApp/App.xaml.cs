using ConvApp.Models;
using Xamarin.Forms;

namespace ConvApp
{

    public partial class App : Application
    {
        public static UserModel User { get; private set; }

        public App()
        {
            InitializeComponent();

            GetUser();  // 이후 로그인으로 대체

            MainPage = new AppShell();
        }

        private async void GetUser()
        {
            User = await ApiManager.GetUser(1);
        }
    }
}
