using ConvApp.Models;
using FFImageLoading;
using HeyRed.Mime;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace ConvApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RecipeEntry : ContentPage
    {
        private readonly List<EntryNode> nodes = new List<EntryNode>();
        private readonly List<FileResult> images = new List<FileResult>();
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

                if (!pickResults.Any())
                    throw new InvalidOperationException("이미지를 선택하지 않았습니다");

                foreach (var photo in pickResults)
                {
                    photo.ContentType = MimeTypesMap.GetMimeType(photo.FileName);
                    images.Add(photo);

                    var bytes = (await photo.OpenReadAsync()).ToByteArray();

                    nodes.Add(new EntryNode
                    {
                        Image = ImageSource.FromStream(() => new MemoryStream(bytes)),
                        Text = string.Empty
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

        private readonly List<ProductModel> productList = new List<ProductModel>();
        private async void AddProduct(object sender, EventArgs e)
        {
            var page = new ProductSelectionPage(productList);
            page.MyEvent += (s, e) =>
            {
                GetSelection((s as ProductSelectionPage).selections);
                (sender as Button).Text = $"상품추가하기 (현재 {productList.Count}개 선택중)";
            };

            await Navigation.PushAsync(page);
        }

        private void GetSelection(List<ProductModel> products)
        {
            productList.Clear();
            if (products.Count != 0)
            {
                foreach (var prod in products)
                    productList.Add(prod);
            }
        }

        private async void OnSave(object sender, EventArgs e)
        {
            try
            {
                var modelNodes = new List<PostingNodeModel>
                {
                    new PostingNodeModel { Text = recipeTitle.Text },
                    new PostingNodeModel { Text = recipeDescription.Text }
                };

                var strArr = (await ApiManager.UploadImage(images)).Split(';');
                foreach (var i in strArr)
                {
                    modelNodes.Add(new PostingNodeModel { ImageFilename = i, Text = nodes[strArr.IndexOf(i)].Text });
                }

                // 이미지 업로드
                await ApiManager.PostPosting(new PostingModel
                {
                    UserId = App.User.Id,
                    PostingType = (byte)PostingTypes.RECIPE,
                    PostingNodes = modelNodes,
                    Products = productList
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
        public ImageSource Image { get; set; }
        public string Text { get; set; }
    }

}