using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace ThreatFinder.UI;

public class BoolToCaretBrushConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        bool isEditing = value is true;
        return isEditing ? Brushes.Black : Brushes.Transparent;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}