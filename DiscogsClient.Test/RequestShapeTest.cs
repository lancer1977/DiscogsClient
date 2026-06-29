using System.Linq;
using DiscogsClient.Data.Query;
using DiscogsClient.Internal;
using DiscogsClient.RestHelpers;
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

    private static bool HasParameter(Parameter parameter, string name, object value)
    {
        return parameter.Type == ParameterType.QueryString &&
               parameter.Name == name &&
               Equals(parameter.Value, value);
    }
}
