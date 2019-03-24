
using System;
using System.Windows.Data;

using TimeTracker.Models;
using TimeTracker.Images;

namespace TimeTracker.Converters
{
    public class ActivityStateToImageConverter : IValueConverter
    {
        #region IValueConverter Members

        private ImageResourceConverter _imageResourceConverter = new ImageResourceConverter();

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var state = (ActivityState)Enum.Parse(typeof(ActivityState), value.ToString());
            
            switch (state)
            {
                case ActivityState.Paused:
                    return _imageResourceConverter.Convert(Images.Images.play_32, null, null, System.Globalization.CultureInfo.CurrentCulture);
                case ActivityState.Running:
                    return _imageResourceConverter.Convert(Images.Images.pause_32, null, null, System.Globalization.CultureInfo.CurrentCulture);
                default:
                    return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
