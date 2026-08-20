using System;
using CommunityToolkit.Mvvm.ComponentModel;
using ThreatFinder.Core;

namespace ThreatFinder.ViewModels;

public partial class ShellViewModel : ViewModelBase, INavigationService
{
    [ObservableProperty]
    public partial ViewModelBase CurrentViewModel { get; set; }

    private ResultsViewModel _resultViewModel;
    private readonly ScanViewModel _scanViewModel;
    private readonly IApiKeyProvider _apiKeyProvider;

    public ShellViewModel(ScanManager scanManager, IFilePickerService filePickerService, IApiKeyProvider apiKeyProvider, ResultsViewModel resultsViewModel)
    {
        _apiKeyProvider = apiKeyProvider;
        _resultViewModel = resultsViewModel;
        _scanViewModel = new ScanViewModel(scanManager, filePickerService, this, resultsViewModel);
        CurrentViewModel = _scanViewModel;
    }

    public async void NavigateToSettings(string? errorMessage = null)
    {
        var settingsViewModel = new SettingsViewModel(_apiKeyProvider, this);
        await settingsViewModel.InitializeAsync();
        settingsViewModel.ErrorMessage = errorMessage ?? string.Empty;
        CurrentViewModel = settingsViewModel;
    }

    public void NavigateToScan()
    {
        CurrentViewModel = _scanViewModel;
    }

    void INavigationService.NavigateToResults<TResults>(TResults resultsViewModel)
    {
        ArgumentNullException.ThrowIfNull(resultsViewModel);

        if (resultsViewModel is not ViewModelBase viewModel)
        {
            throw new ArgumentException("The results view model must derive from ViewModelBase.", nameof(resultsViewModel));
        }

        CurrentViewModel = viewModel;
    }
}