using System;
using Xamarin.Forms;
using ConvApp.Model;
using Plugin.Media;
using System.Collections.Generic;
using Plugin.Media.Abstractions;

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
            
        }

        private void RefreshList() 
        {
            ImageList.ItemsSource = null;
            ImageList.ItemsSource = ImageSrcList;
        } 

        public ImageSource imgSrc = null;

        public static List<ImageSource?> ImageSrcList = new List<ImageSource?>();
        public static List<MediaFile> ImgList = new List<MediaFile>();
       async private void OnImgAdd(object sender, EventArgs e)
        {
            var photo = await CrossMedia.Current.PickPhotoAsync();

            
                if (photo != null)
                {
                    ImgList.Add(photo);

                    ImageSrcList.Add(ImageSource.FromStream(() => photo.GetStream()));
                    RefreshList();

                }
                else
                {
                    ImageSrcList = null;

                }
            
          


        }

        async private void OnNext(object sender, EventArgs e)
        {
            //// Saves gathered data into new 'Post' class instance and adds into the collection.
            //FeedPage.posts.Add(new Post
            //{
            //    UserName="honggildong",
            //    PostTitle="testtitle",
            //    UserImage=null,

            //    PostImage = imgSrc,
            //    PostContent = inputText.Text,
            //    Date = DateTime.Now
            //});

            ////await Navigation.PopAsync();
            //await Navigation.PushAsync(new FeedPage());
            
            await Navigation.PushAsync(new PostContent(ImageSrcList) ) ;
        }
    }
}
