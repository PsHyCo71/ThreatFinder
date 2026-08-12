using System.Net.Http;

namespace ThreatFinder.Core;

public class HttpResponseValidator
{
    public static void EnsureSuccess(HttpResponseMessage response, string providerName)
    {
        if (!response.IsSuccessStatusCode)
        {
            switch ((int)response.StatusCode)
            {
                case 429:
                    throw new RateLimitExceededException
                    {
                        StatusCode = response.StatusCode,
                        ProviderName = providerName
                    };
                case 401:
                case 403:
                    throw new AuthenticationException
                    {
                        StatusCode = response.StatusCode,
                        ProviderName = providerName
                    };
                case >= 500:
                    throw new ProviderUnavailableException
                    {
                        StatusCode = response.StatusCode,
                        ProviderName = providerName
                    };
                default:
                    throw new ProviderHttpException
                    {
                        StatusCode = response.StatusCode,
                        ProviderName = providerName
                    };
            }
        }
    }
}