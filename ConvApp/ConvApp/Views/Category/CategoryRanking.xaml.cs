using ConvApp.Models;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConvApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CategoryRanking : ContentPage
    {
        public CategoryRanking()
        {
            InitializeComponent();
        }

        private async void OnClick_CategoryDetail(object sender, ItemTappedEventArgs e)
        {
            try
            {
                var product = e.Item as ProductModel;
                var productInfo = await ApiManager.GetProductDetailViewModel(product.Id);

                await Navigation.PushAsync(new CategoryDetail { BindingContext = productInfo });
            }
            catch (Exception ex)
            {
                await DisplayAlert(ex.Message, ex.StackTrace, "ok");
            }
        }
    }
}