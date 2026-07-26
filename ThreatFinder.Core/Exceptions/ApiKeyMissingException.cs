using System;

namespace ThreatFinder.Core;

public class ApiKeyMissingException : Exception
{
    public required string ProviderName { get; init; }
    public override string Message => $"API key for provider '{ProviderName}' is missing.";
}