using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.IO;
using System;
using System.Threading.Tasks;
using ThreatFinder.Core;

namespace ThreatFinder.ViewModels;

public partial class ScanViewModel : ViewModelBase
{
    public enum ScanMode { Url, File }

    private ResultsViewModel _resultViewModel;
    private ScanManager _scanManager;
    private IFilePickerService _filePickerService;
    private INavigationService _navigationService;
    public ScanViewModel(ScanManager scanManager, IFilePickerService filePickerService, INavigationService navigationService, ResultsViewModel resultsViewModel)
    {
        _resultViewModel = resultsViewModel;
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
    public partial string FileDisplayInfo { get; set; } = "No file selected";
    [ObservableProperty]
    public partial bool FileError { get; set; } = false;
    [ObservableProperty]
    public partial bool IsScanning { get; set; } = false;

    [RelayCommand]
    private void SelectMode(ScanMode mode)
    {
        SelectedMode = mode;
    }
    [RelayCommand]
    public async Task ScanAsync()
    {
        if (IsScanning)
            return;
        
        if (SelectedMode == ScanMode.File && string.IsNullOrWhiteSpace(FilePath))
        {
            FileError = true;
            FileDisplayInfo = "Please select a file before scanning.";
            return;
        }

        if (SelectedMode == ScanMode.Url && string.IsNullOrWhiteSpace(UrlInput))
        {
            return;
        }

        IsScanning = true;
        try
        {
            if (SelectedMode == ScanMode.File)
            {
                string hash = await FileHasher.ComputeSha256Async(FilePath);
                _resultViewModel.Result = await _scanManager.ScanHashAsync(hash);
                _navigationService.NavigateToResults(_resultViewModel);
            }
            else
            {
                _resultViewModel.Result = await _scanManager.ScanUrlAsync(UrlInput);
                _navigationService.NavigateToResults(_resultViewModel);
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
        finally
        {
            IsScanning = false;
        }
    }

    [RelayCommand]
    public async Task PickFileAsync()
    {
        string? path = await _filePickerService.FilePickerAsync();

        if (path is null)
            return;

        try
        {
            FilePath = path;
            FileInfo fileInfo = new FileInfo(path);
            string name = Path.GetFileName(path);
            long bytes = fileInfo.Length;
            string size = FileSizeFormatter.Format(bytes);
            FileError = false;
            FileDisplayInfo = $"{name} - {size}";
        }
        catch (FileNotFoundException ex)
        {
            FileError = true;
            FileDisplayInfo = ex.Message;
        }
        catch (UnauthorizedAccessException)
        {
            FileError = true;
            FileDisplayInfo = "You lack the necessary permits to access this file!";
        }
        catch (IOException ex)
        {
            FileError = true;
            FileDisplayInfo = ex.Message;
        }
    }


}
