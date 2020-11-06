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
            foreach (ImageSource image1 in imgSrc2)
            {
                Image image2 = new Image
                {
                    Source = image1,
                    WidthRequest = 100,
                    HeightRequest = 100,
                    

                };
            flexlayout.Children.Add(image2);
            }

            //pimage.Source = imgSrc2[0];
            rating.Value = 5;
            starrate.Text = rating.Value.ToString();
        }
        
        async private void OnSave(object sender, EventArgs e)
        {
            // Saves gathered data into new 'Post' class instance and adds into the collection.
            FeedPage.reviewPosts.Add(new ReviewPost
            {
                User = App.User,
                PostImage = imgSrc2,
                Rating = rate,
                PostContent = recipeText.Text,
                Date = DateTime.Now
            }); 

          
            //await Navigation.PushAsync(new AppShell());
            
            await Shell.Current.GoToAsync("//page1");
            //이전 페이지 삭제 
            await Navigation.PopAsync();
        }
        double rate = 0;
        private void starvalue(object sender, ValueChangedEventArgs e)
        {
            rate = Math.Round(e.NewValue/0.5)*0.5;

            starrate.Text = rate.ToString();



        }
    }
}