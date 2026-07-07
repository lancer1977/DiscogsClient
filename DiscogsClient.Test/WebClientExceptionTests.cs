using System;
using System.IO;
using DiscogsClient.RestHelpers;

namespace DiscogsClient.Test;

public class WebClientExceptionTests
{
    [Fact]
    public void LegacyConstructor_PreservesBaseExceptionCompatibility()
    {
        var inner = new IOException("network failed");

        var exception = new WebClientException("request failed", inner);

        exception.Should().BeAssignableTo<Exception>();
        exception.Message.Should().Be("request failed");
        exception.InnerException.Should().BeSameAs(inner);
        exception.FailureKind.Should().BeNull();
    }

    [Fact]
    public void TypedConstructor_CarriesStableRequestContext()
    {
        var inner = new IOException("network failed");

        var exception = new WebClientException(
            "request failed",
            inner,
            WebClientFailureKind.RequestExecutionFailed,
            "database/search",
            "Get");

        exception.FailureKind.Should().Be(WebClientFailureKind.RequestExecutionFailed);
        exception.Resource.Should().Be("database/search");
        exception.Method.Should().Be("Get");
        exception.InnerException.Should().BeSameAs(inner);
    }
}
