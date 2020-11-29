using ConvApp.Models;
using ConvApp.ViewModels;
using System;
using System.Collections.Generic;

using Xamarin.Forms;

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
            var list = new List<SearchResultViewModel>();
            if (result.Postings != null && result.Products != null)
                list = SearchResultModel.Populate(result);
            else
                list.Add(new SearchResultViewModel { Description = "검색 결과가 없습니다" });

            searchResults.ItemsSource = list;
            activityInd.IsVisible = false;
        }
    }
}