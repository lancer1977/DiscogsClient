using System;
using DiscogsClient.RestHelpers;
using RestSharp;

namespace DiscogsClient.Test;

public class WebClientExceptionTests
{
    [Fact]
    public void Constructor_WithRequestContext_PreservesTypedPayload()
    {
        var inner = new InvalidOperationException("network failed");

        var exception = new WebClientException(
            WebClientFailureKind.RequestExecutionFailed,
            "Error During Request Processing",
            "database/search",
            Method.Get,
            inner);

        exception.FailureKind.Should().Be(WebClientFailureKind.RequestExecutionFailed);
        exception.Resource.Should().Be("database/search");
        exception.Method.Should().Be(Method.Get);
        exception.InnerException.Should().BeSameAs(inner);
    }

    [Fact]
    public void Constructor_WithLegacySignature_RemainsCompatible()
    {
        var inner = new InvalidOperationException("network failed");

        var exception = new WebClientException("Error During Request Processing", inner);

        exception.FailureKind.Should().BeNull();
        exception.Resource.Should().BeNull();
        exception.Method.Should().BeNull();
        exception.InnerException.Should().BeSameAs(inner);
    }
}
