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
            //searchResults.ItemsSource = DataSearch.GetSearchResults(searchBar.Text);
        }

        private async void OnClick_search(object sender, TextChangedEventArgs e)
        {
            searchResults.ItemsSource = await ApiManager.GetPostings();
        }
    }
}