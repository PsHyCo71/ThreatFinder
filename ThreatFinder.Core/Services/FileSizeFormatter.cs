namespace ThreatFinder.Core;
public class FileSizeFormatter
{
    private static readonly string[] Units = {"B","KB", "MB", "GB", "TB"};
    public static string Format(long bytes)
    {
        if (bytes <= 1024)
            return $"{bytes} B";

        double size = bytes;
        int unitIndex = 0;

        while (size >= 1024 && unitIndex < Units.Length - 1)
        {
            size /= 1024;
            unitIndex++;
        }

        return $"{size:F2} {Units[unitIndex]}";
    }
}