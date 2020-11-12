using ConvApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConvApp.Views.Feed
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShowReview : ContentPage
    {
        ReviewPost Review = new ReviewPost();
        public ShowReview()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            username.Text = Review.User.Name;
            userimage2 = null;
            date.Text = Review.Date.ToString();
            rating.Text = Review.Rating.ToString();
            postimage.ItemsSource = Review.PostImage;
            postcontent.Text = Review.PostContent;
          
        }
    }
}