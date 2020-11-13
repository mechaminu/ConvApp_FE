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
        public static List<RecipePost> recipeposts = new List<RecipePost>();
      
        public FeedPageRecipe()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();


            numfeed.Text = recipeposts.Count().ToString();

            FillRecipes();

            ShowRecipes();

        }

        private async void FillRecipes()
        {
            var list = await ApiManager.GetPostingRange(1, 20, true);

            foreach (var post in list)
            recipeposts.Add((RecipePost)post);

        }

        private void ShowRecipes()
        {
            refresh();
            foreach (var post in recipeposts)
            {
                var elem = new CachedImage()
                {
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    Aspect = Aspect.AspectFill,
                    CacheDuration = TimeSpan.FromMinutes(5),
                    DownsampleToViewSize = true,
                    RetryCount = 0,
                    RetryDelay = 250,
                    BitmapOptimizations = true,
                    Source = post.RecipeNode[0].NodeImage
                };

                var tapGestureRecognizer = new TapGestureRecognizer();
                tapGestureRecognizer.Tapped += async (s, e) => {
                    await Navigation.PushAsync(new RecipeDetail
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