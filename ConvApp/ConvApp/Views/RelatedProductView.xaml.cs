using ConvApp.Models;
using ConvApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            var productVM = await ApiManager.GetProductDetailViewModel(ctx.Id);

            await Navigation.PushAsync(new CategoryDetail { BindingContext = productVM });
        }
    }
}