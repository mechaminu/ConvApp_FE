using ConvApp.Models;
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
    public partial class FeedPageAll : ContentPage
    {
        public static List<Posting> allposts = new List<Posting>();
        public static ReviewPost reviewPostss = new ReviewPost();

        public FeedPageAll()
        {

            InitializeComponent();

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();


            numfeed.Text = allposts.Count().ToString();

            FillPosts();

            ShowPosts();

        }

        private void FillPosts()
        {
            //allposts.Clear();

            //try
            //{
            //    var list = await ApiManager.GetPostingRange(0, 10, false);
            //    foreach (var post in list)
            //    {
            //        allposts.Add((ReviewPost)post);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    await DisplayAlert("에러", ex.Message, "확인");
            //}

        }

        private void ShowPosts()
        {
            //refresh();
            //foreach (var post in allposts)
            //{
            //    var elem = new CachedImage()
            //    {
            //        HorizontalOptions = LayoutOptions.Center,
            //        VerticalOptions = LayoutOptions.Center,
            //        Aspect = Aspect.AspectFill,
            //        CacheDuration = TimeSpan.FromMinutes(5),
            //        DownsampleToViewSize = true,
            //        RetryCount = 0,
            //        RetryDelay = 250,
            //        BitmapOptimizations = true,
            //        Source = post.PostImage
            //    };

            //    var tapGestureRecognizer = new TapGestureRecognizer();
            //    tapGestureRecognizer.Tapped += async (s, e) =>
            //    {
            //        await Navigation.PushAsync(new ReviewDetail
            //        {
            //            BindingContext = reviewPostss
            //        });
            //    };

            //    elem.GestureRecognizers.Add(tapGestureRecognizer);

            //    if (aa.IndexOf(post) % 2 == 0)
            //    {
            //        LEFT.Children.Add(elem);
            //    }
            //    else
            //    {
            //        RIGHT.Children.Add(elem);
            //    }
            //}
        }
        //void refresh() 
        //{
        //    LEFT.Children.Clear();
        //    RIGHT.Children.Clear();
        //}
    }
}