namespace ThreatFinder.Core;

/// <summary>
/// Represents a result indicating that no threat was detected by the provider.
/// </summary>
/// <param name="ProviderName">Name of the provider that produced the result.</param>
public sealed record CleanResult(string ProviderName) : EngineResult(ProviderName)
{
    /// <summary>
    /// Gets the message associated with the clean result.
    /// </summary>
    public required string Message { get; init; }
}