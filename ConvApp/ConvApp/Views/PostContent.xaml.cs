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
        private List<ImageSource> imgSrc2 = null;

        public PostContent(List<ImageSource> image)
        {
            InitializeComponent();
           
            imgSrc2 = image;
            pimage.Source = imgSrc2[0];
        }
        
        async private void OnSave(object sender, EventArgs e)
        {
            // Saves gathered data into new 'Post' class instance and adds into the collection.
            FeedPage.posts.Add(new Post
            {
                UserName = "honggildong",
                PostTitle = recipeNameText.Text,
                UserImage = imgSrc2[0],

                PostImage = imgSrc2,
                PostContent = $"재료: {recipeItemText.Text}\n가격: {recipePriceText.Text}\n조리방법\n{recipeText.Text}",
                Date = DateTime.Now
            });

          
            //await Navigation.PushAsync(new AppShell());
            
            await Shell.Current.GoToAsync("//page1");
            //이전 페이지 삭제 
            await Navigation.PopAsync();
        }

        async private void SelectStyle(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PostContentDetail());
        }
    }
}