using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace ConvApp.Views
{
    class ImgSrcConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var tmpList = new List<ImageSource>();
            (value as List<byte[]>).ForEach(e => tmpList.Add(ImageSource.FromStream(()=>new MemoryStream(e))));
            return tmpList;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
