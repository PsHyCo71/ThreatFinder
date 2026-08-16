using CommunityToolkit.Mvvm.ComponentModel;
using ThreatFinder.Core;

namespace ThreatFinder.ViewModels;

public partial class ShellViewModel : ViewModelBase, INavigationService
{
    [ObservableProperty]
    public partial ViewModelBase CurrentViewModel { get; set; }

    private ScanViewModel _scanViewModel;
    private IApiKeyProvider _apiKeyProvider;
    
    public ShellViewModel(ScanViewModel scanViewModel, IApiKeyProvider apiKeyProvider)
    {
        _scanViewModel = scanViewModel;
        _apiKeyProvider = apiKeyProvider;
        CurrentViewModel = _scanViewModel!;
    }

    public async void NavigateToSettings(string? errorMessage = null)
    {
        var settingsViewModel = new SettingsViewModel(_apiKeyProvider);
        await settingsViewModel.InitializeAsync();
        settingsViewModel.ErrorMessage = errorMessage ?? string.Empty;
        CurrentViewModel = settingsViewModel;
    }

    public void NavigateToScan()
    {
        CurrentViewModel = _scanViewModel; 
    }
}