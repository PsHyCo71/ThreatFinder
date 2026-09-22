using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.IO;
using System;
using System.Threading.Tasks;
using ThreatFinder.Core;
using Avalonia;
using Avalonia.Styling;

namespace ThreatFinder.ViewModels;

/// <summary>
/// View model responsible for handling file and URL scanning operations.
/// </summary>
public partial class ScanViewModel : ViewModelBase
{
    /// <summary>
    /// Defines the available scanning modes.
    /// </summary>
    public enum ScanMode { Url, File }

    /// <summary>
    /// View model used to store and display scan results.
    /// </summary>
    private ResultsViewModel _resultViewModel;

    /// <summary>
    /// Manager responsible for executing scans through the available providers.
    /// </summary>
    private ScanManager _scanManager;

    /// <summary>
    /// Service responsible for selecting files through the platform file picker.
    /// </summary>
    private IFilePickerService _filePickerService;

    /// <summary>
    /// Service used to navigate between application views.
    /// </summary>
    private INavigationService _navigationService;

    /// <summary>
    /// Initializes a new instance of the <see cref="ScanViewModel"/> class.
    /// </summary>
    /// <param name="scanManager">Manager responsible for executing scans.</param>
    /// <param name="filePickerService">Service used to select files.</param>
    /// <param name="navigationService">Service used to navigate between views.</param>
    /// <param name="resultsViewModel">View model used to display scan results.</param>
    public ScanViewModel(ScanManager scanManager, IFilePickerService filePickerService, INavigationService navigationService, ResultsViewModel resultsViewModel)
    {
        _resultViewModel = resultsViewModel;
        _scanManager = scanManager;
        _filePickerService = filePickerService;
        _navigationService = navigationService;
    }

    /// <summary>
    /// Navigates to the application settings view.
    /// </summary>
    [RelayCommand]
    public void GoToSettings()
    {
        _navigationService.NavigateToSettings();
    }

    /// <summary>
    /// Gets or sets the currently selected scanning mode.
    /// </summary>
    [ObservableProperty]
    public partial ScanMode SelectedMode { get; set; }

    /// <summary>
    /// Gets or sets the URL entered by the user.
    /// </summary>
    [ObservableProperty]
    public partial string UrlInput { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the path of the selected file.
    /// </summary>
    [ObservableProperty]
    public partial string FilePath { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the information displayed for the selected file.
    /// </summary>
    [ObservableProperty]
    public partial string FileDisplayInfo { get; set; } = "No file selected";

    /// <summary>
    /// Gets or sets a value indicating whether an error occurred while handling the selected file.
    /// </summary>
    [ObservableProperty]
    public partial bool FileError { get; set; } = false;

    /// <summary>
    /// Gets or sets a value indicating whether a scan is currently in progress.
    /// </summary>
    [ObservableProperty]
    public partial bool IsScanning { get; set; } = false;

    /// <summary>
    /// Changes the current scanning mode.
    /// </summary>
    /// <param name="mode">Scanning mode to select.</param>
    [RelayCommand]
    private void SelectMode(ScanMode mode)
    {
        SelectedMode = mode;
    }

    /// <summary>
    /// Performs a scan using the currently selected scanning mode.
    /// </summary>
    /// <remarks>
    /// For file scans, the selected file is hashed using SHA-256 before being checked
    /// against the available providers. For URL scans, the entered URL is checked directly.
    /// </remarks>
    [RelayCommand]
    public async Task ScanAsync()
    {
        // Prevents multiple scans from being started simultaneously.
        if (IsScanning)
            return;

        // Ensures that a file has been selected before starting a file scan.
        if (SelectedMode == ScanMode.File && string.IsNullOrWhiteSpace(FilePath))
        {
            FileError = true;
            FileDisplayInfo = "Please select a file before scanning.";
            return;
        }

        // Prevents a URL scan when no URL has been entered.
        if (SelectedMode == ScanMode.Url && string.IsNullOrWhiteSpace(UrlInput))
        {
            return;
        }

        IsScanning = true;
        try
        {
            if (SelectedMode == ScanMode.File)
            {
                // Calculates the SHA-256 hash of the selected file.
                string hash = await FileHasher.ComputeSha256Async(FilePath);

                // Checks the hash against all applicable providers.
                _resultViewModel.Result = await _scanManager.ScanHashAsync(hash);

                // Stores the file information required by the results view.
                _resultViewModel.FileDisplayInfo = FileDisplayInfo;
                _resultViewModel.IsFileResult = true;

                // Navigates to the results view.
                _navigationService.NavigateToResults(_resultViewModel);
            }
            else
            {
                // Checks the entered URL against all applicable providers.
                _resultViewModel.Result = await _scanManager.ScanUrlAsync(UrlInput);
                // Ensures the results view does not treat this as a file scan.
                _resultViewModel.IsFileResult = false;
                // Navigates to the results view.
                _navigationService.NavigateToResults(_resultViewModel);
            }
        }
        catch (ApiKeyMissingException ex)
        {
            // Redirects the user to the settings view when an API key is missing.
            await _navigationService.NavigateToSettings(ex.Message);
        }
        catch (AuthenticationException ex)
        {
            // Redirects the user to the settings view when authentication fails.
            await _navigationService.NavigateToSettings(ex.Message);
        }
        finally
        {
            // Allows a new scan to be started after the current operation has completed.
            IsScanning = false;
        }
    }

    /// <summary>
    /// Opens the file picker and processes the selected file.
    /// </summary>
    /// <remarks>
    /// Retrieves the file path, verifies that the file can be accessed,
    /// and prepares the file name and size for display.
    /// </remarks>
    [RelayCommand]
    public async Task PickFileAsync()
    {
        // Opens the platform file picker and retrieves the selected file path.
        string? path = await _filePickerService.FilePickerAsync();

        // Stops processing if the user cancels the file selection.
        if (path is null)
            return;

        try
        {
            // Stores the selected file path.
            FilePath = path;

            // Retrieves information about the selected file.
            FileInfo fileInfo = new FileInfo(path);
            string name = Path.GetFileName(path);
            long bytes = fileInfo.Length;

            // Converts the file size into a human-readable format.
            string size = FileSizeFormatter.Format(bytes);

            // Clears any previous file error and updates the displayed information.
            FileError = false;
            FileDisplayInfo = $"{name} - {size}";
        }
        catch (FileNotFoundException ex)
        {
            // Displays an error when the selected file cannot be found.
            FileError = true;
            FileDisplayInfo = ex.Message;
        }
        catch (UnauthorizedAccessException)
        {
            // Displays an error when the application cannot access the selected file.
            FileError = true;
            FileDisplayInfo = "You lack the necessary permits to access this file!";
        }
        catch (IOException ex)
        {
            // Displays any other input/output error encountered while accessing the file.
            FileError = true;
            FileDisplayInfo = ex.Message;
        }
    }
	
    /// <summary>
    /// Toggles the application's theme between Light and Dark mode.
    /// </summary>
    [RelayCommand]
    private void ToggleTheme()
    {
        // Gets the current Avalonia application instance.
        var app = Application.Current!;

        // Checks the current theme:
        // if it is Dark, switches to Light;
        // otherwise, switches to Dark.
        app.RequestedThemeVariant = app.ActualThemeVariant == ThemeVariant.Dark
            ? ThemeVariant.Light
            : ThemeVariant.Dark;
    }
}