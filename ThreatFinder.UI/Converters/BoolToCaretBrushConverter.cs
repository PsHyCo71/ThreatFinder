using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace ThreatFinder.UI;

/// <summary>
/// Converts a boolean value indicating whether a control is being edited
/// into a brush used to display or hide the text caret.
/// </summary>
public class BoolToCaretBrushConverter : IValueConverter
{
    /// <summary>
    /// Converts the editing state into a caret brush.
    /// </summary>
    /// <param name="value">Value indicating whether the control is currently being edited.</param>
    /// <param name="targetType">Type expected by the target property.</param>
    /// <param name="parameter">Optional converter parameter.</param>
    /// <param name="culture">Culture information used for the conversion.</param>
    /// <returns>
    /// A black brush when editing is active; otherwise, a transparent brush.
    /// </returns>
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        // Determines whether the provided value indicates an active editing state.
        bool isEditing = value is true;

        // Displays the caret while editing and hides it otherwise.
        return isEditing ? Brushes.Black : Brushes.Transparent;
    }

    /// <summary>
    /// Prevents conversion from a caret brush back to a boolean value.
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