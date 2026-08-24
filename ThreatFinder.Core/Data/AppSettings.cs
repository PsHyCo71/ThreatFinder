namespace ThreatFinder.Core;

/// <summary>
/// Represents the application settings used to store API keys for external providers.
/// </summary>
public class AppSettings
{
    /// <summary>
    /// Gets or sets the API key used to authenticate requests to MalwareBazaar.
    /// </summary>
    public string? MalwareBazaarApiKey { get; set; }

    /// <summary>
    /// Gets or sets the API key used to authenticate requests to URLhaus.
    /// </summary>
    public string? UrlhausApiKey { get; set; }
}