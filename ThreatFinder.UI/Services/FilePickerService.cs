using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using ThreatFinder.Core;

namespace ThreatFinder.UI;

/// <summary>
/// Provides file selection functionality using Avalonia's file picker.
/// </summary>
public class FilePickerService : IFilePickerService
{
    /// <summary>
    /// Provides access to the current top-level Avalonia control.
    /// </summary>
    private readonly Func<TopLevel?> _getTopLevel;

    /// <summary>
    /// Initializes a new instance of the <see cref="FilePickerService"/> class.
    /// </summary>
    /// <param name="getTopLevel">Function used to retrieve the current top-level control.</param>
    public FilePickerService(Func<TopLevel?> getTopLevel)
    {
        _getTopLevel = getTopLevel;
    }

    /// <summary>
    /// Opens a file picker and returns the path of the selected file.
    /// </summary>
    /// <returns>
    /// The local path of the selected file, or <c>null</c> if no top-level control is available
    /// or no file was selected.
    /// </returns>
    public async Task<string?> FilePickerAsync()
    {
        // Retrieves the current top-level control required to open the file picker.
        var topLevel = _getTopLevel();
        if (topLevel is null)
            return null;

        // Configures the file picker to allow the selection of a single file.
        FilePickerOpenOptions options = new FilePickerOpenOptions { Title = "Select file to scan.", AllowMultiple = false };
        var files = await topLevel.StorageProvider.OpenFilePickerAsync(options);

        // Returns null if the user did not select any file.
        if (files.Count == 0)
            return null;

        // Returns the local file system path of the selected file.
        return files[0].Path.LocalPath;
    }
}