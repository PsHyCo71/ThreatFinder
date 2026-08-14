using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using ThreatFinder.Core;

namespace ThreatFinder.UI;
public class FilePickerService : IFilePickerService
{
    private readonly Func<TopLevel?> _getTopLevel;

    public FilePickerService(Func<TopLevel?> getTopLevel)
    {
        _getTopLevel = getTopLevel;
    }

    public async Task<string?> FilePickerAsync()
    {
        var topLevel = _getTopLevel();
        if (topLevel is null)
            return null;

        FilePickerOpenOptions options = new FilePickerOpenOptions { Title = "Select file to scan.", AllowMultiple = false};
        var files = await topLevel.StorageProvider.OpenFilePickerAsync(options);

        if (files.Count == 0)
            return null;

        return files[0].Path.LocalPath;
    }
}