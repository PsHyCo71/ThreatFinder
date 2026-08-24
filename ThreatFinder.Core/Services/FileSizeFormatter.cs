namespace ThreatFinder.Core;

/// <summary>
/// Provides functionality for formatting file sizes into human-readable units.
/// </summary>
public class FileSizeFormatter
{
    /// <summary>
    /// Defines the units used to represent file sizes.
    /// </summary>
    private static readonly string[] Units = {"B","KB", "MB", "GB", "TB"};

    /// <summary>
    /// Converts a file size from bytes to a human-readable string.
    /// </summary>
    /// <param name="bytes">File size in bytes.</param>
    /// <returns>The formatted file size using the most appropriate unit.</returns>
    public static string Format(long bytes)
    {
        if (bytes < 1024)
            return $"{bytes} B";

        double size = bytes;
        int unitIndex = 0;

        // Converts the size to the next unit until it is below 1024
        // or the largest available unit is reached.
        while (size >= 1024 && unitIndex < Units.Length - 1)
        {
            size /= 1024;
            unitIndex++;
        }

        return $"{size:F2} {Units[unitIndex]}";
    }
}