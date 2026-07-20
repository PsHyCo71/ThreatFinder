using System.Threading.Tasks;

namespace ThreatFinder.Core;
public interface IThreatIntelProvider
{
    string Name { get; }
    bool SupportsHash { get; }
    bool SupportsUrl { get; }

    Task<EngineResult> CheckFileHashAsync(string sha256Hash);

    Task<EngineResult> CheckUrlAsync(string url);
}