using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace ThreatFinder.UI;

/// <summary>
/// Converts a boolean error state into a brush used to indicate whether an error is present.
/// </summary>
public class BoolToErrorColorConverter : IValueConverter
{
    /// <summary>
    /// Converts the error state into a corresponding brush.
    /// </summary>
    /// <param name="value">Boolean value indicating whether an error is present.</param>
    /// <param name="targetType">Type expected by the target property.</param>
    /// <param name="parameter">Optional converter parameter.</param>
    /// <param name="culture">Culture information used for the conversion.</param>
    /// <returns>A red brush when an error is present; otherwise, a black brush.</returns>
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool isError)
        {
            return isError ? Brushes.Red : Brushes.Black;
        }

        return Brushes.Black;
    }

    /// <summary>
    /// Prevents conversion from a brush back to a boolean error state.
    /// </summary>
    /// <param name="value">Value produced by the target property.</param>
    /// <param name="targetType">Type expected by the source property.</param>
    /// <param name="parameter">Optional converter parameter.</param>
    /// <param name="culture">Culture information used for the conversion.</param>
    /// <returns>This method does not return a value.</returns>
    /// <exception cref="NotSupportedException">
    /// Always thrown because reverse conversion is not supported.
    /// </exception>
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}