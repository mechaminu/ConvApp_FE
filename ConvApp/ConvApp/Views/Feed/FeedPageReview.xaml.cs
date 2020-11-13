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
        public List<ReviewPost> reviewposts = new List<ReviewPost>();

        public FeedPageReview()
        {
            InitializeComponent();
            
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await FillReviews();
            numfeed.Text = reviewposts.Count().ToString();
           
            ShowReviews();

        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            refresh();
        }

        private async Task FillReviews()
        {
            reviewposts.Clear();

            try
            {
                var list = await ApiManager.GetPostingRange(0, 20, false);
                foreach (var post in list)
                {
                    reviewposts.Add((ReviewPost)post);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("에러",  ex.Message,  "확인");
            }
        }


        private void ShowReviews()
        {
            foreach (var post in reviewposts)
            {
                var elem = new CachedImage()
                {
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    Aspect = Aspect.AspectFill,
                    CacheDuration = TimeSpan.FromDays(1),
                    DownsampleToViewSize = true,
                    BitmapOptimizations = true,
                    RetryCount = 4,
                    RetryDelay = 250,        
                    Source = post.PostImage,
                    BackgroundColor = Color.Red
  
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
