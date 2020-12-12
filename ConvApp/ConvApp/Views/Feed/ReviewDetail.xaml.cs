using ConvApp.ViewModels;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConvApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReviewDetail : ContentPage
    {
        // TODO Dependency injection으로 FeedbackViewModel 받아올까 함
        public ReviewDetail()
        {
            InitializeComponent();
        }
    }
}