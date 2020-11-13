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
        public List<ReviewPost> postList = new List<ReviewPost>();

        public FeedPageReview()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await GetData();    // await 하지 않으면 미처 로딩이 완료되기 전에 Show 메소드로 넘어감. + 트래픽 절약을 위해 처음 1회만 실시하고, 이후 스크롤이나 유저의 새로고침 명령으로 새로고침하는 편이 좋을 듯.
            Show();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            Clear();    // 끝의 몇 요소들이 전시되지 않던 오류 해결
        }

        private async Task GetData()
        {
            try
            {
                var list = await ApiManager.GetReviews(0, 100);

                postList.Clear();

                foreach (var post in list)
                {
                    postList.Add((ReviewPost)post);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("에러",  ex.Message,  "확인");
            }
        }

        private void Show()
        {
            foreach (var post in postList)
            {
                // 현재로서는 직접 View element를 C#으로만 생성
                // 추후 별개 Xaml 파일로 선언된 element를 여기서 인스턴스화해서 List에 추가해주는 것이 좋을 듯.
                var elem = new CachedImage()
                {
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    Aspect = Aspect.AspectFill,
                    CacheDuration = TimeSpan.FromDays(1),   // 무슨 역할을 하는지 모르겠음
                    DownsampleToViewSize = true,
                    BitmapOptimizations = true,  
                    Source = post.PostImage,
                    BackgroundColor = Color.Red
                };

                elem.Margin = 5;

                var tapGestureRecognizer = new TapGestureRecognizer();
                tapGestureRecognizer.Tapped += async (s, e) => {
                    await Navigation.PushAsync(new ReviewDetail
                    {
                        BindingContext = post
                    });
                };
                elem.GestureRecognizers.Add(tapGestureRecognizer);

                // 강제로 좌,우 교대하여 입력하지 말고, 가장 마지막 요소의 높이가 좀 더 위에 있는 열을 감지해서 그 열에 새 요소를 넣어주는 로직이 바람직할 듯.
                // ㄴ> 요소의 높이를 파악하기가 쉽지 않다...
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
