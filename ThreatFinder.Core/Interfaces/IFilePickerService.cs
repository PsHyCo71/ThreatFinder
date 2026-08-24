using System.Threading.Tasks;

namespace ThreatFinder.Core;

/// <summary>
/// Defines the contract for a service that allows the user to select a file.
/// </summary>
public interface IFilePickerService
{
    /// <summary>
    /// Opens the file picker and returns the path of the selected file.
    /// </summary>
    /// <returns>The selected file path, or null if no file was selected.</returns>
    Task<string?> FilePickerAsync();
}