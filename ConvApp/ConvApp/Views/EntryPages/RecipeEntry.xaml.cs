using System;
using System.Collections.Generic;

using Plugin.Media;
using Plugin.Media.Abstractions;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using ConvApp.ViewModels;

namespace ConvApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RecipeEntry : ContentPage
    {
        private List<Node> nodes = new List<Node>();

        public RecipeEntry()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                var pickedImages = await CrossMedia.Current.PickPhotosAsync();

                if (pickedImages.Count == 0)
                    return;

                foreach (var photo in pickedImages)
                {
                    nodes.Add(new Node {
                        NodeImage = ImageSource.FromStream(() => photo.GetStream()),
                        NodeString = string.Empty
                    });
                }

                RefreshList();

            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("이미지 추가 과정에서 문제가 발생했습니다", ex);
            }
        }

        private void RefreshList()
        {
            recipeNodeList.ItemsSource = null;
            recipeNodeList.ItemsSource = nodes;
        }

       async private void AddNodesFromImage(object sender, EventArgs e)
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
            // 서버로 Recipe post 보내는 것으로 수정할 것
            FeedPage.recipePosts.Add(new RecipePost
            {
                User = App.User,
                Date = DateTime.Now,
                Title = recipeTitle.Text,
                PostContent = recipeDescription.Text,
                RecipeNode = nodes

            });

            await Navigation.PopToRootAsync();
            await Shell.Current.GoToAsync("//page1");
        }
    }
}