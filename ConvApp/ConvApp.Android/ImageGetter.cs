using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace ConvApp.Droid
{
    class ImageGetter
    {

        public Task<List<Stream>> GetAllImages()
        {
            String[] proj = { MediaStore.Images.Media.ContentType };

            var cursor = Application.Context.ContentResolver.Query(
                MediaStore.Images.Media.ExternalContentUri,
                proj,
                null,
                null,
                null);

            return 
        }
    }
}