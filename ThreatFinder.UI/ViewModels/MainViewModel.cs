using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using ThreatFinder.Core;

namespace ThreatFinder.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    public enum ScanMode { Url, File }

    private ScanManager _scanManager;
    public MainViewModel(ScanManager scanManager)
    {
        _scanManager = scanManager;
    }
    
    [ObservableProperty]
    public partial ScanMode SelectedMode { get; set; }
    [ObservableProperty]
    public partial string UrlInput { get; set; } = string.Empty;
    [ObservableProperty]
    public partial string FilePath { get; set; } = string.Empty;
    [ObservableProperty]
    public partial ScanResult? Result { get; set; } = null;
    [RelayCommand]
    private void SelectMode(ScanMode mode)
    {
        SelectedMode = mode;
    }
    [RelayCommand]
    private async Task ScanAsync()
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
}
