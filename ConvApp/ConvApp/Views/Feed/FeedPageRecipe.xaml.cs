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
        // FeedPageReview에 피드페이지 관련 Comment 작성되어있음

        public List<PostingViewModel> postList = new List<PostingViewModel>();
        public bool populated = false;

        private DateTime basetime = DateTime.UtcNow;
        private int page = 0;

        public FeedPageRecipe()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            if (!populated)
            {
                await Refresh();
            }
        }

        private async void OnPaging(object sender, EventArgs e)
        {
            try
            {
                var list = await ApiManager.GetPostings(time: basetime, page: page++, type: (byte)PostingTypes.RECIPE);

                foreach (var post in list)
                {
                    await AddElem(post);
                    postList.Add(post);
                }
            }
            catch
            {
                await Refresh();
            }
        }

        private async void RefreshView_Refreshing(object sender, EventArgs e)
        {
            if (populated)
            {
                await Refresh();
                (sender as RefreshView).IsRefreshing = false;
            }
        }

        private async Task Refresh()
        {
            populated = false;

            basetime = DateTime.UtcNow;
            page = 0;

            LEFT.Children.Clear();
            RIGHT.Children.Clear();

            refreshView.IsRefreshing = true;    // 이게 RefreshView Refreshing event를 invoke하는 문제가 있음. 해당 eventhandler delegate에서 관련 처리해야함.
            try
            {
                await GetData();
                await Show();
            }
            catch
            {
                await DisplayAlert("오류", "요소 전시 실패", "확인");
            }
            finally
            {
                refreshView.IsRefreshing = false;
            }

            populated = true;
        }

        private async Task GetData()
        {
            try
            {
                var list = await ApiManager.GetPostings(time: basetime, page: page++, type: (byte?)PostingTypes.RECIPE);
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
                await AddElem(post);
            }
        }

        public async Task AddElem(PostingViewModel posting)
        {
            var imgUrl = (posting is ReviewViewModel ? (posting as ReviewViewModel).PostImage : (posting as RecipeViewModel).RecipeNode[0].NodeImage).Split(';')[0];

            var layout = new StackLayout();
            var elem = new Frame()
            {
                CornerRadius = 10,
                Content = layout
            };

            elem.Padding = 0;
            elem.Margin = new Thickness { Top = 0, Bottom = 5, Left = 0, Right = 0 };
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

            var tap = new TapGestureRecognizer();

            tap.Tapped += async (s, e) =>
            {
                var feedback = new FeedbackViewModel(0, posting.Id);
                try
                {
                    await feedback.Refresh();
                }
                catch
                {
                    await DisplayAlert("오류", "게시글 피드백 불러오기 실패", "확인");
                    return;
                }

                posting.Feedback = feedback;
                Page targetPage;

                if (posting is ReviewViewModel)
                    targetPage = new ReviewDetail { BindingContext = posting };
                else
                    targetPage = new RecipeDetail { BindingContext = posting };
                await Navigation.PushAsync(targetPage);
            };
            elem.GestureRecognizers.Add(tap);
        }
    }
}
