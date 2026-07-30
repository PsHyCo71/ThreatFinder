using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ThreatFinder.Core;

public class ScanManager
{
    private readonly IEnumerable<IThreatIntelProvider> _providers;
    private readonly IApiKeyProvider _apiKeyProvider;

    public ScanManager(IEnumerable<IThreatIntelProvider> providers, IApiKeyProvider apiKeyProvider)
    {
        _providers = providers;
        _apiKeyProvider = apiKeyProvider;
    }

    public async Task<ScanResult> ScanHashAsync(string hash)
    {
        var applicableProviders = _providers.Where(p => p.SupportsHash);
        var tasks = applicableProviders.Select(async provider =>
        {
            try
            {
                string MBkey = await _apiKeyProvider.GetApiKeyAsync(provider.Name);
                return await provider.CheckFileHashAsync(MBkey, hash);
            }
            catch (ApiKeyMissingException)
            {
                throw;
            }
            catch (Exception ex)
            {
                return new ErrorResult(provider.Name) { Message = ex.Message };
            }
        });

        var result = await Task.WhenAll(tasks);

        return new ScanResult { Target = hash, Results = result };
    }

    public async Task<ScanResult> ScanUrlAsync(string url)
    {
        var applicableProviders = _providers.Where(p => p.SupportsUrl);
        var tasks = applicableProviders.Select(async provider =>
        {
            try
            {
                string URLhauskey = await _apiKeyProvider.GetApiKeyAsync(provider.Name);
                return await provider.CheckUrlAsync(URLhauskey, url);
            }
            catch (ApiKeyMissingException)
            {
                throw;
            }
            catch (Exception ex)
            {
                return new ErrorResult(provider.Name) { Message = ex.Message };
            }
        });

        var result = await Task.WhenAll(tasks);

        return new ScanResult { Target = url, Results = result };
    }
}