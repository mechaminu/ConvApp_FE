using ConvApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace ConvApp
{
    public partial class FeedPage : ContentPage
    {
        public FeedPage()
        {
            InitializeComponent();
        }

        // OnAppearing 메소드에서 리스트뷰에 리스트를 제공해야 튕기지 않음!
        protected override void OnAppearing()
        {
            base.OnAppearing();
            list.ItemsSource = MainPage.posts;
        }
    }
}