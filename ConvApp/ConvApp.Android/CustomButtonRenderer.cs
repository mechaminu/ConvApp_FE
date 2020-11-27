using Android.Content;
using Android.Views;
using ConvApp.Droid;
using ConvApp.Utilites;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomButton), typeof(CustomButtonRenderer))]
namespace ConvApp.Droid
{
   public class CustomButtonRenderer : ButtonRenderer
    {
        public CustomButtonRenderer(Context context) : base(context)
        {
        }
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Button> e)
        {
            base.OnElementChanged(e);
            Control.Gravity = GravityFlags.Start | GravityFlags.CenterVertical; // Set the vertical text alignment to center
        }
    }
}