using System.Threading.Tasks;

namespace ThreatFinder.Core;

public interface IApiKeyProvider
{
    string GetApiKey(string providerName);
    void SaveApiKey(string providerName, string key);
}