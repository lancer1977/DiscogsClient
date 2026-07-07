using System;

namespace DiscogsClient.RestHelpers
{
    /// <summary>
    /// Classifies failures produced by the Discogs web client request layer.
    /// </summary>
    public enum WebClientFailureKind
    {
        /// <summary>
        /// RestSharp reported an exception while executing the request.
        /// </summary>
        RequestExecutionFailed = 1
    }

    /// <summary>
    /// Represents a Discogs web client request failure with optional typed request context.
    /// </summary>
    public class WebClientException : Exception
    {
        /// <summary>
        /// Creates a legacy web client exception.
        /// </summary>
        public WebClientException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Creates a web client exception with stable request context.
        /// </summary>
        public WebClientException(
            string message,
            Exception innerException,
            WebClientFailureKind failureKind,
            string resource,
            string method) : base(message, innerException)
        {
            FailureKind = failureKind;
            Resource = resource;
            Method = method;
        }

        /// <summary>
        /// Gets the stable failure classification when provided by the request layer.
        /// </summary>
        public WebClientFailureKind? FailureKind { get; }

        /// <summary>
        /// Gets the RestSharp resource associated with the failed request.
        /// </summary>
        public string Resource { get; }

        /// <summary>
        /// Gets the HTTP method associated with the failed request.
        /// </summary>
        public string Method { get; }
    }
}
