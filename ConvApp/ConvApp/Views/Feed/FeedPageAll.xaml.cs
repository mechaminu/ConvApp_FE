using ConvApp.Models;
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
        public List<Post> postList = new List<Post>();

        public FeedPageAll()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await GetData();
            Show();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            Clear();
        }

        private async Task GetData()
        {
            try
            {
                var list = await ApiManager.GetPostings(0, 100);

                postList.Clear();

                foreach (var post in list)
                {
                    postList.Add(post);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("에러", ex.Message, "확인");
            }
        }


        private void Show()
        {
            foreach (var post in postList)
            {
                var elem = new CachedImage()
                {
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    Aspect = Aspect.AspectFill,
                    CacheDuration = TimeSpan.FromDays(1),
                    DownsampleToViewSize = true,
                    BitmapOptimizations = true,
                    Source = post is ReviewPost ? (post as ReviewPost).PostImage : (post as RecipePost).RecipeNode[0].NodeImage,
                    BackgroundColor = Color.Red
                };

                elem.Margin = 5;

                //var tapGestureRecognizer = new TapGestureRecognizer();
                //tapGestureRecognizer.Tapped += async (s, e) => {
                //    await Navigation.PushAsync(new ReviewDetail
                //    {
                //        BindingContext = post
                //    });
                //};
                //elem.GestureRecognizers.Add(tapGestureRecognizer);


                if (postList.IndexOf(post) % 2 == 0)
                {
                    LEFT.Children.Add(elem);
                }
                else
                {
                    RIGHT.Children.Add(elem);
                }
            }
        }

        void Clear()
        {
            LEFT.Children.Clear();
            RIGHT.Children.Clear();
        }
    }
}