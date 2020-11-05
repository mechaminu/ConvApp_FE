using ConvApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConvApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FeedViewCell : ViewCell
    {
        public FeedViewCell()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            var curUser = ((ReviewPost)BindingContext).User;

            username.Text = curUser.Name;
            userimage.Source = curUser.ProfileImage;
        }
    }
}