using ConvApp.Models;
using System;
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
                await Navigation.PushAsync(new CategoryDetail { BindingContext = await ProductModel.Populate(product) });
                await ApiManager.AddView(1, product.Id);
            }
            catch (Exception ex)
            {
                await DisplayAlert(ex.Message, ex.StackTrace, "ok");
            }
        }
    }
}