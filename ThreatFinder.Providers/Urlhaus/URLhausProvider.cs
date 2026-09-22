using System;
using System.Threading.Tasks;
using System.Net.Http;
using ThreatFinder.Core;
using System.Collections.Generic;
using System.Text.Json;

namespace ThreatFinder.Providers;

/// <summary>
/// Provides access to the URLhaus API for checking URLs.
/// </summary>
public class URLhausProvider : IThreatIntelProvider
{
    /// <summary>
    /// HTTP client used to communicate with the URLhaus API.
    /// </summary>
    private readonly HttpClient _httpClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="URLhausProvider"/> class.
    /// </summary>
    /// <param name="httpClient">HTTP client used for API requests.</param>
    public URLhausProvider(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    /// <summary>
    /// Gets the name of the provider.
    /// </summary>
    public string Name => "URLhaus";

    /// <summary>
    /// Gets a value indicating that URLhaus does not support file hash checks.
    /// </summary>
    public bool SupportsHash => false;

    /// <summary>
    /// Gets a value indicating that URLhaus supports URL checks.
    /// </summary>
    public bool SupportsUrl => true;

    /// <summary>
    /// Attempts to check a file hash, which is not supported by URLhaus.
    /// </summary>
    /// <param name="authKey">Authentication key used to access the provider.</param>
    /// <param name="sha256Hash">SHA-256 hash of the file to check.</param>
    /// <returns>This method does not return a result because hash checks are not supported.</returns>
    /// <exception cref="NotSupportedException">
    /// Always thrown because this provider does not support file hash checks.
    /// </exception>
    public Task<EngineResult> CheckFileHashAsync(string authKey, string sha256Hash) => throw new NotSupportedException("This provider does not support sha256hash");

    /// <summary>
    /// Sends a request to the URLhaus API to retrieve information about a URL.
    /// </summary>
    /// <param name="authKey">Authentication key used to authenticate the API request.</param>
    /// <param name="url">URL to query.</param>
    /// <returns>The deserialized response returned by URLhaus.</returns>
    private async Task<URLhausResponse> URLhausHTTPCall(string authKey, string url)
    {
        // Builds the form data required by the URLhaus API.
        var formData = new Dictionary<string, string>
        {
            ["url"] = url
        };

        // Creates the request content using URL-encoded form data.
        var content = new FormUrlEncodedContent(formData);

        // Creates the HTTP POST request targeting the URLhaus API.
        var request = new HttpRequestMessage(HttpMethod.Post, "https://urlhaus-api.abuse.ch/v1/url/")
        {
            Content = content
        };

        // Adds the API key required to authenticate the request.
        request.Headers.Add("Auth-Key", authKey);

        // Sends the request and waits for the provider's response.
        var response = await _httpClient.SendAsync(request);

        // Validates the HTTP response and throws the appropriate provider exception if necessary.
        HttpResponseValidator.EnsureSuccess(response, Name);

        // Reads the response body as a JSON string.
        var json = await response.Content.ReadAsStringAsync();

        // Deserializes the JSON response into the corresponding response model.
        var result = JsonSerializer.Deserialize<URLhausResponse>(json);

        // Ensures that a valid response was received from URLhaus.
        if (result is null)
            throw new Exception("URLhaus returned an empty or invalid response.");

        return result;
    }

    /// <summary>
    /// Checks a URL against the URLhaus database.
    /// </summary>
    /// <param name="authkey">Authentication key used to access the provider.</param>
    /// <param name="url">URL to check.</param>
    /// <returns>An <see cref="EngineResult"/> representing the outcome of the query.</returns>
    public async Task<EngineResult> CheckUrlAsync(string authkey, string url)
    {
        // Performs the API request and retrieves the raw URLhaus response.
        URLhausResponse rawResponse = await URLhausHTTPCall(authkey, url);

        string queryStatus = rawResponse.QueryStatus;

        // Converts the provider's query status into the corresponding application result.
        return queryStatus switch
        {
            // "ok" indicates that the queried URL was found in the URLhaus database.
            "ok" => new MaliciousResult("URLhaus")
            {
                ThreatType = rawResponse.Threat ?? string.Empty,
                Timestamp = rawResponse.DateAdded ?? string.Empty,
                Tags = rawResponse.Tags is { Count: > 0 } tags
                    ? tags
                    : new List<string> { "no_tags" }
            },

            // "no_results" indicates that the URL was not found in the URLhaus database.
            "no_results" => new CleanResult("URLhaus")
            {
                Message = "The submitted URL has not been found on URLhaus database. This may indicate that the file is safe, or that it has not yet been discovered or reported."
            },

            // Handles any other status returned by the URLhaus API as an error.
            _ => new ErrorResult("URLhaus")
            {
                Message = $"Error: URLhaus endpoint returned the following error '{queryStatus}' "
            }
        };
    }
}