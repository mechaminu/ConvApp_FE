using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConvApp.Views
{
    public partial class CategoryPage : ContentPage
    {
        public CategoryPage()
        {
            InitializeComponent();
        }

        public async void StoreHandler(int id)
        {
            var products = await ApiManager.GetProducts(store: id);
            await Navigation.PushAsync(new CategoryRanking { BindingContext = products });
           
        }

        public async void CategoryHandler(int id)
        {
            var products = await ApiManager.GetProducts(category: id);
            await Navigation.PushAsync(new CategoryRanking { BindingContext = products });
        }

        private void OnClick_GS25(object sender, EventArgs e)
        {
            StoreHandler(1);
        }

        private void OnClick_CU(object sender, EventArgs e)
        {
            StoreHandler(2);
        }

        private void OnClick_Ministop(object sender, EventArgs e)
        {
            StoreHandler(3);
        }

        private void OnClick_711(object sender, EventArgs e)
        {
            StoreHandler(4);
        }

        private void OnClick_Emart24(object sender, EventArgs e)
        {
            StoreHandler(5);
        }

        private void OnClick_Misc(object sender, EventArgs e)
        {
            StoreHandler(6);
        }

        private void OnClick_bento(object sender, EventArgs e)
        {
            CategoryHandler(1);
        }

        private void OnClick_noodle(object sender, EventArgs e)
        {
            CategoryHandler(2);
        }

        private void OnClick_gimbab(object sender, EventArgs e)
        {
            CategoryHandler(3);
        }

        private void OnClick_instant(object sender, EventArgs e)
        {
            CategoryHandler(4);
        }

        private void OnClick_snack(object sender, EventArgs e)
        {
            CategoryHandler(5);
        }

        private void OnClick_drink(object sender, EventArgs e)
        {
            CategoryHandler(6);
        }

        private void OnClick_alchol(object sender, EventArgs e)
        {
            CategoryHandler(7);
        }

        private void OnClick_daily(object sender, EventArgs e)
        {
            CategoryHandler(8);
        }

        private void OnClick_etc(object sender, EventArgs e)
        {
            CategoryHandler(9);
        }
    }
}