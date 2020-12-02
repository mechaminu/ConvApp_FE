using ConvApp.Models;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConvApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RelatedProductView : StackLayout
    {
        public RelatedProductView()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            var btn = sender as Button;
            var ctx = btn.BindingContext as ProductModel;
            await Navigation.PushAsync(new CategoryDetail { BindingContext = await ProductModel.Populate(ctx) });
        }
    }
}