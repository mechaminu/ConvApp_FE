using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Graphics.Drawables.Shapes;
using Android.Views;
using Android.Widget;
using ConvApp.Droid;
using ConvApp.Utilites;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomSearchBar), typeof(CustomSearchBarRenderer))]
namespace ConvApp.Droid
{
    public class CustomSearchBarRenderer : SearchBarRenderer
    {
        public CustomSearchBarRenderer(Context context) : base(context)
        {
        }
        protected override void OnElementChanged(ElementChangedEventArgs<SearchBar> e)
        {
            base.OnElementChanged(e);

            var element = Element as CustomSearchBar;
            //SearchView searchView = (base.Control as SearchView);


            if (element != null)
            {
                var plateid = Resources.GetIdentifier("android:id/search_plate", null, null);
                var plate = Control.FindViewById(plateid);
                plate.SetBackgroundColor(Android.Graphics.Color.Transparent);

                //var searchMagIcon = Resources.GetIdentifier("android:id/search_mag_icon", null, null);
                //var magIcon = Control.FindViewById(searchMagIcon);
                //(magIcon as ImageView).SetImageDrawable(null);
                //(magIcon as ImageView).SetColorFilter(Android.Graphics.Color.Transparent);
                //var searchMagIcon = searchView.Context.Resources.GetIdentifier("android:id/search_mag_icon", null, null);
                //ImageView magIcon = (ImageView)searchView.FindViewById(searchMagIcon);
                //magIcon.SetColorFilter(Android.Graphics.Color.Transparent);

                var gradientDrawable = new GradientDrawable();
                gradientDrawable.SetCornerRadius(20);
                gradientDrawable.SetColor(Android.Graphics.Color.LightGray);
                Control.SetBackground(gradientDrawable);
                


            }
        }

    }






}