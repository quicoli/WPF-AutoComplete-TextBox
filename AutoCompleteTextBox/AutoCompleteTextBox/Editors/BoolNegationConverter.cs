using System;
using System.Globalization;
using System.Windows.Data;

namespace AutoCompleteTextBox.Editors
{
    internal class BoolNegationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => value is bool boolValue && !boolValue;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => value is bool boolValue && boolValue;
    }
}
