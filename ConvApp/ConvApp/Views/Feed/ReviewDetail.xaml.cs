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
    public partial class ReviewDetail : ContentPage
    {
        public ReviewDetail()
        {
            InitializeComponent();
        }

        private void OnPostComment (object sender, EventArgs e)
        {

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

        }
    }
}