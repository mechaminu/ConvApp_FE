using ConvApp.Models;
using ConvApp.Views;
using Xamarin.Forms;

namespace ConvApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
        }
    }

    class ProfileShellContent : ShellContent
    {
        public ProfileShellContent()
        {
            Appearing += async (s, e) =>
            {
                if (App.User == null)
                {
                    if (await App.Current.MainPage.DisplayAlert("", "로그인 후 이용가능한 메뉴입니다", "로그인으로 이동", "취소"))
                        await App.Current.MainPage.Navigation.PushModalAsync(new LoginPage());
                    else
                        await Shell.Current.GoToAsync("//pageHotFeed");
                    return;
                }

                var page = (s as ShellContent).Content as ProfilePage;
                page.BindingContext = await UserDetailModel.Populate(await ApiManager.GetUserDetail(App.User.Id));
            };
        }
    }
}