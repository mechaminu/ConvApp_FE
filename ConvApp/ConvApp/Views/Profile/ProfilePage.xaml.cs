using ConvApp.Models;
using ConvApp.ViewModels;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ConvApp.Views
{
    public partial class ProfilePage : ContentPage
    {
        public ProfilePage()
        {
            InitializeComponent();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (BindingContext is UserDetailViewModel)
                listView.ItemsSource = (BindingContext as UserDetailViewModel).Postings;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            listView.ItemsSource = null;
            listView.ItemsSource = (BindingContext as UserDetailViewModel).Postings;
        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {
            listView.ItemsSource = null;
            listView.ItemsSource = (BindingContext as UserDetailViewModel).LikedPostings;
        }

        private void listView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            (e.SelectedItem as PostingViewModel).ShowPage.Execute(this);
        }

        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            if (await App.Current.MainPage.DisplayAlert("로그아웃","로그아웃하시겠습니까?","예","아니오"))
            {
                App.User = null;
                Preferences.Remove("auth");
                App.Current.MainPage = new LoginPage();
            }
        }
    }
}