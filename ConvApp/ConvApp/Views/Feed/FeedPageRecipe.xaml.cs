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
        public List<RecipePost> postList = new List<RecipePost>();

        public FeedPageRecipe()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await Fill();
            Show();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            Clear();
        }

        private async Task Fill()
        {
            try
            {
                var list = await ApiManager.GetRecipes(0, 20);

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
                var elem = new CachedImage()
                {
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    Aspect = Aspect.AspectFill,
                    CacheDuration = TimeSpan.FromDays(1),
                    DownsampleToViewSize = true,
                    BitmapOptimizations = true,
                    Source = post.RecipeNode[0].NodeImage,
                };

                var tapGestureRecognizer = new TapGestureRecognizer();
                tapGestureRecognizer.Tapped += async (s, e) => {
                    await Navigation.PushAsync(new ReviewDetail
                    {
                        BindingContext = post
                    });
                };
                elem.GestureRecognizers.Add(tapGestureRecognizer);

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
