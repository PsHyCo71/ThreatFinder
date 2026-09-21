using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Avalonia;
using Avalonia.Styling;
using ThreatFinder.Core;

namespace ThreatFinder.ViewModels;

/// <summary>
/// View model responsible for managing and displaying API keys for the supported providers.
/// </summary>
public partial class SettingsViewModel : ViewModelBase
{
    /// <summary>
    /// Gets or sets the API key associated with MalwareBazaar.
    /// </summary>
    [ObservableProperty]
    public partial string MalwareBazaarKey { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether the MalwareBazaar API key is currently being edited.
    /// </summary>
    [ObservableProperty]
    public partial bool IsEditingMalwareBazaarKey { get; set; } = false;

    /// <summary>
    /// Gets or sets the API key associated with URLhaus.
    /// </summary>
    [ObservableProperty]
    public partial string UrlhausKey { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether the URLhaus API key is currently being edited.
    /// </summary>
    [ObservableProperty]
    public partial bool IsEditingUrlhausKey { get; set; } = false;

    /// <summary>
    /// Gets or sets the error message displayed in the settings view.
    /// </summary>
    [ObservableProperty]
    public partial string ErrorMessage { get; set; } = string.Empty;

    /// <summary>
    /// Provides access to the stored API keys.
    /// </summary>
    private readonly IApiKeyProvider _apiKeyProvider;

    /// <summary>
    /// Service used to navigate between application views.
    /// </summary>
    private INavigationService _navigationService;

    /// <summary>
    /// Initializes a new instance of the <see cref="SettingsViewModel"/> class.
    /// </summary>
    /// <param name="apiKeyProvider">Service used to retrieve and store API keys.</param>
    /// <param name="navigationService">Service used to navigate between views.</param>
    public SettingsViewModel(IApiKeyProvider apiKeyProvider, INavigationService navigationService)
    {
        _apiKeyProvider = apiKeyProvider;
        _navigationService = navigationService;
    }

    /// <summary>
    /// Loads the stored API keys for all supported providers.
    /// </summary>
    public async Task InitializeAsync()
    {
        try
        {
            // Attempts to retrieve the stored MalwareBazaar API key.
            MalwareBazaarKey = await _apiKeyProvider.GetApiKeyAsync("MalwareBazaar");
        }
        catch (ApiKeyMissingException)
        {
            // Uses an empty value when no MalwareBazaar API key is configured.
            MalwareBazaarKey = string.Empty;
        }

        try
        {
            // Attempts to retrieve the stored URLhaus API key.
            UrlhausKey = await _apiKeyProvider.GetApiKeyAsync("URLhaus");
        }
        catch (ApiKeyMissingException)
        {
            // Uses an empty value when the URLhaus API key cannot be retrieved.
            UrlhausKey = string.Empty;
        }
    }

    /// <summary>
    /// Navigates to the scan view.
    /// </summary>
    [RelayCommand]
    public void GoToScan()
    {
        _navigationService.NavigateToScan();
    }

    /// <summary>
    /// Enables editing for the API key associated with the specified provider.
    /// </summary>
    /// <param name="providerName">Name of the provider whose API key should be edited.</param>
    /// <exception cref="ArgumentException">
    /// Thrown when the specified provider is not supported.
    /// </exception>
    [RelayCommand]
    public void OpenEdit(string providerName)
    {
        switch (providerName)
        {
            case "MalwareBazaar":
                IsEditingMalwareBazaarKey = true;
                break;
            case "URLhaus":
                IsEditingUrlhausKey = true;
                break;
            default:
                throw new ArgumentException($"Unknown provider: {providerName}");

        }
    }

    /// <summary>
    /// Saves the API key associated with the specified provider.
    /// </summary>
    /// <param name="providerName">Name of the provider whose API key should be saved.</param>
    /// <exception cref="ArgumentException">
    /// Thrown when the specified provider is not supported.
    /// </exception>
    [RelayCommand]
    public async Task SaveKeyAsync(string providerName)
    {
        switch (providerName)
        {
            case "MalwareBazaar":
                // Saves the key only when the MalwareBazaar field is currently being edited.
                if (IsEditingMalwareBazaarKey)
                {
                    await _apiKeyProvider.SaveApiKeyAsync(providerName, MalwareBazaarKey);
                    IsEditingMalwareBazaarKey = false;
                }
                break;

            case "URLhaus":
                // Saves the key only when the URLhaus field is currently being edited.
                if (IsEditingUrlhausKey)
                {
                    await _apiKeyProvider.SaveApiKeyAsync(providerName, UrlhausKey);
                    IsEditingUrlhausKey = false;
                }
                break;

            default:
                throw new ArgumentException($"Unknown provider: {providerName}");
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