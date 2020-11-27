using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConvApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchEntry : ContentPage
    {
        public SearchEntry()
        {
            InitializeComponent();
        }
        private async void OnClick_focused(object sender, FocusEventArgs e)
        {
            await Navigation.PushAsync(new SearchDetail());
        }
    }
}