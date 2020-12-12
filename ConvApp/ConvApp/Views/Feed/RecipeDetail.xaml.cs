using ConvApp.ViewModels;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ConvApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RecipeDetail : ContentPage
    {
        // TODO Dependency injection으로 FeedbackViewModel 받아올까 함
        public RecipeDetail()
        {
            InitializeComponent();

            BindingContextChanged += (s, e) => ShowNodes();
        }

        private void ShowNodes()
        {
            if (BindingContext == null)
                return;

            var btx = BindingContext as RecipeViewModel;

            nodeArea.Children.Clear();
            MainThread.BeginInvokeOnMainThread(() =>
            {
                foreach (var node in btx.RecipeNode)
                    nodeArea.Children.Add(new RecipeNodeCell { BindingContext = node });
            });
        }
    }
}