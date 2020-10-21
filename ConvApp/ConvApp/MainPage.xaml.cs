using System;
using System.Collections.Generic;

using Xamarin.Forms;
using ConvApp.Model;

using Plugin.Media;


namespace ConvApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        public List<Post> posts = new List<Post>();
        private ImageSource imgSrc = null;

        async private void OnImgAdd(object sender, EventArgs e)
        {
            var photo = await CrossMedia.Current.PickPhotoAsync();
            imgSrc = ImageSource.FromStream(() => photo.GetStream());
            if (imgSrc != null)
                image.Source = imgSrc;
        }

        private void OnSave(object sender, EventArgs e)
        {
            // Saves gathered data into new 'Post' class instance and adds into the collection.
            posts.Add(new Post
            {
                Image = imgSrc,
                TextContent = inputText.Text,
                Date = DateTime.Now
            });

            // Resets after saving
            inputText.Text = "";
            imgSrc = null;
            image.Source = null;
        }

        async private void OnShowList(object sender, EventArgs e)
        {
            await DisplayAlert("List Status", $"# of post : {posts.Count}", "OK");
            //await Navigation.PushAsync(new FeedPage());
        }
    }
}
