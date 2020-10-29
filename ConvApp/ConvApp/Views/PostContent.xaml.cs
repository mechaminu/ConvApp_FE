using ConvApp.Model;
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
    public partial class PostContent : ContentPage
    {
        private ImageSource imgSrc2 = null;

        public PostContent(ImageSource image)
        {
            InitializeComponent();
           
            imgSrc2 = image;
            pimage.Source = imgSrc2;
        }
        
        async private void OnSave(object sender, EventArgs e)
        {
            // Saves gathered data into new 'Post' class instance and adds into the collection.
            FeedPage.posts.Add(new Post
            {
                UserName = "honggildong",
                PostTitle = recipeNameText.Text,
                UserImage = imgSrc2,

              PostImage = imgSrc2,
                PostContent = $"재료: {recipeItemText.Text}\n 가격: {recipePriceText.Text}\n 조리방법\n{recipeText.Text}",
                Date = DateTime.Now
            });

            //await Navigation.PopAsync();
            await Navigation.PushAsync(new FeedPage());
        }
    }
}