using System.Collections.Generic;

namespace ThreatFinder.Core;

/// <summary>
/// Represents the results of a scan performed on a target.
/// </summary>
public record ScanResult
{
    /// <summary>
    /// Gets the target that was scanned.
    /// </summary>
    public required string Target { get; init; }

    /// <summary>
    /// Gets the results returned by the providers involved in the scan.
    /// </summary>
    public required IReadOnlyList<EngineResult> Results { get; init; }
}