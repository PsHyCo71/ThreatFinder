using System.Collections.Generic;
using System.Linq;

namespace ThreatFinder.Core;
public record ScanResult
{
    public required string Target { get; init; }
    public required IReadOnlyList<EngineResult> Results { get; init; }
    public int MaliciousCount => Results.OfType<MaliciousResult>().Count();
    public int CleanCount => Results.OfType<CleanResult>().Count();
    public int ExceptionCount => Results.OfType<ErrorResult>().Count();
}