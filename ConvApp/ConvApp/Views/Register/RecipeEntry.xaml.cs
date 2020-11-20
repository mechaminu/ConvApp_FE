using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using ConvApp.Models;
using ConvApp.ViewModels;
using FFImageLoading;
using Xamarin.Essentials;
using System.Linq;
using Xamarin.Forms.Internals;
using HeyRed.Mime;

namespace ConvApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RecipeEntry : ContentPage
    {
        private List<EntryNode> nodes = new List<EntryNode>();
        private List<FileResult> images = new List<FileResult>();
        private bool isSelecting = false;

        public RecipeEntry()
        {
            InitializeComponent();
            RefreshList();
        }

        private void RefreshList()
        {
            recipeNodeList.ItemsSource = null;
            recipeNodeList.ItemsSource = nodes;
        }

        private async Task AddImage()
        {
            try
            {
                var pickResults = await FilePicker.PickMultipleAsync(new PickOptions { PickerTitle = "사진 선택", FileTypes = FilePickerFileType.Images });

                if (pickResults.Count() == 0)
                    throw new InvalidOperationException("이미지를 선택하지 않았습니다");

                foreach (var photo in pickResults)
                {
                    photo.ContentType = MimeTypesMap.GetMimeType(photo.FileName);
                    images.Add(photo);

                    var bytes = (await photo.OpenReadAsync()).ToByteArray();

                    nodes.Add(new EntryNode
                    {
                        image = ImageSource.FromStream(() => new MemoryStream(bytes)),
                        text = string.Empty
                    });
                }
                RefreshList();
            }
            catch (Exception ex)
            {
                await DisplayAlert("에러", ex.Message, "확인");
            }
        }

        async private void AddNodesFromImage(object sender, EventArgs e)
        {
            if (!isSelecting)
            {
                isSelecting = true;
                await AddImage();
                isSelecting = false;
            }
        }

        private async void OnSave(object sender, EventArgs e)
        {
            try
            {
                var modelNodes = new List<PostingNode>();

                modelNodes.Add(new PostingNode { Text = recipeTitle.Text });
                modelNodes.Add(new PostingNode { Text = recipeDescription.Text });

                var strArr = (await ApiManager.UploadImage(images)).Split(';');
                foreach (var i in strArr)
                {
                    modelNodes.Add(new PostingNode { Image = i, Text = nodes[strArr.IndexOf(i)].text });
                }

                // 이미지 업로드
                await ApiManager.PostPosting(new PostingDTO
                {
                    CreatorId = App.User.Id,
                    PostingType = (byte)PostingTypes.RECIPE,
                    PostingNodes = modelNodes
                });

                await Navigation.PopToRootAsync();
                await Shell.Current.GoToAsync("//pageRecipeFeed");
            }
            catch (Exception ex)
            {
                await DisplayAlert("에러", ex.Message, "확인");
            }
        }
    }

    public class EntryNode
    {
        public ImageSource image { get; set; }
        public string text { get; set; }
    }

}