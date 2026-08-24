using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ThreatFinder.Core;

/// <summary>
/// Coordinates scans across the available threat information providers.
/// </summary>
public class ScanManager
{
    /// <summary>
    /// Gets the collection of registered threat information providers.
    /// </summary>
    private readonly IEnumerable<IThreatIntelProvider> _providers;

    /// <summary>
    /// Provides access to the API keys required by the providers.
    /// </summary>
    private readonly IApiKeyProvider _apiKeyProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="ScanManager"/> class.
    /// </summary>
    /// <param name="providers">Collection of available threat information providers.</param>
    /// <param name="apiKeyProvider">Service used to retrieve provider API keys.</param>
    public ScanManager(IEnumerable<IThreatIntelProvider> providers, IApiKeyProvider apiKeyProvider)
    {
        _providers = providers;
        _apiKeyProvider = apiKeyProvider;
    }

    /// <summary>
    /// Scans a file hash using all providers that support hash checks.
    /// </summary>
    /// <param name="hash">Hash of the file to scan.</param>
    /// <returns>A <see cref="ScanResult"/> containing the results returned by each applicable provider.</returns>
    public async Task<ScanResult> ScanHashAsync(string hash)
    {
        // Selects only providers that support file hash checks.
        var applicableProviders = _providers.Where(p => p.SupportsHash);

        // Creates a task for each applicable provider so that all requests can run concurrently.
        var tasks = applicableProviders.Select(async provider =>
        {
            try
            {
                // Retrieves the API key associated with the current provider.
                string MBkey = await _apiKeyProvider.GetApiKeyAsync(provider.Name);

                // Sends the hash to the provider for analysis.
                return await provider.CheckFileHashAsync(MBkey, hash);
            }
            catch (ApiKeyMissingException)
            {
                // Propagates missing API key errors so they can be handled by the caller.
                throw;
            }
            catch (AuthenticationException)
            {
                // Propagates authentication errors so they can be handled by the caller.
                throw;
            }
            catch (UnauthorizedAccessException)
            {
                // Propagates file or resource access errors to the caller.
                throw;
            }
            catch (IOException)
            {
                // Propagates input/output errors to the caller.
                throw;
            }
            catch (Exception ex)
            {
                // Converts unexpected provider errors into an error result
                // without interrupting the remaining provider requests.
                return new ErrorResult(provider.Name) { Message = ex.Message };
            }
        });

        // Waits for all provider requests to complete.
        var result = await Task.WhenAll(tasks);

        // Combines the target hash and provider results into a single scan result.
        return new ScanResult { Target = hash, Results = result };
    }

    /// <summary>
    /// Scans a URL using all providers that support URL checks.
    /// </summary>
    /// <param name="url">URL to scan.</param>
    /// <returns>A <see cref="ScanResult"/> containing the results returned by each applicable provider.</returns>
    public async Task<ScanResult> ScanUrlAsync(string url)
    {
        // Selects only providers that support URL checks.
        var applicableProviders = _providers.Where(p => p.SupportsUrl);

        // Creates a task for each applicable provider so that all requests can run concurrently.
        var tasks = applicableProviders.Select(async provider =>
        {
            try
            {
                // Retrieves the API key associated with the current provider.
                string URLhauskey = await _apiKeyProvider.GetApiKeyAsync(provider.Name);

                // Sends the URL to the provider for analysis.
                return await provider.CheckUrlAsync(URLhauskey, url);
            }
            catch (ApiKeyMissingException)
            {
                // Propagates missing API key errors so they can be handled by the caller.
                throw;
            }
            catch (AuthenticationException)
            {
                // Propagates authentication errors so they can be handled by the caller.
                throw;
            }
            catch (Exception ex)
            {
                // Converts unexpected provider errors into an error result
                // without interrupting the remaining provider requests.
                return new ErrorResult(provider.Name) { Message = ex.Message };
            }
        });

        // Waits for all provider requests to complete.
        var result = await Task.WhenAll(tasks);

        // Combines the target URL and provider results into a single scan result.
        return new ScanResult { Target = url, Results = result };
    }
}