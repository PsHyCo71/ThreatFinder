using System;
using System.Net;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ThreatFinder.Core;

namespace ThreatFinder.ViewModels;

public partial class SettingsViewModel : ViewModelBase
{
    [ObservableProperty]
    public partial string MalwareBazaarKey { get; set; } = string.Empty;

    [ObservableProperty]
    public partial bool IsEditingMalwareBazaarKey { get; set; } = false;

    [ObservableProperty]
    public partial string UrlhausKey { get; set; } = string.Empty;

    [ObservableProperty]
    public partial bool IsEditingUrlhausKey { get; set; } = false;
    [ObservableProperty]
    public partial string ErrorMessage { get; set; } = string.Empty;

    private readonly IApiKeyProvider _apiKeyProvider;

    public SettingsViewModel(IApiKeyProvider apiKeyProvider)
    {
        _apiKeyProvider = apiKeyProvider;
    }

    public async Task InitializeAsync()
    {
        try
        {
            MalwareBazaarKey = await _apiKeyProvider.GetApiKeyAsync("MalwareBazaar");
        }
        catch (ApiKeyMissingException)
        {
            MalwareBazaarKey = string.Empty;
        }
        try
        {
            UrlhausKey = await _apiKeyProvider.GetApiKeyAsync("URLhaus");
        }
        catch
        {
            UrlhausKey = string.Empty;
        }
    }

    [RelayCommand]
    public async Task ConfirmKeyAsync(string providerName)
    {
        switch (providerName)
        {
            case "MalwareBazaar":
                if (IsEditingMalwareBazaarKey)
                {
                    await _apiKeyProvider.SaveApiKeyAsync(providerName, MalwareBazaarKey);
                    IsEditingMalwareBazaarKey = false;
                }
                else
                {
                    IsEditingMalwareBazaarKey = true;
                }
                break;

            case "URLhaus":
                if (IsEditingUrlhausKey)
                {
                    await _apiKeyProvider.SaveApiKeyAsync(providerName, UrlhausKey);
                    IsEditingUrlhausKey = false;
                }
                else
                {
                    IsEditingUrlhausKey = true;
                }
                break;

            default:
                throw new ArgumentException($"Unknown provider: {providerName}");
        }
    }
}