namespace ThreatFinder.Core;

/// <summary>
/// Represents the base type for results returned by threat information providers.
/// </summary>
/// <param name="ProviderName">Name of the provider that produced the result.</param>
public abstract record EngineResult(string ProviderName);