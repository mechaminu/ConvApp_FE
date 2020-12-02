using ConvApp.Models;
using ConvApp.ViewModels;
using Xamarin.Forms;

namespace ConvApp.Views
{
    public partial class ProfilePage : ContentPage
    {
        public ProfilePage()
        {
            InitializeComponent();
        }

        private void Button_Clicked(object sender, System.EventArgs e)
        {
            listView.ItemsSource = null;
            listView.ItemsSource = (BindingContext as UserDetailViewModel).Postings;
        }

        private void Button_Clicked_1(object sender, System.EventArgs e)
        {
            listView.ItemsSource = null;
            listView.ItemsSource = (BindingContext as UserDetailViewModel).LikedPostings;
        }

        private void listView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            (e.SelectedItem as PostingViewModel).ShowPage.Execute(this);
        }
    }
}