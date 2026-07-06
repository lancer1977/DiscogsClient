using System;
using RestSharp;

namespace DiscogsClient.RestHelpers;

/// <summary>
/// Represents a Discogs HTTP client failure.
/// </summary>
public class WebClientException : Exception
{
    /// <summary>
    /// Creates a Discogs web-client exception with structured request context.
    /// </summary>
    public WebClientException(
        WebClientFailureKind failureKind,
        string message,
        string resource,
        Method method,
        Exception innerException) : base(message, innerException)
    {
        FailureKind = failureKind;
        Resource = resource;
        Method = method;
    }

    /// <summary>
    /// Creates a Discogs web-client exception with the legacy message/inner-exception contract.
    /// </summary>
    public WebClientException(string message, Exception innerException) : base(message, innerException)
    {
    }

    /// <summary>
    /// Gets the structured failure kind when the exception was created with request context.
    /// </summary>
    public WebClientFailureKind? FailureKind { get; }

    /// <summary>
    /// Gets the request resource associated with the failure.
    /// </summary>
    public string Resource { get; }

    /// <summary>
    /// Gets the HTTP method associated with the failure.
    /// </summary>
    public Method? Method { get; }
}

/// <summary>
/// Identifies the kind of Discogs web-client failure.
/// </summary>
public enum WebClientFailureKind
{
    /// <summary>
    /// The request failed while RestSharp was executing it.
    /// </summary>
    RequestExecutionFailed = 1
}
