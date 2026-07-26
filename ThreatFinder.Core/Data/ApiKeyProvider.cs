using System;
using System.IO;
using System.Text.Json;
using Avalonia.Input;

namespace ThreatFinder.Core;

public class ApiKeyProvider : IApiKeyProvider
{
    public string GetApiKey(string providerName)
    {
        if (File.Exists("appsettings.json"))
        {
            string json = File.ReadAllText("appsettings.json");
            var keys = JsonSerializer.Deserialize<AppSettings>(json);
            if (keys is null)
                return string.Empty;
            return providerName switch
            {
                "MalwareBazaar" => keys.MalwareBazaarApiKey ?? string.Empty,
                "URLhaus" => keys?.UrlhausApiKey ?? string.Empty,
                _ => throw new ArgumentException($"Unknown provider: {providerName}")
            };
        }
        else
        {
            throw new ApiKeyMissingException()
            {
                ProviderName = providerName
            };
        }
    }

    public void SaveApiKey(string apiKey)
    {
        
    }
}