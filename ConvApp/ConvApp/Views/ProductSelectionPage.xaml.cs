using ConvApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConvApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductSelectionPage : ContentPage
    {
        ObservableCollection<ProductModel> products = new ObservableCollection<ProductModel>();
        public List<ProductModel> selections = new List<ProductModel>();

        public event EventHandler MyEvent;

        public ProductSelectionPage()
        {
            InitializeComponent();
            list.ItemsSource = products;
        }

        protected override void OnAppearing()
        {
            PopulateList();
            base.OnAppearing();
        }

        private async void PopulateList()
        {
            try
            {
                var prodList = await ApiManager.GetProducts();

                products.Clear();
                prodList.ForEach(e => products.Add(e));
            }
            catch (Exception ex)
            {
                await DisplayAlert(ex.Message, ex.StackTrace, "확인");
            }
        }

        private void list_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var product = products[e.ItemIndex];
            selections.Add(product);
            selectionList.Children.Add(new ProductSelectionItem(product));
        }

        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            MyEvent.Invoke(this, new EventArgs());
            await Navigation.PopAsync();
        }
    }

    public class ProductSelectionItem : Frame
    {
        public ProductSelectionItem(ProductModel product)
        {
            Margin = new Thickness(5, 5, 0, 5);
            Padding = 0;
            CornerRadius = 10;
            WidthRequest = 50;

            Content = new Image { Source = product.Image, Aspect = Aspect.AspectFill };
        }
    }
}