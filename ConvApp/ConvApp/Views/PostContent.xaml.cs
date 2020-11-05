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
            FeedPage.reviewPosts.Add(new ReviewPost
            {
                User = App.User,
                PostImage = imgSrc2,
                PostContent =recipeText.Text,
                Date = DateTime.Now
            })  ;

          
            //await Navigation.PushAsync(new AppShell());
            
            await Shell.Current.GoToAsync("//page1");
            //이전 페이지 삭제 
            await Navigation.PopAsync();
        }

      
    }
}