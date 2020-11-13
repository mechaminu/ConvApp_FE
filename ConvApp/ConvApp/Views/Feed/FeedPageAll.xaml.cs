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
    public partial class FeedPageAll : ContentPage
    {
      public static List<ReviewPost> aa = new List<ReviewPost>();
      public static ReviewPost reviewPostss = new ReviewPost ();

        public FeedPageAll()
        {

            InitializeComponent();

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            
            numfeed.Text = aa.Count().ToString();

            FillReviews();

            ShowReviews();

        }

        private void FillReviews()
        {

        }

        private void ShowReviews()
        {
            refresh();
            foreach (var post in aa)
            {
                var elem = new CachedImage()
                {
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    Aspect = Aspect.AspectFill,
                    CacheDuration = TimeSpan.FromMinutes(5),
                    DownsampleToViewSize = true,
                    RetryCount = 0,
                    RetryDelay = 250,
                    BitmapOptimizations = true,
                    Source = post.PostImage
                };

                var tapGestureRecognizer = new TapGestureRecognizer();
                tapGestureRecognizer.Tapped += async (s, e) => {
                    await Navigation.PushAsync(new ReviewDetail
                    {
                        BindingContext = reviewPostss
                    });
                };

                elem.GestureRecognizers.Add(tapGestureRecognizer);

                if (aa.IndexOf(post) % 2 == 0)
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