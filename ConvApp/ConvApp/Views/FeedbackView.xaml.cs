using ConvApp.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConvApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FeedbackView : StackLayout
    {
        public FeedbackView()
        {
            InitializeComponent();
        }

        private bool likeClicked = false;
        private async void OnLikeClick(object sender, EventArgs e)
        {
            if (likeClicked)
                return;

            likeClicked = true;

            await ((FeedbackViewModel)BindingContext).ToggleLike();

            likeClicked = false;
        }

        private void LikeRefresh()
        {
            if (((FeedbackViewModel)BindingContext).IsLiked)
            {
                likeBtn.Source = ImageSource.FromResource("ConvApp.Resources.heartfilled.png");
            }
            else
            {
                likeBtn.Source = ImageSource.FromResource("ConvApp.Resources.heart.png");
            }
        }
    }
}