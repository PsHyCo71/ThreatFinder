using System.Collections.Generic;

namespace ThreatFinder.Core;

/// <summary>
/// Represents a result indicating that a malicious threat was detected by the provider.
/// </summary>
/// <param name="ProviderName">Name of the provider that produced the result.</param>
public sealed record MaliciousResult(string ProviderName) : EngineResult(ProviderName)
{
    /// <summary>
    /// Gets the type or category of the detected threat.
    /// </summary>
    public required string ThreatType { get; init; }

    /// <summary>
    /// Gets the timestamp associated with the detected threat.
    /// </summary>
    public required string Timestamp { get; init; }

    /// <summary>
    /// Gets the tags associated with the detected threat.
    /// </summary>
    public required IReadOnlyList<string> Tags { get; init; } 
}