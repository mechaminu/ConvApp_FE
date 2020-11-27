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
    public partial class SearchDetail : TabbedPage
    {
        public SearchDetail()
        {
            InitializeComponent();
        }

        private void OnTextChanged_Search(object sender, TextChangedEventArgs e)
        {
            searchResults.ItemsSource = DataSearch.GetSearchResults(searchBar.Text);
        }
    }
}