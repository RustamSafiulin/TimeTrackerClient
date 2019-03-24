
using System;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Data;

namespace TimeTracker.Converters
{
    public class BytesToImageConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var bytes = value as byte[];

            if (bytes != null)
            {
                try
                {
                    using (var ms = new MemoryStream(bytes))
                    {
                        var bi = new BitmapImage();
                        bi.BeginInit();
                        bi.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                        bi.CacheOption = BitmapCacheOption.OnLoad;
                        bi.StreamSource = ms;
                        bi.EndInit();

                        return bi;
                    }
                }
                catch (Exception)
                {
                    return null;
                }
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