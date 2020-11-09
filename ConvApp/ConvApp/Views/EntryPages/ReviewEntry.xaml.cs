using System;
using System.Collections.Generic;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ConvApp.ViewModels;

namespace ConvApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReviewEntry : ContentPage
    {
        public ReviewEntry()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // 별점 초기값
            ratingSlider.Value = 5;
            ratingLabel.Text = string.Empty;
        }

        List<ImageSource> ImgSrcList = new List<ImageSource>();
        double rate = 0;

        async private void AddImage(object sender, EventArgs e)
        {
            RefreshImage();
            var photos = await CrossMedia.Current.PickPhotosAsync();

            foreach (MediaFile photo in photos)
            {
                var img = ImageSource.FromStream(() => photo.GetStream());

                ImgSrcList.Add(img);
                flexlayout.Children.Add(new Image
                {
                    Source = img,
                    WidthRequest = 100,
                    HeightRequest = 100,
                });
            }
        }

        private void RefreshImage() 
        {
            ImgSrcList.Clear();
            flexlayout.Children.Clear();
        }

        async private void OnSave(object sender, EventArgs e)
        {
            // Saves gathered data into new 'Post' class instance and adds into the collection.
            FeedPage.reviewPosts.Add(new ReviewPost
            {
                User = App.User,
                Date = DateTime.Now,
                PostImage = ImgSrcList,
                Rating = rate,
                PostContent = reviewContent.Text,
            });

            await Navigation.PopToRootAsync();
            await Shell.Current.GoToAsync("//page1");
        }

        private void OnSliderChange(object sender, ValueChangedEventArgs e)
        {
            rate = Math.Round(e.NewValue / 0.5) * 0.5;
            ratingLabel.Text = rate.ToString();
        }
    }
}