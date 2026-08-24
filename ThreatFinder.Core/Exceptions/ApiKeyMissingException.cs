using System;

namespace ThreatFinder.Core;

/// <summary>
/// Represents an exception thrown when an API key required by a provider is missing.
/// </summary>
public class ApiKeyMissingException : Exception
{
    /// <summary>
    /// Gets the name of the provider whose API key is missing.
    /// </summary>
    public required string ProviderName { get; init; }

    /// <summary>
    /// Gets a message describing the missing API key and its associated provider.
    /// </summary>
    public override string Message => $"API key for provider '{ProviderName}' is missing.";
}