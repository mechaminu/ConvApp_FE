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
using static FFImageLoading.Forms.CachedImageEvents;

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
            populated = false;

            await Clear();
            await GetData();
            await Show();

            populated = true;
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
                var imgUrl = (post is ReviewPost ? (post as ReviewPost).PostImage : (post as RecipePost).RecipeNode[0].NodeImage).Split(';')[0];

                var layout = new StackLayout();
                var elem = new Frame()
                {
                    CornerRadius = 5,
                    Content = layout
                };

                elem.BackgroundColor = Color.Green;
                elem.Padding = 0;
                layout.BackgroundColor = Color.Blue;
                layout.VerticalOptions = LayoutOptions.Center;
                LEFT.Children.Add(elem);

                var tcs1 = new TaskCompletionSource<SuccessEventArgs>();
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                Task.Factory.StartNew(() =>
                {
                    Console.WriteLine("hello");
                    layout.Children.Add(new CachedImage()
                    {
                        WidthRequest = elem.Width,
                        Aspect = Aspect.AspectFill,
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Center,
                        CacheDuration = TimeSpan.FromDays(1),
                        DownsampleToViewSize = true,
                        BitmapOptimizations = true,
                        SuccessCommand = new Command<SuccessEventArgs>((SuccessEventArgs e) =>
                        {
                            Console.WriteLine("world");
                            tcs1.TrySetResult(e);
                        }),
                        Source = imgUrl,
                        BackgroundColor = Color.Red
                    });
                });
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

                var myVar1 = await tcs1.Task;
                elem.HeightRequest = myVar1.ImageInformation.CurrentHeight;
                layout.HeightRequest = myVar1.ImageInformation.CurrentHeight;
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