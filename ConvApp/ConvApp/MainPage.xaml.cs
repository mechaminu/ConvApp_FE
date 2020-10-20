using ConvApp.Model;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ConvApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            //refreshCnt();
        }


        //public static List<Post> posts = new List<Post>();
        //static ImageSource imgSrc = null;
        async private void inputImg(object sender, EventArgs e)
        {
            var photo = await CrossMedia.Current.PickPhotoAsync();
            var photoStream = photo.GetStream();
            image.Source = ImageSource.FromStream(() => photoStream);
            //imgSrc = ImageSource.FromStream(() => photoStream);
        }
        private void postSave(object sender, EventArgs e)
        {
            //posts.Add(new Post
            //{
            //    Image = imgSrc,
            //    TextContent = inputText.Text
            //});

            //refreshCnt();
            //inputText.Text = "";
        }
        //void refreshCnt()
        //{
        //    postListStatusLabel.Text = posts.Count.ToString();
        //}
        async private void btn_callphotonext(object sender, EventArgs e)
        {
            //await Navigation.PushAsync(new FeedPage());
        }
    }
}
