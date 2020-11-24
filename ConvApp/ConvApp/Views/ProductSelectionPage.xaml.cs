using ConvApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConvApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductSelectionPage : ContentPage
    {
        readonly ObservableCollection<ProductModel> products = new ObservableCollection<ProductModel>();
        public List<ProductModel> selections = new List<ProductModel>();
        private readonly bool single;

        public event EventHandler MyEvent;

        public ProductSelectionPage(List<ProductModel> l = null, bool _single = false)
        {
            InitializeComponent();

            single = _single;
            list.ItemsSource = products;

            if (single || l == null || !l.Any())
                return;

            foreach (var e in l)
                selections.Add(e);

            RefreshSelection();
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

        private void ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var product = products[e.ItemIndex];

            if (!selections.Exists(e => e.Id == product.Id))
            {
                selections.Add(product);
                if (single)
                    OnSave(this, new EventArgs());
            }

            RefreshSelection();
        }

        private void ItemRemove(object sender, EventArgs e)
        {
            var product = (sender as ProductSelectionItem).BindingContext as ProductModel;
            selections.Remove(product);
            RefreshSelection();
        }

        private void RefreshSelection()
        {
            selectionList.Children.Clear();
            foreach(var e in selections)
            {
                var item = new ProductSelectionItem(e);
                item.GestureRecognizer.Tapped += (s, e) => ItemRemove(s, e);
                selectionList.Children.Add(item);
            }
        }

        private async void OnSave(object sender, EventArgs e)
        {
            MyEvent.Invoke(this, new EventArgs());
            await Navigation.PopAsync();
        }
    }

    public class ProductSelectionItem : Frame
    {
        public TapGestureRecognizer GestureRecognizer;

        public ProductSelectionItem(ProductModel product)
        {
            BindingContext = product;

            Margin = new Thickness(5, 5, 0, 5);
            Padding = 0;
            CornerRadius = 10;
            WidthRequest = 50;

            GestureRecognizer = new TapGestureRecognizer { };

            GestureRecognizers.Add(GestureRecognizer);

            Content = new Image { Source = product.Image, Aspect = Aspect.AspectFill };
        }
    }
}