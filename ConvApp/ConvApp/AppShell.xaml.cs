using ConvApp.Models;
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

    class ProfileTab : Tab
    {
        public ProfileTab() : base()
        {
            Appearing += async (s, e) =>
            {
                BindingContext = await UserDetailModel.Populate(await ApiManager.GetUserDetail(App.User.Id));
            };
        }
    }
}