using ConvApp.ViewModels;
using FFImageLoading.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConvApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FeedPageAll : ContentPage
    {
        public List<PostingViewModel> postList = new List<PostingViewModel>();
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

            Clear();
            await GetData();
            Show();

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

        private void Show()
        {
            foreach (var post in postList)
            {
                var imgUrl = (post is ReviewPostingViewModel ? (post as ReviewPostingViewModel).PostImage : (post as RecipePostingViewModel).RecipeNode[0].NodeImage).Split(';')[0];

                var layout = new StackLayout();
                var elem = new Frame()
                {
                    CornerRadius = 10,
                    Content = layout
                };

                elem.BackgroundColor = Color.Green;
                elem.Padding = 0;
                elem.Margin = new Thickness { Top = 0, Bottom = 5, Left = 0, Right = 0 };
                layout.BackgroundColor = Color.Blue;
                (LEFT.Children.Count == 0 ||
                (RIGHT.Children.Count != 0 &&
                    (LEFT.Children.Last().Y + LEFT.Children.Last().Height) < (RIGHT.Children.Last().Y + RIGHT.Children.Last().Height))
                ? LEFT : RIGHT)
                    .Children.Add(elem);

                layout.Children.Add(new CachedImage()
                {
                    WidthRequest = elem.Width,
                    Aspect = Aspect.AspectFill,
                    CacheDuration = TimeSpan.FromDays(1),
                    DownsampleToViewSize = true,
                    BitmapOptimizations = true,
                    Source = imgUrl
                });

                var tap = new TapGestureRecognizer();

                tap.Tapped += async (s, e) =>
                {
                    Page targetPage;

                    if (post is ReviewPostingViewModel)
                    {
                        var feedback = new FeedbackViewModel(0, post.Id);
                        targetPage = new ReviewDetail { BindingContext = post };
                    }
                    else
                        targetPage = new RecipeDetail { BindingContext = post };

                    await Navigation.PushAsync(targetPage);
                };

                elem.GestureRecognizers.Add(tap);
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