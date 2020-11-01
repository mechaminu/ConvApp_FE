using System;
using Xamarin.Forms;
using ConvApp.ViewModels;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System.Collections.Generic;

namespace ConvApp.Views
{
    public partial class PostEntryPage : ContentPage
    {
        public PostEntryPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            // Placeholder 이미지를 전시하자
            image.Source = ImageSource.FromUri(new Uri("https://via.placeholder.com/150"));
        }

        private List<MediaFile> imgList = new List<MediaFile>();
        private ImageSource imgSrc = null;

        async private void OnImgAdd(object sender, EventArgs e)
        {
            var photo = await CrossMedia.Current.PickPhotoAsync();  // MediaFile 획득

            imgList.Add(photo);
            imgSrc = ImageSource.FromStream(() => photo.GetStream());
            if (imgSrc != null)
                image.Source = imgSrc;
        }

        async private void OnSave(object sender, EventArgs e)
        {
            // Saves gathered data into new 'Post' class instance and adds into the collection.
            var entry = new PostEntry
            {
                Type = PostType.UserRecipe,
                PostTitle = "테스트 제목1234567890",
                PostContent = inputText.Text,
                PostImage = imgList
            };
            try
            {
                FeedPage.posts.Add(await ApiManager.UploadPosting(entry));
                await Navigation.PopAsync();
            }
            catch (InvalidOperationException exp)
            {
                await DisplayAlert("에러", exp.Message, "확인");
            }
            

            //FeedPage.posts.Add(new Post
            //{
            //    PostTitle="testtitle",
            //    PostImage = imgSrc,
            //    PostContent = inputText.Text,
            //    Date = DateTime.Now
            //});
        }
    }
}
