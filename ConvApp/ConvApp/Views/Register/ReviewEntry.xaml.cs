using ConvApp.Models;
using HeyRed.Mime;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConvApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReviewEntry : ContentPage
    {
        private ObservableCollection<ProductModel> productList = new ObservableCollection<ProductModel>();

        public ReviewEntry()
        {
            InitializeComponent();
            prodSelection.ItemsSource = productList;
        }

        protected override void OnAppearing()
        {
            ratingSlider.Value = 5;
            ratingLabel.Text = "5.0";

            base.OnAppearing();
        }

        List<FileResult> resultList = new List<FileResult>();
        Stream imgStream;
        double rate = 5;

        private async void AddImage(object sender, EventArgs e)
        {

            var pickResult = await FilePicker.PickAsync(new PickOptions { PickerTitle = "사진 선택", FileTypes = FilePickerFileType.Images });
            if (pickResult != null)
            {
                pickResult.ContentType = MimeTypesMap.GetMimeType(pickResult.FileName);

                imgStream = await pickResult.OpenReadAsync();
                imageView.Source = ImageSource.FromStream(() => imgStream);

                resultList.Add(pickResult);
            }
            else
                return;
        }

        private async void AddProduct(object sender, EventArgs e)
        {
            var page = new ProductSelectionPage();

            page.MyEvent += (s, e) => GetSingleSelection((s as ProductSelectionPage).selections);

            await Navigation.PushAsync(page);
        }

        private void GetSingleSelection(List<ProductModel> products)
        {
            if (products.Count != 0)
            {
                if (productList.Count == 0)
                    prodSelection.ItemsSource = productList;
                productList.Add(products[0]);
            }
        }

        private void DeleteSelection(object sender, EventArgs e)
        {
            var product = (sender as Button).BindingContext as ProductModel;
            productList.Remove(product);

            if (productList.Count == 0)
                prodSelection.ItemsSource = null;
        }

        private async void OnSave(object sender, EventArgs e)
        {
            try
            {
                var modelNodes = new List<PostingNodeModel>();
                modelNodes.Add(new PostingNodeModel { Text = ratingLabel.Text });
                modelNodes.Add(new PostingNodeModel { Text = reviewContent.Text });

                var imageFilename = await ApiManager.UploadImage(resultList);
                modelNodes.Add(new PostingNodeModel { ImageFilename = imageFilename });

                var prodList = new List<ProductModel>();
                foreach(var prod in productList)
                    prodList.Add(prod);

                await ApiManager.PostPosting(new PostingModel  // 이미지 업로드
                {
                    CreatorId = App.User.Id,
                    PostingType = (byte)PostingTypes.REVIEW,
                    PostingNodes = modelNodes,
                    Products = prodList
                });

                await Navigation.PopToRootAsync();
                await Shell.Current.GoToAsync("//pageReviewFeed");
            }
            catch (Exception ex)
            {
                await DisplayAlert("에러", ex.Message, "확인");
            }
        }

        private void OnSliderChange(object sender, ValueChangedEventArgs e)
        {
            rate = Math.Round(e.NewValue / 0.5) * 0.5;
            ratingLabel.Text = rate.ToString();
        }
    }
}