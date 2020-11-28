using ConvApp.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using ConvApp.Utilites;
using Android.Content;

[assembly: ExportRenderer(typeof(CustomEditor), typeof(CustomEditorRenderer))]
namespace ConvApp.Droid
{
    public class CustomEditorRenderer : EditorRenderer
    {
        public CustomEditorRenderer(Context context) : base(context)
        {
        }
        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                // do whatever you want to the UITextField here!
                Control.SetBackgroundColor(global::Android.Graphics.Color.Transparent);

            }
        }

    }
}