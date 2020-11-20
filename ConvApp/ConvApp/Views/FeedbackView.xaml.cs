using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using ConvApp.ViewModels;
using System.Runtime.CompilerServices;

namespace ConvApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FeedbackView : StackLayout
    {
        public FeedbackView()
        {
            InitializeComponent();

            BindingContextChanged += FeedbackView_BindingContextChanged;
        }

        private void FeedbackView_BindingContextChanged(object sender, EventArgs e)
        {
            likeCnt.Text = ((Feedback)BindingContext).LikeCount + "";
            LikeBtnRefresh();
            ((Feedback)BindingContext).PropertyChanged += FeedbackView_PropertyChanged;
        }

        private void FeedbackView_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            likeCnt.Text = ((Feedback)BindingContext).LikeCount + "";
            LikeBtnRefresh();
            Console.WriteLine(e.PropertyName);
        }

        private bool likeClicked = false;


        private async void OnLikeClick(object sender, EventArgs e)
        {
            if (likeClicked)
                return;

            likeClicked = true;

            await ((Feedback)BindingContext).ToggleLike();

            LikeBtnRefresh();

            likeClicked = false;
        }

        private void LikeBtnRefresh()
        {
            if (((Feedback)BindingContext).IsLiked)
            {
                likeBtn.Source = ImageSource.FromResource("ConvApp.IMAGES.heartfilled.png");
            }
            else
            {
                likeBtn.Source = ImageSource.FromResource("ConvApp.IMAGES.heart.png");
            }
        }
    }
}