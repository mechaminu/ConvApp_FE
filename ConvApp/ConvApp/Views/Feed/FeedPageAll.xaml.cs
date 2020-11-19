using ConvApp.Models;
using ConvApp.ViewModels;
using ConvApp.Views;
using FFImageLoading.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
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
                    CornerRadius = 10,
                    Content = layout
                };

                elem.BackgroundColor = Color.Green;
                elem.Padding = 0;
                elem.Margin = new Thickness { Top = 0, Bottom = 5, Left = 0, Right = 0};
                layout.BackgroundColor = Color.Blue;
                (LEFT.Children.Count == 0 ||
                (RIGHT.Children.Count != 0 && 
                    (LEFT.Children.Last().Y + LEFT.Children.Last().Height) < (RIGHT.Children.Last().Y + RIGHT.Children.Last().Height))
                ? LEFT : RIGHT)
                    .Children.Add(elem);

                var tcs1 = new TaskCompletionSource<SuccessEventArgs>();
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                Task.Factory.StartNew(() =>
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        var img = new CachedImage()
                        {
                            WidthRequest = elem.Width,
                            Aspect = Aspect.AspectFill,
                            CacheDuration = TimeSpan.FromDays(1),
                            DownsampleToViewSize = true,
                            BitmapOptimizations = true,
                            SuccessCommand = new Command<SuccessEventArgs>((SuccessEventArgs e) =>
                            {
                                tcs1.SetResult(e);
                            }),
                            Source = imgUrl
                        };

                        layout.Children.Add(img);
                    });
                });
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

                await tcs1.Task;
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