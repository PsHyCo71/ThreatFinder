using System.Net.Http;

namespace ThreatFinder.Core;

/// <summary>
/// Provides validation methods for HTTP responses returned by external providers.
/// </summary>
public class HttpResponseValidator
{
    /// <summary>
    /// Ensures that the HTTP response indicates a successful request.
    /// </summary>
    /// <param name="response">HTTP response to validate.</param>
    /// <param name="providerName">Name of the provider that returned the response.</param>
    /// <exception cref="RateLimitExceededException">
    /// Thrown when the provider's rate limit has been exceeded (HTTP 429).
    /// </exception>
    /// <exception cref="AuthenticationException">
    /// Thrown when authentication fails or access to the requested resource is forbidden
    /// (HTTP 401 or 403).
    /// </exception>
    /// <exception cref="ProviderUnavailableException">
    /// Thrown when the provider encounters a server-side error (HTTP 5xx).
    /// </exception>
    /// <exception cref="ProviderHttpException">
    /// Thrown when the response contains an HTTP error status that is not handled explicitly.
    /// </exception>
    public static void EnsureSuccess(HttpResponseMessage response, string providerName)
    {
        // Checks whether the HTTP response has a successful status code (2xx).
        if (!response.IsSuccessStatusCode)
        {
            switch ((int)response.StatusCode)
            {
                // 429 Too Many Requests: the provider's rate limit has been exceeded.
                case 429:
                    throw new RateLimitExceededException
                    {
                        StatusCode = response.StatusCode,
                        ProviderName = providerName
                    };

                // 401 Unauthorized: authentication credentials are missing or invalid.
                // 403 Forbidden: the request is authenticated but access is not allowed.
                case 401:
                case 403:
                    throw new AuthenticationException
                    {
                        StatusCode = response.StatusCode,
                        ProviderName = providerName
                    };

                // 5xx Server Error: the provider encountered an error while processing the request.
                case >= 500:
                    throw new ProviderUnavailableException
                    {
                        StatusCode = response.StatusCode,
                        ProviderName = providerName
                    };

                // Handles all other HTTP error status codes.
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