using System.Threading.Tasks;

namespace ThreatFinder.Core;

public interface IApiKeyProvider
{
    Task<string> GetApiKeyAsync(string providerName);
    Task SaveApiKeyAsync(string providerName, string key);
}