
using System;
using System.Windows.Data;

namespace TimeTracker.Converters
{
    public class ToFriendlyDateTimeConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is DateTime)
            {
                var result = (DateTime)value;

                return result.ToString("F", new System.Globalization.CultureInfo("ru-RU"));
            }

            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
