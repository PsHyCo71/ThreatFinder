using System;
using System.Net;

namespace ThreatFinder.Core;

public class ProviderHttpException : Exception
{
    public required HttpStatusCode StatusCode { get; init; }
    public required string ProviderName { get; init; }
    public override string Message => $"{ProviderName} returned http error '{StatusCode}'";
}

public class RateLimitExceededException : ProviderHttpException
{
    public override string Message => base.Message + ": you have reached the daily query limit!";
}
public class AuthenticationException : ProviderHttpException
{
    public override string Message => base.Message + ": please check that your API key is valid!";
}
public class ProviderUnavailableException : ProviderHttpException
{
    public override string Message => base.Message + $": {ProviderName} is temporarily unavailable, please try again later!";
}