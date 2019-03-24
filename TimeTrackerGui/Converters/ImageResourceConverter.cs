
using System;

using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace TimeTracker.Converters
{
    public class ImageResourceConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            MemoryStream ms = new MemoryStream();
            var bitmap = value as System.Drawing.Bitmap;

            if (bitmap != null)
            {
                bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                var image = new BitmapImage();
                image.BeginInit();
                ms.Seek(0, SeekOrigin.Begin);
                image.StreamSource = ms;
                image.EndInit();

                return image;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
