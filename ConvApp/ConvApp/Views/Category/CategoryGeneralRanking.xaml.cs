﻿using ConvApp.Models;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConvApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CategoryGeneralRanking : ContentPage
    {
        public CategoryGeneralRanking()
        {
            InitializeComponent();
        }

        private async void OnClick_CategoryDetail(object sender, ItemTappedEventArgs e)
        {
            try
            {
                var product = e.Item as ProductModel;

                await Navigation.PushAsync(new CategoryDetail { BindingContext = await ProductModel.Populate(product) });

                await ApiManager.AddView(1, product.Id);
            }
            catch (Exception ex)
            {
                await DisplayAlert(ex.Message, ex.StackTrace, "ok");
            }
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            (sender as Button).IsEnabled = false;
            list.ItemsSource = null;
            list.ItemsSource = await ApiManager.GetHotProducts();
            (sender as Button).IsEnabled = true;
        }
    }
}