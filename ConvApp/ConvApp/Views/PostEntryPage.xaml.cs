using System;
using Xamarin.Forms;
using ConvApp.ViewModels;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System.Collections.Generic;
using System.IO;

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
            RefreshList();
        }

        private void RefreshList()
        {
            imageList.ItemsSource = null;
            imageList.ItemsSource = imgSrcList;
        }

        private List<MediaFile> imgList = new List<MediaFile>();
        private List<ImageSource> imgSrcList = new List<ImageSource>();

        async private void OnImgAdd(object sender, EventArgs e)
        {
            var photo = await CrossMedia.Current.PickPhotoAsync();  // MediaFile 획득
            if (photo != null)
            {
                imgList.Add(photo);
                imgSrcList.Add(ImageSource.FromStream(()=>photo.GetStream()));
                RefreshList();
            }
        }

        async private void OnSave(object sender, EventArgs e)
        {
            // Saves gathered data into new 'Post' class instance and adds into the collection.
            var entry = new PostEntry
            {
                Type = PostType.UserRecipe,
                PostTitle = inputTitle.Text,
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
        }
    }
}
