namespace ThreatFinder.Core;

public sealed record CleanResult(string ProviderName) : EngineResult(ProviderName);