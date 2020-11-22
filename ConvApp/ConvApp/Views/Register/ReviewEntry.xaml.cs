using ConvApp.Models;
using HeyRed.Mime;
using System;
using System.Collections.Generic;
using System.IO;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConvApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReviewEntry : ContentPage
    {
        private ProductDTO product;
        
        public ReviewEntry()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ratingSlider.Value = 5;             // 별점 초기값
            ratingLabel.Text = "5.0";    // 
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

            page.MyEvent += (s, e) => GetSelection((s as ProductSelectionPage).selections);

            await Navigation.PushAsync(page);
        }

        private void GetSelection(List<ProductDTO> products)
        {
            if (products.Count != 0)
                product = products[0];
        }

        private async void OnSave(object sender, EventArgs e)
        {
            try
            {
                var modelNodes = new List<PostingNode>();
                modelNodes.Add(new PostingNode { Text = ratingLabel.Text });
                modelNodes.Add(new PostingNode { Text = reviewContent.Text });

                var imageFilename = await ApiManager.UploadImage(resultList);
                modelNodes.Add(new PostingNode { Image = imageFilename });

                var prodList = new List<ProductDTO>();
                prodList.Add(product);

                await ApiManager.PostPosting(new PostingDTO  // 이미지 업로드
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