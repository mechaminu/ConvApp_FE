using ConvApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConvApp.Views.Category
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RankingList : ContentPage
    {
        List<Product> products = new List<Product>();
        public RankingList()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();





        }

        void refresh() 
        {
            ranklist.ItemsSource = null;
            ranklist.ItemsSource = products;

        }


    }
}