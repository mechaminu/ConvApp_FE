using System;
using Xamarin.Forms;
using ConvApp.Model;
using Plugin.Media; 


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

        public ImageSource imgSrc = null;

        async private void OnImgAdd(object sender, EventArgs e)
        {
            var photo = await CrossMedia.Current.PickPhotoAsync();
            imgSrc = ImageSource.FromStream(() => photo.GetStream());
            if (imgSrc != null)
                image.Source = imgSrc;
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
            
            await Navigation.PushAsync(new PostContent(imgSrc)) ;
        }
    }
}
