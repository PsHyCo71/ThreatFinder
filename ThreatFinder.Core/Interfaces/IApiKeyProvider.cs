using System.Threading.Tasks;

namespace ThreatFinder.Core;

/// <summary>
/// Defines the contract for retrieving and storing API keys for external providers.
/// </summary>
public interface IApiKeyProvider
{
    /// <summary>
    /// Retrieves the API key associated with the specified provider.
    /// </summary>
    /// <param name="providerName">Name of the provider whose API key is requested.</param>
    /// <returns>The API key associated with the specified provider.</returns>
    Task<string> GetApiKeyAsync(string providerName);

    /// <summary>
    /// Saves or updates the API key associated with the specified provider.
    /// </summary>
    /// <param name="providerName">Name of the provider whose API key is being saved.</param>
    /// <param name="key">API key to store.</param>
    Task SaveApiKeyAsync(string providerName, string key);
}