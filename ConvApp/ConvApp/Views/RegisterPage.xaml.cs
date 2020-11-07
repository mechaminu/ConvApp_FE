using Plugin.Media;
using Plugin.Media.Abstractions;
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
    public partial class RegisterPage : ContentPage
    {
        public RegisterPage()
        {
            InitializeComponent();
        }

       private async void PostReview(object sender, EventArgs e)
       {
            await Navigation.PushAsync(new ReviewContent());
       }

       List<ImageSource> RcpImageList = new List<ImageSource>();
       
        private async void PostRecipe(object sender, EventArgs e)
        {
            try
            {
                var pickedImages = await CrossMedia.Current.PickPhotosAsync();

                if (pickedImages.Count == 0)
                    return;

                foreach (var photo in pickedImages)
                {
                    RcpImageList.Add(ImageSource.FromStream(() => photo.GetStream()));
                }

                await Navigation.PushAsync(new RecipeContent(RcpImageList));
                RcpImageList.Clear();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("이미지 추가 과정에서 문제가 발생했습니다", ex);
            }

        }
    }
}