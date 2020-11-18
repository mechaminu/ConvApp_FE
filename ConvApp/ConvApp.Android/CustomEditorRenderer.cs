using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Views;
using Android.Widget;
using ConvApp.Droid;
using ConvApp.ViewElements;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomEditor), typeof(CustomEditorRenderer))]
namespace ConvApp.Droid
{
  
        class CustomEditorRenderer : EditorRenderer
        {
            public CustomEditorRenderer(Context context) : base(context)
            {
            }

            protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
            {
                base.OnElementChanged(e);

                if (Control != null)
                {
                     Control.Background = new ColorDrawable(Android.Graphics.Color.Transparent);
                    
                }
            }
        }
    }
