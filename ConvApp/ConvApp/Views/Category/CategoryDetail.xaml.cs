using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConvApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CategoryDetail : ContentPage
    {
        int btn_count = 0;
        public CategoryDetail()
        {
            InitializeComponent();
        }
        private void OnClick_love(object sender, EventArgs e)
        {
            if (btn_count == 0)
            {
                btn_love.Source = ImageSource.FromResource("ConvApp.Resources.heartfilled.png");
                btn_count++;
            }
            else
            {
                btn_love.Source = ImageSource.FromResource("ConvApp.Resources.heart.png");
                btn_count--;
            }
        }
    }
}