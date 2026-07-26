using System;
using System.Threading.Tasks;
using System.Net.Http;
using ThreatFinder.Core;
using System.Collections.Generic;
using System.Text.Json;

namespace ThreatFinder.Providers;

public class URLhausProvider : IThreatIntelProvider
{
    private readonly HttpClient _httpClient;

    public URLhausProvider(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public string Name => "URLhaus";
    public bool SupportsHash => false;
    public bool SupportsUrl => true;
    public Task<EngineResult> CheckFileHashAsync(string authKey, string sha256Hash) => throw new NotSupportedException("This provider does not support sha256hash");

    private async Task<URLhausResponse> URLhausHTTPCall(string authKey, string url)
    {
        var formData = new Dictionary<string, string>
        {
            ["url"] = url
        };

        var content = new FormUrlEncodedContent(formData);
        var request = new HttpRequestMessage(HttpMethod.Post, "https://urlhaus-api.abuse.ch/v1/url/")
        {
            Content = content
        };
        request.Headers.Add("Auth-key", authKey);
        var response = await _httpClient.SendAsync(request);
        var json = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<URLhausResponse>(json);
        if (result is null)
            throw new Exception("MB returned an empty or invalid response.");
        return result;
    }

    public async Task<EngineResult> CheckUrlAsync(string authkey, string url)
    {
        URLhausResponse rawResponse = await URLhausHTTPCall(authkey, url);

        string queryStatus = rawResponse.QueryStatus;
        return queryStatus switch
        {
            "ok" => new MaliciousResult("URLhaus")
            {
                ThreatType = rawResponse.Threat ?? string.Empty ,
                Timestamp = rawResponse.DateAdded ?? string.Empty,
                Tags = rawResponse.Tags ?? Array.Empty<string>()
            },
            "no_results" => new CleanResult("URLhaus")
            {
                Message = "The submitted URL has not been found on URLhaus database, this results indicate that it is safe."
            },
            _ => new ErrorResult("URLhaus")
            {
                Message = $"Error: URLhaus endpoint returned the follwing error '{queryStatus}' "
            }
        }; 
    }
}