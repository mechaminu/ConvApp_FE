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
                var page = (s as ShellContent).Content as ProfilePage;
                page.BindingContext = await UserDetailModel.Populate(await ApiManager.GetUserDetail(App.User.Id));
            };
        }
    }
}