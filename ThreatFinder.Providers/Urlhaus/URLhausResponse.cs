using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace ThreatFinder.Providers;
public record URLhausResponse(
    [property: JsonPropertyName("query_status")] string QueryStatus,
    [property: JsonPropertyName("id")] string? Id,
    [property: JsonPropertyName("url")] string? Url,
    [property: JsonPropertyName("url_status")] string? UrlStatus,
    [property: JsonPropertyName("date_added")] string? DateAdded,
    [property: JsonPropertyName("threat")] string? Threat,
    [property: JsonPropertyName("tags")] IReadOnlyList<string>? Tags,
    [property: JsonPropertyName("reporter")] string? Reporter
);