using FFImageLoading.Args;
using FFImageLoading.Forms;
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
    public partial class RecipeNodeCell : ContentView
    {
        public RecipeNodeCell()
        {
            InitializeComponent();
        }

        private void CachedImage_Success(object sender, SuccessEventArgs e)
        {
            var elem = sender as CachedImage;
        }
    }
}