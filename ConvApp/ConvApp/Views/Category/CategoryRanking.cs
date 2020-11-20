using ConvApp.Models;
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
    public partial class CategoryRanking : ContentPage
    {
        private List<Product> products = new List<Product>();
        
        public CategoryRanking()
        {
            InitializeComponent();
            
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();


            products = (List<Product>)BindingContext;
            
            

         
        }

        private async void OnClick_CategoryDetail(object sender, SelectedItemChangedEventArgs e)
        {
            var product = products[e.SelectedItemIndex];

            await Navigation.PushAsync(new CategoryDetail { BindingContext = product });
        }
    }
}