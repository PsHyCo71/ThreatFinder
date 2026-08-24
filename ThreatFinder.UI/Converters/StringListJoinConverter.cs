using Avalonia.Data.Converters;
using System;
using System.Collections;
using System.Globalization;
using System.Linq;

namespace ThreatFinder.UI;

/// <summary>
/// Converts an enumerable collection into a comma-separated string.
/// </summary>
public class StringListJoinConverter : IValueConverter
{
    /// <summary>
    /// Converts an enumerable collection into a string with its elements separated by commas.
    /// </summary>
    /// <param name="value">Value to convert, expected to be an enumerable collection.</param>
    /// <param name="targetType">Type expected by the target property.</param>
    /// <param name="parameter">Optional converter parameter.</param>
    /// <param name="culture">Culture information used for the conversion.</param>
    /// <returns>
    /// A comma-separated string containing the collection elements, or the original value
    /// if it is not an enumerable collection.
    /// </returns>
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        // Converts enumerable values into a comma-separated string.
        if (value is IEnumerable list and not string)
            return string.Join(", ", list.Cast<object>());

        return value;
    }

    /// <summary>
    /// Prevents conversion from a string back to an enumerable collection.
    /// </summary>
    /// <param name="value">Value produced by the target property.</param>
    /// <param name="targetType">Type expected by the source property.</param>
    /// <param name="parameter">Optional converter parameter.</param>
    /// <param name="culture">Culture information used for the conversion.</param>
    /// <returns>This method does not return a value.</returns>
    /// <exception cref="NotSupportedException">
    /// Always thrown because reverse conversion is not supported.
    /// </exception>
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}