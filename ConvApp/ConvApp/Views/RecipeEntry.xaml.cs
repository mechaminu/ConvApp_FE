using System;
using System.Drawing;
using System.Collections.Generic;

using Plugin.Media;
using Plugin.Media.Abstractions;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using ConvApp.ViewModels;
using FFImageLoading;
using System.IO;
using ConvApp.Models;

namespace ConvApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RecipeEntry : ContentPage
    {
        private List<PostContentNode> nodes = new List<PostContentNode>();
        private List<byte[]> images = new List<byte[]>();

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
                    var img = photo.GetStream().ToByteArray();

                    images.Add(img);

                    nodes.Add(new PostContentNode {
                        NodeImage = ImageSource.FromStream(() => new MemoryStream(img)),
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
                    nodes.Add(new PostContentNode()
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

            var modelNodes = new List<PostingNodeClient>();

            modelNodes.Add(new PostingNodeClient { text = recipeTitle.Text });
            modelNodes.Add(new PostingNodeClient { text = recipeDescription.Text });

            foreach ( var i in nodes )
            {

            }

            // 이미지 업로드
            await ApiManager.UploadPosting(new Posting
            {
                create_user_oid = App.User.Id,
                is_recipe = true,
                contentNodes = modelNodes
            });

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