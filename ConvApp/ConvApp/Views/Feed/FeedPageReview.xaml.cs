using ConvApp.ViewModels;
using ConvApp.Views.Feed;
using FFImageLoading.Forms;
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
    public partial class FeedPageReview : ContentPage
    {
        public static List<ReviewPost> reviewposts = new List<ReviewPost>();
        public static ReviewPost reviewpost = new ReviewPost();
        public FeedPageReview()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();


            numfeed.Text = reviewposts.Count().ToString();

            FillReviews();

            ShowReviews();

        }

        private async void FillReviews()
        {
            var list = await ApiManager.GetPostingRange(0,10,false);
            reviewposts.Clear();
            foreach (var post in list)
            {
                reviewposts.Add((ReviewPost)post);
            }
        }

        private void ShowReviews()
        {
            refresh();
            foreach (var post in reviewposts)
            {
                var elem = new CachedImage()
                {
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    Aspect = Aspect.AspectFill,
                    CacheDuration = TimeSpan.FromMinutes(5),
                    DownsampleToViewSize = true,
                    RetryCount = 0,
                    RetryDelay = 10,
                    BitmapOptimizations = true,
                    Source = post.PostImage,
                    
                    
                };

                

                var tapGestureRecognizer = new TapGestureRecognizer();
                tapGestureRecognizer.Tapped += async (s, e) => {
                    await Navigation.PushAsync(new ReviewDetail
                    {
                        BindingContext = post
                    });
                };

                elem.GestureRecognizers.Add(tapGestureRecognizer);

                if (reviewposts.IndexOf(post) % 2 == 0)
                {
                    LEFT.Children.Add(elem);
                }
                else
                {
                    RIGHT.Children.Add(elem);
                }
            }
        }
        void refresh()
        {
            LEFT.Children.Clear();
            RIGHT.Children.Clear();
        }
    }




}
