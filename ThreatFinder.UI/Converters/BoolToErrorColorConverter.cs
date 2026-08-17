using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace ThreatFinder.UI;

public class BoolToErrorColorConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool isError)
        {
            return isError ? Brushes.Red : Brushes.Black;
        }

        return Brushes.Black;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}