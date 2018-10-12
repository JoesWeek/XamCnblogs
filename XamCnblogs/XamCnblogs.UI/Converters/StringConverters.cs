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
                    return (Color)Application.Current.Resources["ItemBarText"];
                case -1:
                    return Color.Red;
                default:
                    return (Color)Application.Current.Resources["TitleText"];
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
