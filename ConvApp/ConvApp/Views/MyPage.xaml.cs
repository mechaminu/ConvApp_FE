using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConvApp.Views
{
    public partial class MyPage : ContentPage
    {
        public MyPage()
        {
            InitializeComponent();
        }

        public async void OnClicked(object sender, EventArgs e)
        {
            var btn = sender as Button;
            btn.IsEnabled = false;

            var st = await ApiManager.GetImage("1wkjcs9rwdfy.jpeg");

            image.Source = null;
            image.Source = ImageSource.FromStream(()=>st);

            btn.IsEnabled = true;
        }
    }
}