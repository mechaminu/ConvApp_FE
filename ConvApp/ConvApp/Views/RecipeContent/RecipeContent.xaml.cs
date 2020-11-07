using ConvApp.Model;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConvApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RecipeContent : ContentPage
    {
        private List<Node> nodes = new List<Node>();

        public RecipeContent(List<ImageSource> images)
        {
            InitializeComponent();

            foreach (ImageSource image in images)
                nodes.Add(new Node
                {
                    NodeImage = image,
                    NodeString = ""
                });
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            RefreshList();
        }

        private void RefreshList()
        {
            RecipeNode.ItemsSource = null;
            RecipeNode.ItemsSource = nodes;
        }

       async private void cameraplus(object sender, EventArgs e)
        {
            try
            {
                var pickedImages = await CrossMedia.Current.PickPhotosAsync();

                if (pickedImages.Count == 0)
                    return;

                foreach (MediaFile image2 in pickedImages)
                {
                    nodes.Add(new Node()
                    {
                        NodeImage = ImageSource.FromStream(() => image2.GetStream()),
                        NodeString = ""
                    });
                }

                RefreshList();
            }
            catch (Exception ex) 
            {
                throw new InvalidOperationException("이미지 추가 과정에서 문제가 발생했습니다", ex);
            }
       }

       private async void OnSave(object sender, EventArgs e)
       {
            FeedPage.recipePosts.Add(new RecipePost
            {

                User = App.User,
                Date = DateTime.Now,
                Title = recipetitle.Text,
                PostContent = recipecontent.Text,
                RecipeNode = nodes

            }); ;

            await Shell.Current.GoToAsync("//page1");
            await Navigation.PopToRootAsync();

        }


    }
}