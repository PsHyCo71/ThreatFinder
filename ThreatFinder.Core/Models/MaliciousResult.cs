using System;
using System.Collections.Generic;

namespace ThreatFinder.Core;

public sealed record MaliciousResult(string ProviderName) : EngineResult(ProviderName)
{
    public required string ThreatType { get; init; }
    public required DateTime Timestamp { get; init; }
    public required IReadOnlyList<string> Tags { get; init; } 
}