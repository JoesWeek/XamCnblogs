using System;
using System.Globalization;
using Xamarin.Forms;

namespace XamCnblogs.UI.Converters
{
    class DealFlagConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((int)value)
            {
                case 1:
                    return (Color)Application.Current.Resources["Primary"];
                case -1:
                    return Color.Red;
                default:
                    return (Color)Application.Current.Resources["PrimaryText"];
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
