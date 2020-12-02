using ConvApp.ViewModels;
using FFImageLoading.Args;
using FFImageLoading.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace ConvApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchDetail : ContentPage
    {
        public List<PostingViewModel> flexList = new List<PostingViewModel>();
        private bool populated = false;
        private DateTime basetime = DateTime.UtcNow;
        private int page = 0;
        public SearchDetail()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            if (!populated)
            {
                await Refresh();
            }
            base.OnAppearing();

        }

        private async void OnPaging(object sender, EventArgs e)
        {
            //time: basetime, page: page++
            try
            {
                var list = await ApiManager.GetPostings(time: basetime, page: page++);

                foreach (var post in list)
                {
                    await AddElem(post);
                    flexList.Add(post);
                }
            }
            catch
            {
                await Refresh();
            }
        }


        private async Task Refresh()
        {
            populated = false;

            basetime = DateTime.UtcNow;
            page = 0;

            flexLayout.Children.Clear();


            refreshFlex.IsRefreshing = true;    // 이게 RefreshView Refreshing event를 invoke하는 문제가 있음. 해당 eventhandler delegate에서 관련 처리해야함.
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
                refreshFlex.IsRefreshing = false;
            }

            populated = true;
        }

        private async Task GetData()
        {
            //time: basetime
            try
            {
                var list = await ApiManager.GetPostings(time: basetime, page: page++);
                flexList.Clear();

                foreach (var post in list)
                {
                    flexList.Add(post);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("에러", ex.Message, "확인");
            }
        }

        private async Task Show()
        {
            foreach (var post in flexList)
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
                WidthRequest = 130,
                HeightRequest = 130,
                CornerRadius = 0,
                Content = layout
            };

            elem.Padding = 0;
            elem.Margin = new Thickness { Top = 0, Bottom = 5, Left = 0, Right = 0 };
            flexLayout.Children.Add(elem);

            var tcs1 = new TaskCompletionSource<SuccessEventArgs>();
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            Task.Factory.StartNew(() =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    var img = new CachedImage()
                    {
                        WidthRequest = elem.Width,
                        HeightRequest = elem.Height,
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

                ApiManager.AddView(0, posting.Id);

                await Navigation.PushAsync(targetPage);
            };
            elem.GestureRecognizers.Add(tap);
        }

        private async void RefreshFlex_Refreshing(object sender, EventArgs e)
        {
            if (populated)
            {
                await Refresh();
                (sender as RefreshView).IsRefreshing = false;
            }
        }

        private async void OnFocused_SearchBar(object sender, FocusEventArgs e)
        {
            await Navigation.PushAsync(new SearchPage());
        }
    }
}