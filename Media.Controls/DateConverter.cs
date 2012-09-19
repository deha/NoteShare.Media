using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Data;

namespace Media.Converters
{
    public class DateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (targetType != typeof(double)) throw new NotImplementedException();
            double secs = ((TimeSpan)value).TotalMilliseconds;
            return secs;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (targetType != typeof(TimeSpan)) throw new NotImplementedException();
            double val = (double)value;
            int miliseconds = (int)(val);
            TimeSpan Span = new TimeSpan(0, 0, 0, 0, miliseconds);
            return Span;
        }
    }
}
