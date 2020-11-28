
using ConvApp.ViewModels;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;



namespace ConvApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchPage : ContentPage
    {
        public List<ReviewPostingViewModel> aa = new List<ReviewPostingViewModel>();
            
        public SearchPage()
        {
            InitializeComponent();
            //searchResults.ItemsSource = DataSearch.GetSearchResults(searchBar.Text);
        }

        private void OnClick_search(object sender, TextChangedEventArgs e)
        {
           searchResults.ItemsSource = DataSearch.GetSearchResults(e.NewTextValue);
        }

       
    }
}
