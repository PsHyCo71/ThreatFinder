using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.IO;
using System.Threading.Tasks;
using ThreatFinder.Core;

namespace ThreatFinder.ViewModels;

public partial class ScanViewModel : ViewModelBase
{
    public enum ScanMode { Url, File }

    private ScanManager _scanManager;
    private IFilePickerService _filePickerService;
    private INavigationService _navigationService;
    public ScanViewModel(ScanManager scanManager, IFilePickerService filePickerService, INavigationService navigationService)
    {
        _scanManager = scanManager;
        _filePickerService = filePickerService;
        _navigationService = navigationService;
    }

    [RelayCommand]
    public void GoToSettings()
    {
        _navigationService.NavigateToSettings();
    }

    [ObservableProperty]
    public partial ScanMode SelectedMode { get; set; }
    [ObservableProperty]
    public partial string UrlInput { get; set; } = string.Empty;
    [ObservableProperty]
    public partial string FilePath { get; set; } = string.Empty;
    [ObservableProperty]
    public partial ScanResult? Result { get; set; } = null;
    [ObservableProperty]
    public partial string FileDisplayInfo { get; set; } = "No file selected";
    [RelayCommand]
    private void SelectMode(ScanMode mode)
    {
        SelectedMode = mode;
    }
    [RelayCommand]
    public async Task ScanAsync()
    {
        try
        {
            if (SelectedMode == ScanMode.File)
            {
                string hash = await FileHasher.ComputeSha256Async(FilePath);
                ScanResult result = await _scanManager.ScanHashAsync(hash);
                Result = result;
            }
            else
            {
                ScanResult result = await _scanManager.ScanUrlAsync(UrlInput);
                Result = result;
            }
        }
        catch (ApiKeyMissingException ex)
        {
            _navigationService.NavigateToSettings(ex.Message);
        }
        catch (AuthenticationException ex)
        {
            _navigationService.NavigateToSettings(ex.Message);
        }
    }
    [RelayCommand]
    public async Task PickFileAsync()
    {
        string? path = await _filePickerService.FilePickerAsync();

        if (path is null)
            return;

        FilePath = path;
        FileInfo fileInfo = new FileInfo(path);
        string name = Path.GetFileName(path);
        long bytes = fileInfo.Length;
        string size = FileSizeFormatter.Format(bytes);
        FileDisplayInfo = $"{name} - {size}";
    }


}
