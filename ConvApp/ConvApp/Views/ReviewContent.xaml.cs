using ConvApp.Model;
using Plugin.Media;
using Plugin.Media.Abstractions;
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
    public partial class ReviewContent : ContentPage
    {
        public ReviewContent()
        {
            InitializeComponent();
            rating2.Value = 5;
            starrate2.Text = rating2.Value.ToString();
        }

        List<MediaFile> ImgList = new List<MediaFile>();
        List<ImageSource> ImgSrcList = new List<ImageSource>();
        async private void takephoto(object sender, EventArgs e)
        {
            refresh();
            var photos = await CrossMedia.Current.PickPhotosAsync();
            foreach (MediaFile photo in photos)
            {
                ImgSrcList.Add(ImageSource.FromStream(() => photo.GetStream()));
            }

            foreach (ImageSource image1 in ImgSrcList)
            {
                Image image2 = new Image
                {
                    Source = image1,
                    WidthRequest = 100,
                    HeightRequest = 100,
                };
                flexlayout.Children.Add(image2);

            }
        }

        private void refresh()
        {
            ImgSrcList.Clear();
            flexlayout.Children.Clear();
        }
        async private void OnSave2(object sender, EventArgs e)
        {
            // Saves gathered data into new 'Post' class instance and adds into the collection.
            //FeedPage.reviewPosts.Add(new 
            //FeedPage.reviewPosts.Add(new ReviewPost
            //{
            //    User = App.User,
            //    PostImage = ImgSrcList,
            //    Rating = rate,
            //    PostContent = reviewtext.Text,
            //    Date = DateTime.Now
            //});
            FeedAll.reviewPostss = new ReviewPost
            {
                User = App.User,
                PostImage = ImgSrcList,
                Rating = rate,
                PostContent = reviewtext.Text,
                Date = DateTime.Now
            };
            ImageSource b = ImgSrcList[0];
            FeedAll.aa.Add(b);

            //await Navigation.PushAsync(new AppShell());
            await Shell.Current.GoToAsync("//page1");
            //이전 페이지 삭제 
            await Navigation.PopToRootAsync();

        }

        double rate = 0;
        private void starvalue2(object sender, ValueChangedEventArgs e)
        {
            rate = Math.Round(e.NewValue / 0.5) * 0.5;
            starrate2.Text = rate.ToString();

        }
    }
}