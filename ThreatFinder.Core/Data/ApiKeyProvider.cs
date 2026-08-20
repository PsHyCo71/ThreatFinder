using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace ThreatFinder.Core;

public class ApiKeyProvider : IApiKeyProvider
{
    public async Task<string> GetApiKeyAsync(string providerName)
    {
        if (File.Exists("appsettings.json"))
        {
            string json = await File.ReadAllTextAsync("appsettings.json");
            var keys = JsonSerializer.Deserialize<AppSettings>(json);

            if
            (
                (providerName == "MalwareBazaar" && keys?.MalwareBazaarApiKey is null)
                ||
                (providerName == "URLhaus" && keys?.UrlhausApiKey is null)
            )
            {
                throw new ApiKeyMissingException() { ProviderName = providerName };
            }

            return providerName switch
            {
                "MalwareBazaar" => keys?.MalwareBazaarApiKey ?? string.Empty,
                "URLhaus" => keys?.UrlhausApiKey ?? string.Empty,
                _ => throw new ArgumentException($"Unknown provider: {providerName}")
            };
        }
        else
        {
            throw new ApiKeyMissingException() { ProviderName = providerName };
        }
    }

    public async Task SaveApiKeyAsync(string providerName, string key)
    {
        AppSettings keys;
        if (File.Exists("appsettings.json"))
        {
            string json = await File.ReadAllTextAsync("appsettings.json");
            keys = JsonSerializer.Deserialize<AppSettings>(json) ?? new AppSettings();
        }
        else
        {
            keys = new AppSettings();
        }

        switch (providerName)
        {
            case "MalwareBazaar":
                keys.MalwareBazaarApiKey = key;
                break;
            case "URLhaus":
                keys.UrlhausApiKey = key;
                break;
            default:
                throw new ArgumentException($"Unknown provider: {providerName}");
        }

        await File.WriteAllTextAsync("appsettings.json", JsonSerializer.Serialize(keys, new JsonSerializerOptions { WriteIndented = true }));
    }
}
