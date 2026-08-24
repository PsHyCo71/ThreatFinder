using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using ThreatFinder.Core;

namespace ThreatFinder.ViewModels;

/// <summary>
/// View model responsible for managing the currently displayed view
/// and handling navigation between the application's main views.
/// </summary>
public partial class ShellViewModel : ViewModelBase, INavigationService
{
    /// <summary>
    /// Gets or sets the view model currently displayed by the application.
    /// </summary>
    [ObservableProperty]
    public partial ViewModelBase CurrentViewModel { get; set; }

    /// <summary>
    /// View model responsible for displaying scan results.
    /// </summary>
    private ResultsViewModel _resultViewModel;

    /// <summary>
    /// View model responsible for handling scan operations.
    /// </summary>
    private readonly ScanViewModel _scanViewModel;

    /// <summary>
    /// Service used to retrieve and store provider API keys.
    /// </summary>
    private readonly IApiKeyProvider _apiKeyProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="ShellViewModel"/> class.
    /// </summary>
    /// <param name="scanManager">Manager responsible for executing scans.</param>
    /// <param name="filePickerService">Service used to select files.</param>
    /// <param name="apiKeyProvider">Service used to retrieve and store API keys.</param>
    public ShellViewModel(ScanManager scanManager, IFilePickerService filePickerService, IApiKeyProvider apiKeyProvider)
    {
        _apiKeyProvider = apiKeyProvider;
        _resultViewModel = new ResultsViewModel(this);
        _scanViewModel = new ScanViewModel(scanManager, filePickerService, this, _resultViewModel);
        CurrentViewModel = _scanViewModel;
    }

    /// <summary>
    /// Navigates to the settings view and initializes it with the stored API keys.
    /// </summary>
    /// <param name="errorMessage">Optional error message to display in the settings view.</param>
    public async Task NavigateToSettings(string? errorMessage = null)
    {
        // Creates a new settings view model using the configured API key provider.
        var settingsViewModel = new SettingsViewModel(_apiKeyProvider, this);

        // Loads the currently stored API keys before displaying the settings view.
        await settingsViewModel.InitializeAsync();

        // Sets the error message, if one was provided.
        settingsViewModel.ErrorMessage = errorMessage ?? string.Empty;

        // Displays the settings view.
        CurrentViewModel = settingsViewModel;
    }

    /// <summary>
    /// Navigates to the scan view.
    /// </summary>
    public void NavigateToScan()
    {
        CurrentViewModel = _scanViewModel;
    }

    /// <summary>
    /// Navigates to the specified results view model.
    /// </summary>
    /// <typeparam name="TResults">Type of the object containing the results view model.</typeparam>
    /// <param name="resultsViewModel">Object containing the view model to display.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when the provided results view model is null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when the provided object does not derive from <see cref="ViewModelBase"/>.
    /// </exception>
    void INavigationService.NavigateToResults<TResults>(TResults resultsViewModel)
    {
        // Ensures that a results view model has been provided.
        ArgumentNullException.ThrowIfNull(resultsViewModel);

        // Ensures that the provided object is a valid view model.
        if (resultsViewModel is not ViewModelBase viewModel)
        {
            throw new ArgumentException("The results view model must derive from ViewModelBase.", nameof(resultsViewModel));
        }

        // Displays the provided results view model.
        CurrentViewModel = viewModel;
    }
}