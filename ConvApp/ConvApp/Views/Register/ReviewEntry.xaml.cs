using System;
using System.IO;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using ConvApp.Models;
using HeyRed.Mime;

namespace ConvApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReviewEntry : ContentPage
    {
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

        private async void OnSave(object sender, EventArgs e)
        {
            try
            {
                var modelNodes = new List<PostingNode>();
                modelNodes.Add(new PostingNode { Text = ratingLabel.Text });
                modelNodes.Add(new PostingNode { Text = reviewContent.Text });

                var imageFilename = await ApiManager.UploadImage(resultList);
                modelNodes.Add(new PostingNode { Image = imageFilename });

                await ApiManager.PostPosting(new PostingDTO  // 이미지 업로드
                {
                    CreatorId = App.User.Id,
                    PostingType = (byte)PostingTypes.REVIEW,
                    PostingNodes = modelNodes
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