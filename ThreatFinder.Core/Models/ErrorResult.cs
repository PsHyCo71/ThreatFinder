namespace ThreatFinder.Core;

public sealed record ErrorResult(string ProviderName) : EngineResult(ProviderName)
{
    public required string Message { get; init; }
}