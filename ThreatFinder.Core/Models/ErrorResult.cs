namespace ThreatFinder.Core;

/// <summary>
/// Represents a result indicating that an error occurred while processing a request.
/// </summary>
/// <param name="ProviderName">Name of the provider that produced the result.</param>
public sealed record ErrorResult(string ProviderName) : EngineResult(ProviderName)
{
    /// <summary>
    /// Gets the message describing the error that occurred.
    /// </summary>
    public required string Message { get; init; }
}