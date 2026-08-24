using System.Threading.Tasks;

namespace ThreatFinder.Core;

/// <summary>
/// Defines the contract for a provider that supplies information about potential threats.
/// </summary>
public interface IThreatIntelProvider
{
    /// <summary>
    /// Gets the name of the threat information provider.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Gets a value indicating whether the provider supports file hash checks.
    /// </summary>
    bool SupportsHash { get; }

    /// <summary>
    /// Gets a value indicating whether the provider supports URL checks.
    /// </summary>
    bool SupportsUrl { get; }

    /// <summary>
    /// Checks a file hash against the provider's threat information database.
    /// </summary>
    /// <param name="authKey">Authentication key used to access the provider.</param>
    /// <param name="sha256Hash">SHA-256 hash of the file to check.</param>
    /// <returns>The result returned by the provider.</returns>
    Task<EngineResult> CheckFileHashAsync(string authKey, string sha256Hash);

    /// <summary>
    /// Checks a URL against the provider's threat information database.
    /// </summary>
    /// <param name="authKey">Authentication key used to access the provider.</param>
    /// <param name="url">URL to check.</param>
    /// <returns>The result returned by the provider.</returns>
    Task<EngineResult> CheckUrlAsync(string authKey, string url);
}