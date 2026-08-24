using System;
using System.Net;

namespace ThreatFinder.Core;

/// <summary>
/// Represents an HTTP-related exception returned by an external provider.
/// </summary>
public class ProviderHttpException : Exception
{
    /// <summary>
    /// Gets the HTTP status code returned by the provider.
    /// </summary>
    public required HttpStatusCode StatusCode { get; init; }

    /// <summary>
    /// Gets the name of the provider that returned the error.
    /// </summary>
    public required string ProviderName { get; init; }

    /// <summary>
    /// Gets a message describing the HTTP error returned by the provider.
    /// </summary>
    public override string Message => $"{ProviderName} returned http error '{StatusCode}'";
}

/// <summary>
/// Represents an exception thrown when the provider's rate limit has been exceeded.
/// </summary>
public class RateLimitExceededException : ProviderHttpException
{
    /// <summary>
    /// Gets a message indicating that the daily query limit has been reached.
    /// </summary>
    public override string Message => base.Message + ": you have reached the daily query limit!";
}

/// <summary>
/// Represents an exception thrown when authentication with the provider fails.
/// </summary>
public class AuthenticationException : ProviderHttpException
{
    /// <summary>
    /// Gets a message indicating that the API key may be invalid.
    /// </summary>
    public override string Message => base.Message + ": please check that your API key is valid!";
}

/// <summary>
/// Represents an exception thrown when the provider is temporarily unavailable.
/// </summary>
public class ProviderUnavailableException : ProviderHttpException
{
    /// <summary>
    /// Gets a message indicating that the provider is temporarily unavailable.
    /// </summary>
    public override string Message => base.Message + $": {ProviderName} is temporarily unavailable, please try again later!";
}