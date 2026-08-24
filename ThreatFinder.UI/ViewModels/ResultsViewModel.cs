using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ThreatFinder.Core;

namespace ThreatFinder.ViewModels;

/// <summary>
/// View model responsible for displaying the results of a scan.
/// </summary>
public partial class ResultsViewModel : ViewModelBase
{
    /// <summary>
    /// Service used to navigate between application views.
    /// </summary>
    private INavigationService _navigationService;

    /// <summary>
    /// Initializes a new instance of the <see cref="ResultsViewModel"/> class.
    /// </summary>
    /// <param name="navigationService">Service used to handle application navigation.</param>
    public ResultsViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;
    }

    /// <summary>
    /// Gets or sets the result of the completed scan.
    /// </summary>
    [ObservableProperty]
    public partial ScanResult? Result { get; set; } = null;

    /// <summary>
    /// Gets or sets the formatted information displayed for a scanned file.
    /// </summary>
    [ObservableProperty]
    public partial string? FileDisplayInfo { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the current result is associated with a file.
    /// </summary>
    [ObservableProperty]
    public partial bool IsFileResult { get; set; } = false;

    /// <summary>
    /// Navigates back to the scan view.
    /// </summary>
    [RelayCommand]
    public void GoToScan()
    {
        _navigationService.NavigateToScan();
    }
}