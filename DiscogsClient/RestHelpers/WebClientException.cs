using System;

namespace DiscogsClient.RestHelpers;

public class WebClientException : Exception
{
    public WebClientException(string message, Exception innerException) : base(message, innerException)
    {
    }
}