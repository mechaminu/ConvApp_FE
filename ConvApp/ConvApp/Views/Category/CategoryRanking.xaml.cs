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
        private List<ProductDTO> products = new List<ProductDTO>();
        //private List<ProductInformation> products = new List<ProductInformation>();
        public CategoryRanking()
        {
            InitializeComponent();

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            products = (List<ProductDTO>)BindingContext;
            //products = (List<ProductInformation>)BindingContext;
        }
        
        private async void OnClick_CategoryDetail(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                var product = products[e.SelectedItemIndex];
                var productInfo = await ApiManager.GetProductInformation(product.Id);
                await Navigation.PushAsync(new CategoryDetail { BindingContext = productInfo });
            }
            catch (Exception ex)
            {
                await DisplayAlert(ex.Message, ex.StackTrace, "ok");
            }
        }
    }
}