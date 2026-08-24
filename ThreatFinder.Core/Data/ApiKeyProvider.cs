using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace ThreatFinder.Core;

/// <summary>
/// Provides access to API keys stored in the application configuration file.
/// </summary>
public class ApiKeyProvider : IApiKeyProvider
{
    /// <summary>
    /// Retrieves the API key associated with the specified provider.
    /// </summary>
    /// <param name="providerName">Name of the provider whose API key is requested.</param>
    /// <returns>The API key associated with the specified provider.</returns>
    /// <exception cref="ApiKeyMissingException">
    /// Thrown when the configuration file does not exist or the requested API key is missing.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when the specified provider is not supported.
    /// </exception>
    public async Task<string> GetApiKeyAsync(string providerName)
    {
        if (File.Exists("appsettings.json"))
        {
            // Reads and deserializes the application configuration.
            string json = await File.ReadAllTextAsync("appsettings.json");
            var keys = JsonSerializer.Deserialize<AppSettings>(json);

            // Verifies that the requested provider has a configured API key.
            if
            (
                (providerName == "MalwareBazaar" && keys?.MalwareBazaarApiKey is null)
                ||
                (providerName == "URLhaus" && keys?.UrlhausApiKey is null)
            )
            {
                throw new ApiKeyMissingException() { ProviderName = providerName };
            }

            // Returns the API key corresponding to the requested provider.
            return providerName switch
            {
                "MalwareBazaar" => keys?.MalwareBazaarApiKey ?? "Missing",
                "URLhaus" => keys?.UrlhausApiKey ?? "Missing",
                _ => throw new ArgumentException($"Unknown provider: {providerName}")
            };
        }
        else
        {
            // The configuration file is required to retrieve an API key.
            throw new ApiKeyMissingException() { ProviderName = providerName };
        }
    }

    /// <summary>
    /// Saves or updates the API key associated with the specified provider.
    /// </summary>
    /// <param name="providerName">Name of the provider whose API key is being saved.</param>
    /// <param name="key">API key to store in the application configuration.</param>
    /// <exception cref="ArgumentException">
    /// Thrown when the specified provider is not supported.
    /// </exception>
    public async Task SaveApiKeyAsync(string providerName, string key)
    {
        AppSettings keys;

        if (File.Exists("appsettings.json"))
        {
            // Loads the existing configuration, creating a new instance if deserialization fails.
            string json = await File.ReadAllTextAsync("appsettings.json");
            keys = JsonSerializer.Deserialize<AppSettings>(json) ?? new AppSettings();
        }
        else
        {
            // Creates a new configuration when the file does not exist.
            keys = new AppSettings();
        }

        // Updates the API key associated with the specified provider.
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

        // Serializes the updated configuration and persists it to the configuration file.
        await File.WriteAllTextAsync("appsettings.json", JsonSerializer.Serialize(keys, new JsonSerializerOptions { WriteIndented = true }));
    }
}