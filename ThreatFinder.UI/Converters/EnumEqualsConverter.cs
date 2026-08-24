using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace ThreatFinder.UI;

/// <summary>
/// Converts a value into a boolean indicating whether it is equal to the specified parameter.
/// </summary>
public class EnumEqualsConverter : IValueConverter
{
    /// <summary>
    /// Compares the provided value with the specified parameter.
    /// </summary>
    /// <param name="value">Value to compare.</param>
    /// <param name="targetType">Type expected by the target property.</param>
    /// <param name="parameter">Value to compare against.</param>
    /// <param name="culture">Culture information used for the conversion.</param>
    /// <returns><c>true</c> if the value matches the parameter; otherwise, <c>false</c>.</returns>
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value?.Equals(parameter) ?? false;
    }

    /// <summary>
    /// Prevents conversion from a boolean value back to the original value.
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