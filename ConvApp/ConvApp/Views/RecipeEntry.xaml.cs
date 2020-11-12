﻿using System;
using System.IO;
using System.Collections.Generic;

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
        private List<PostContentNode> nodes = new List<PostContentNode>();
        private List<FileResult> images = new List<FileResult>();

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

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            try
            {
                try
                {
                    var pickResults = await FilePicker.PickMultipleAsync(new PickOptions { PickerTitle = "사진 선택", FileTypes = FilePickerFileType.Images });

                    if (pickResults.Count() == 0)
                        return;

                    foreach (var photo in pickResults)
                    {
                        photo.ContentType = MimeTypesMap.GetMimeType(photo.FileName);
                        images.Add(photo);

                        var bytes = (await photo.OpenReadAsync()).ToByteArray();
                        nodes.Add(new PostContentNode
                        {
                            NodeImage = ImageSource.FromStream(() => new MemoryStream(bytes)),
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
            catch (Exception ex)
            {
                await DisplayAlert("에러", ex.Message, "확인");
            }
        }

       async private void AddNodesFromImage(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    var pickResults = await FilePicker.PickMultipleAsync(new PickOptions { PickerTitle = "사진 선택", FileTypes = FilePickerFileType.Images });

                    if (pickResults.Count() == 0)
                        return;

                    foreach (var photo in pickResults)
                    {
                        photo.ContentType = MimeTypesMap.GetMimeType(photo.FileName);
                        images.Add(photo);

                        var bytes = (await photo.OpenReadAsync()).ToByteArray();
                        nodes.Add(new PostContentNode
                        {
                            NodeImage = ImageSource.FromStream(() => new MemoryStream(bytes)),
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
            catch (Exception ex)
            {
                await DisplayAlert("에러", ex.Message, "확인");
            }
        }

       private async void OnSave(object sender, EventArgs e)
       {
            try
            {
                var modelNodes = new List<PostingNodeClient>();

                modelNodes.Add(new PostingNodeClient { text = recipeTitle.Text });
                modelNodes.Add(new PostingNodeClient { text = recipeDescription.Text });

                var strArr = (await ApiManager.UploadImage(images)).Split(';');
                foreach (var i in strArr)
                {
                    modelNodes.Add(new PostingNodeClient { image = i, text = nodes[strArr.IndexOf(i)].NodeString });
                }

                // 이미지 업로드
                await ApiManager.UploadPosting(new Posting
                {
                    create_user_oid = App.User.Id,
                    is_recipe = true,
                    PostingNodes = modelNodes
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
            catch (Exception ex)
            {
                await DisplayAlert("에러", ex.Message, "확인");
            }
        }
    }
}