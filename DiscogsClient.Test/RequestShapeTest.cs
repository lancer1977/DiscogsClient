using System.Linq;
using System.Reflection;
using DiscogsClient.Data.Query;
using DiscogsClient.Internal;
using DiscogsClient.RestHelpers;
using RateLimiter;
using RestSharp;

namespace DiscogsClient.Test;

public class RequestShapeTest
{
    [Fact]
    public void Token_client_adds_discogs_token_authorization_header()
    {
        var client = new DiscogsWebClient(new TokenAuthenticationInformation("test-token"), "DiscogsClient.Test");

        var request = client.GetSearchRequest();

        request.Resource.Should().Be("database/search");
        request.Parameters.Should().Contain(parameter =>
            parameter.Type == ParameterType.HttpHeader &&
            parameter.Name == "Authorization" &&
            parameter.Value!.ToString() == "Discogs token=test-token");
    }

    [Fact]
    public void Search_parameters_use_discogs_query_names()
    {
        var request = new RestRequest("database/search");

        request.AddAsParameter(new DiscogsSearch
        {
            query = "Ornette Coleman",
            release_title = "The Shape Of Jazz To Come",
            type = DiscogsEntityType.master,
            year = 1959
        });

        request.Parameters.Should().Contain(parameter => HasParameter(parameter, "q", "Ornette Coleman"));
        request.Parameters.Should().Contain(parameter => HasParameter(parameter, "release_title", "The Shape Of Jazz To Come"));
        request.Parameters.Should().Contain(parameter => HasParameter(parameter, "type", "master"));
        request.Parameters.Should().Contain(parameter => HasParameter(parameter, "year", "1959"));
    }

    [Fact]
    public void Release_requests_target_release_resource_with_url_segment()
    {
        var client = new DiscogsWebClient(new TokenAuthenticationInformation("test-token"), "DiscogsClient.Test");

        var request = client.GetReleaseRequest(1704673);

        request.Resource.Should().Be("releases/{releaseId}");
        request.Parameters.Should().Contain(parameter =>
            parameter.Type == ParameterType.UrlSegment &&
            parameter.Name == "releaseId" &&
            parameter.Value!.ToString() == "1704673");
    }

    [Fact]
    public void Identity_request_targets_oauth_identity_resource()
    {
        var client = new DiscogsWebClient(new TokenAuthenticationInformation("test-token"), "DiscogsClient.Test");

        var request = client.GetUserIdentityRequest();

        request.Resource.Should().Be("oauth/identity");
        request.Parameters.Should().Contain(parameter =>
            parameter.Type == ParameterType.HttpHeader &&
            parameter.Name == "Accept-Encoding" &&
            parameter.Value!.ToString() == "gzip");
    }

    [Fact]
    public void User_release_rating_requests_use_username_and_release_segments()
    {
        var client = new DiscogsWebClient(new TokenAuthenticationInformation("test-token"), "DiscogsClient.Test");

        var request = client.GetPutUserReleaseRatingRequest("collector", 488973);

        request.Resource.Should().Be("releases/{releaseId}/rating/{userName}");
        request.Method.Should().Be(Method.Put);
        request.Parameters.Should().Contain(parameter =>
            parameter.Type == ParameterType.UrlSegment &&
            parameter.Name == "userName" &&
            parameter.Value!.ToString() == "collector");
        request.Parameters.Should().Contain(parameter =>
            parameter.Type == ParameterType.UrlSegment &&
            parameter.Name == "releaseId" &&
            parameter.Value!.ToString() == "488973");
    }

    [Fact]
    public void Paginable_parameters_use_discogs_page_names()
    {
        var request = new RestRequest("database/search");

        request.AddAsParameter(new DiscogsPaginable
        {
            page = 3,
            per_page = 25
        });

        request.Parameters.Should().Contain(parameter => HasParameter(parameter, "page", "3"));
        request.Parameters.Should().Contain(parameter => HasParameter(parameter, "per_page", "25"));
    }

    [Fact]
    public void Discogs_web_clients_share_the_same_rate_limiter()
    {
        var first = new DiscogsWebClient(new TokenAuthenticationInformation("first-token"), "DiscogsClient.Test");
        var second = new DiscogsWebClient(new TokenAuthenticationInformation("second-token"), "DiscogsClient.Test");

        GetLimiter(first).Should().BeSameAs(GetLimiter(second));
    }

    private static bool HasParameter(Parameter parameter, string name, object value)
    {
        return parameter.Type == ParameterType.QueryString &&
               parameter.Name == name &&
               Equals(parameter.Value, value);
    }

    private static TimeLimiter GetLimiter(DiscogsWebClient client)
    {
        var property = typeof(DiscogsWebClient).GetProperty("TimeLimiter", BindingFlags.Instance | BindingFlags.NonPublic);
        property.Should().NotBeNull();
        return (TimeLimiter)property!.GetValue(client)!;
    }
}
