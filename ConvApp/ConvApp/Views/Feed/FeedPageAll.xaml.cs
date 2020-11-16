using ConvApp.Models;
using ConvApp.ViewModels;
using ConvApp.Views;
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
        public bool populated = false;

        public FeedPageAll()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            if (!populated)
            {
                populated = true;
                await Refresh();
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }

        private async Task Refresh()
        {
            await Clear();
            await GetData();
            await Show();
        }

        private async Task GetData()
        {
            try
            {
                var list = await ApiManager.GetPostings();

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


        private async Task Show()
        {
            foreach (var post in postList)
            {
                View elem;

                var imgUrl = (post is ReviewPost ? (post as ReviewPost).PostImage : (post as RecipePost).RecipeNode[0].NodeImage).Split(';')[0];
                if (imgUrl != string.Empty)
                {
                    var imgElem = new CachedImage()
                    {
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Center,
                        Aspect = Aspect.AspectFill,
                        CacheDuration = TimeSpan.FromDays(1),
                        DownsampleToViewSize = true,
                        BitmapOptimizations = true,
                        Source = imgUrl,
                        BackgroundColor = Color.Red
                    };

                    elem = new Frame()
                    {
                        CornerRadius = 5,
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        Content = imgElem
                    };
                }
                else
                {
                    elem = new Frame()
                    {
                        CornerRadius = 5,
                        HeightRequest = 150,
                        Content = new Label { Text = "No image" }
                    };
                }

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

                await Task.Delay(100);
            }
        }

        private async Task Clear()
        {
            LEFT.Children.Clear();
            RIGHT.Children.Clear();
            await Task.Delay(100);
        }

        private async void RefreshView_Refreshing(object sender, EventArgs e)
        {
            var elem = (RefreshView)sender;
            await Refresh();
            elem.IsRefreshing = false;
        }
    }
}