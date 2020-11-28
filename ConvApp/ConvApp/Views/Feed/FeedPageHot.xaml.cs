using ConvApp.Models;
using ConvApp.ViewModels;
using FFImageLoading.Args;
using FFImageLoading.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConvApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FeedPageHot : ContentPage
    {
        public FeedPageHot()
        {
            InitializeComponent();
        }

        // TODO 피드페이지 뷰모델 List 제거 방안 고안
        // 굳이 뷰 로직 안에 전시요소의 뷰모델을 list 변수로 따로 저장해서 관리해야 하는가?
        // 전시요소 BindingContext에 들어가 있는 내용이기에 불필요한 중복으로 보임.
        public List<PostingViewModel> postList = new List<PostingViewModel>();
        public bool populated = false;

        // TODO 피드페이지 페이지네이션 작동방식 보완
        // 무한스크롤 페이지네이션을 위한 기준 시간과, 페이지 번호 변수
        // 현재는 pagenation을 limit 없이 계속해서 page 번호를 증가시키며 진행하고 있음
        // DB에서 nocontent response를 제공하면 그때를 스크롤 끝에 도달한 것으로 판단, 새로고침을 진행하고 있다. << 이것이 불필요한 백엔드 부하로 작용할 여지가 있다.
        // 데이터베이스가 제공하는 최대 페이지 번호를 따로 관리하는 방법을 모색하여, 스크롤 끝에 도달한 여부를 클라이언트 로직으로 처리하는 방법을 고안할 것
        private DateTime basetime = DateTime.UtcNow;
        private int page = 0;

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            // 일정 시간 지난경우 populated false로 다시 만들어 초기화시켜주기 필요할까? pros 최신 포스팅 제공 cons 보던게 있는경우 새로고침되면서 다시 스크롤해야함
            if (!populated)
            {
                await Refresh();
            }
        }

        // TODO 피드페이지 새로고침 Issue
        // RefreshView의 Refresh eventlistener임.
        // 동시에 여러번의 Refresh 요청이 들어오면 진행중인 refresh의 완료 여부와 상관 없이 계속해서 spinner가 전시되고 추가적인 refresh를 진행할수 없는 오류가 존재함.
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

        private async void OnPaging(object sender, EventArgs e)
        {
            try
            {
                // TODO 피드페이지 Verbosity 보완
                // All, Review, Recipe 등 다양한 피드페이지 종류를 결정하는 부분임
                // 다시 말하면 피드 페이지를 구현하는 다른 여러 페이지가 형태가 다 똑같고 이부분만 다르다는 이야기임
                // 그냥 피드페이지 뷰를 하나만 구현하고 그 속에서 아래 부분을 조건에 따라 다르게 적용하는 구현이 더 깔끔하겠다는 생각이 있음
                var list = await ApiManager.GetPostings(time: basetime, page: page++, type: (byte)PostingTypes.REVIEW);

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

        private async Task GetData()
        {
            try
            {
                var list = await ApiManager.GetHotPostings(time: basetime, page: page++);
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

            // TODO 피드페이지 요소 추가방식 개선 - 좌측열 or 우측열 선택 메커니즘
            // 새 요소를 추가하기 직전 좌측 열과 우측 열의 가장 마지막 요소의 높이를 비교해, 높이가 작은 곳에 새 요소를 추가하여 두 열의 높이를 균등하게 맞주기
            // 이렇게 추가때마다 일일히 평가할 것이 아니라, 각 요소 추가함과 동시에 열 높이 변수를 따로 관리하여 단순 비교하는 것이 빠르지 않을까?
            (LEFT.Children.Count == 0 ||
            (RIGHT.Children.Count != 0 &&
                (LEFT.Children.Last().Y + LEFT.Children.Last().Height) < (RIGHT.Children.Last().Y + RIGHT.Children.Last().Height))
            ? LEFT : RIGHT)
                .Children.Add(elem);


            // TODO 피드페이지 요소 추가방식 개선 - CachedImage Loading을 꼭 기다려야 할까?
            // 요소의 추가 및 화면 전시까지 순간을 하나의 Task로 실행, TaskCompletionSource 기능을 이용해 완료될때까지 await
            // 요소 내 CachedImage가 외부 이미지 URL로부터 다운로드 및 로딩 후 높이가 결정되어 화면에 나타나기까지 기다려주는 부분임.
            // 근데 이것도 정해진 열의 Width에 대해서 원본 이미지의 Dimension을 얻어온 후 Resize될 dimension을 미리 획득할 수 있지 않을까?
            // 그렇게 하면 CachedImage가 다운로드 되기도 전에 미리 Frame 요소들을 생성하고 Placeholder Animation을 실행시켜둘 수 있을 뿐만 아니라,
            // 열의 높이차이를 보완하는 위 요소 추가 메커니즘에 더 정확한 열의 높이를 제공할 수 있을 것으로 판단되므로
            var tcs1 = new TaskCompletionSource<SuccessEventArgs>();

            // Await하지 않은 Task를 실행시켜 요소 추가를 진행시키되, TaskCompletionSource의 완료 조건을 아래 Task 내부에서 요소 로딩 완료 Event 속에서 Invoke시킬 것임
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

            // TODO 피드페이지 요소 추가 - 비동기화 고려
            // TaskCompletionSource의 Task. 위의 Await되지 않은 Task 속에 아래 Task의 완결조건이 내장되어있으며, 비동기적인 이미지 로딩을 await을 이용해 동기적으로 기다림
            // "비동기적인 이미지 로딩을 동기적으로 기다림"은 Performance 차원에서 좋지 않아보이므로 보완점 고안할 것.
            await tcs1.Task;


            // 요소가 탭(클릭)되는 경우 해당 요소의 세부페이지로 진입하게끔하기 위해
            // 유저의 탭하는 행위를 인식하는 GestureRecognizer를 구성하고 탭 행위시 fire되는 event에 적절한 Listener delegate를 물려놓는다
            var tap = new TapGestureRecognizer();
            tap.Tapped += async (s, e) =>
            {
                // 피드에선 Feedback 속성이 비어있는 PostingViewModel들만 보여줌
                // 클릭 시 PostingViewModel 내부 Feedback 속성에 FeedbackViewModel을 받아와 채워준 후 세부페이지 전시함.
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

        private async void Button_Clicked(object sender, EventArgs e)
        {
            (sender as Button).IsEnabled = false;

            await ApiManager.RefreshRank();
            await Refresh();

            (sender as Button).IsEnabled = true;
        }
    }
}
