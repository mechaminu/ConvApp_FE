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
    public partial class FeedPageRecipe : ContentPage
    {
        public List<RecipePost> recipeposts = new List<RecipePost>();

        public FeedPageRecipe()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await FillRecipes();
            numfeed.Text = recipeposts.Count().ToString();
            ShowRecipes();

        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            refresh();
        }

        private async Task FillRecipes()
        {
            recipeposts.Clear();

            try
            {
                var list = await ApiManager.GetPostingRange(0, 20, true);
                foreach (var post in list)
                {
                    recipeposts.Add((RecipePost)post);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("에러", ex.Message, "확인");
            }
        }

        private void ShowRecipes()
        {
            foreach (var post in recipeposts)
            {
                var elem = new CachedImage()
                {
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    Aspect = Aspect.AspectFill,
                    CacheDuration = TimeSpan.FromDays(1),
                    DownsampleToViewSize = true,
                    BitmapOptimizations = true,
                    RetryCount = 4,
                    RetryDelay = 250,
                    Source = post.RecipeNode[0].NodeImage,
                    BackgroundColor = Color.Red

                };

                var tapGestureRecognizer = new TapGestureRecognizer();
                tapGestureRecognizer.Tapped += async (s, e) => {
                    await Navigation.PushAsync(new ReviewDetail
                    {
                        BindingContext = post
                    });
                };
                elem.GestureRecognizers.Add(tapGestureRecognizer);


                if (recipeposts.IndexOf(post) % 2 == 0)
                {
                    LEFT.Children.Add(elem);
                }
                else
                {
                    RIGHT.Children.Add(elem);
                }
            }
        }

        void refresh()
        {
            LEFT.Children.Clear();
            RIGHT.Children.Clear();
        }
    }
}
