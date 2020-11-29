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
    public partial class SearchPage : ContentPage
    {
        public List<ReviewViewModel> aa = new List<ReviewViewModel>();

        public SearchPage()
        {
            InitializeComponent();
        }

        private async void OnClick_search(object sender, EventArgs e)
        {
            searchResults.ItemsSource = null;
            activityInd.IsVisible = true;

            var result = await ApiManager.GetSearch((sender as SearchBar).Text);
            var list = result.Postings;
            
            searchResults.ItemsSource = list;
            activityInd.IsVisible = false;
        }
    }
}