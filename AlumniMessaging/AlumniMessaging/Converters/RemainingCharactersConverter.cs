using System;
using Xamarin.Forms;

namespace AlumniMessaging.Converters
{
    public class RemainingCharactersConverter : BaseConverter
    {
        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var text = value as string;
            if (!string.IsNullOrEmpty(text))
            {
                var remaining = (int) Application.Current.Resources["MaxMessageLength"] - text.Length;
                if (remaining <= 20)
                    return $"{remaining} characters remaining";
            }

            return string.Empty;
        }
        public override object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}