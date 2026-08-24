using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace ThreatFinder.Providers;

/// <summary>
/// Represents the response returned by the URLhaus API.
/// </summary>
/// <param name="QueryStatus">Status of the query returned by URLhaus.</param>
/// <param name="Id">Unique identifier assigned to the URL by URLhaus, if available.</param>
/// <param name="Url">URL associated with the response, if available.</param>
/// <param name="UrlStatus">Current status of the URL according to URLhaus, if available.</param>
/// <param name="DateAdded">Date and time when the URL was added to the URLhaus database, if available.</param>
/// <param name="Threat">Threat category associated with the URL, if available.</param>
/// <param name="Tags">Tags associated with the URL, if available.</param>
/// <param name="Reporter">Name or identifier of the entity that reported the URL, if available.</param>
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