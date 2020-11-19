using ConvApp.Models;
using ConvApp.ViewModels;
using FFImageLoading.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static FFImageLoading.Forms.CachedImageEvents;

namespace ConvApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FeedPageRecipe : ContentPage
    {
        public FeedPageRecipe()
        {
            InitializeComponent();
        }

        public List<Post> postList = new List<Post>();
        public bool populated = false;

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            if (!populated)
            {
                populated = true;
                await Refresh();
            }
        }

        private async Task Refresh()
        {
            populated = false;

            await GetData();
            Clear();
            await Show();

            populated = true;
        }

        private async Task GetData()
        {
            try
            {
                var list = await ApiManager.GetPostings((byte?)PostingTypes.RECIPE);
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

                var layout = new AbsoluteLayout();
                var elem = new Frame()
                {
                    Content = layout,
                    CornerRadius = 10,
                    Padding = 0,
                    Margin = new Thickness { Top = 0, Bottom = 5, Left = 0, Right = 0 }
                };

                // 좌측열 우측열 중 마지막 요소의 높이가 낮은곳에 새 요소를 배치
                (LEFT.Children.Count == 0 || (RIGHT.Children.Count != 0 &&
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

        private void Clear()
        {
            LEFT.Children.Clear();
            RIGHT.Children.Clear();
        }

        private async void RefreshView_Refreshing(object sender, EventArgs e)
        {
            var elem = (RefreshView)sender;
            await Refresh();
            elem.IsRefreshing = false;
        }
    }
}
