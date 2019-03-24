
using System;
using System.Windows.Data;
using System.Globalization;

namespace TimeTracker.Converters
{
    public class UppercaseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var str = value as String;
            if (str != null)
            {
                return str.ToUpper();
            }

            return String.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}