using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using DiscogsClient.Data.Query;
using DiscogsClient.Data.Result;
using DiscogsClient.Internal;
using DiscogsClient.RestHelpers;
using DiscogsClient.RestHelpers.OAuth1;
using RestSharp;

namespace DiscogsClient.Test;

public class DiscogsClientBehaviorTest
{
    [Fact]
    public async Task GetUserIdentityAsync_CachesIdentityAfterFirstRequest()
    {
        var fakeClient = new FakeDiscogsWebClient();
        fakeClient.Identity = new DiscogsIdentity
        {
            id = 7,
            username = "collector",
            resource_url = "https://api.discogs.com/users/collector",
            consumer_name = "test app"
        };
        var client = CreateClient(fakeClient);

        var first = await client.GetUserIdentityAsync();
        var second = await client.GetUserIdentityAsync();

        first.Should().BeSameAs(second);
        fakeClient.IdentityExecuteCount.Should().Be(1);
        fakeClient.IdentityRequestCount.Should().Be(1);
    }

    [Fact]
    public void SearchAsEnumerable_RequestsPagesUntilMaxResultsAreSatisfied()
    {
        var fakeClient = new FakeDiscogsWebClient();
        fakeClient.SearchPages.Enqueue(SearchPage(page: 1, pages: 2,
            Result(1, "first"),
            Result(2, "second")));
        fakeClient.SearchPages.Enqueue(SearchPage(page: 2, pages: 2,
            Result(3, "third"),
            Result(4, "fourth")));
        var client = CreateClient(fakeClient);

        var results = client.SearchAsEnumerable(new DiscogsSearch { query = "jazz" }, max: 3).ToList();

        results.Select(result => result.title).Should().Equal("first", "second", "third");
        fakeClient.SearchRequests.Should().HaveCount(2);
        fakeClient.SearchRequests[0].Should().Contain(parameter => QueryParameter(parameter, "page", "1"));
        fakeClient.SearchRequests[0].Should().Contain(parameter => QueryParameter(parameter, "per_page", "3"));
        fakeClient.SearchRequests[1].Should().Contain(parameter => QueryParameter(parameter, "page", "2"));
    }

    [Fact]
    public void SearchAsEnumerable_WhenClientFails_PropagatesClientError()
    {
        var fakeClient = new FakeDiscogsWebClient
        {
            SearchException = new WebClientException("rate limit path failed", new IOException("too many requests"))
        };
        var client = CreateClient(fakeClient);

        var act = () => client.SearchAsEnumerable(new DiscogsSearch { query = "jazz" }, max: 1).ToList();

        act.Should().Throw<WebClientException>()
            .WithMessage("rate limit path failed");
    }

    private static DiscogsClient CreateClient(IDiscogsWebClient fakeClient)
    {
        var client = new DiscogsClient(new TokenAuthenticationInformation("test-token"));
        var field = typeof(DiscogsClient).GetField("_client", BindingFlags.Instance | BindingFlags.NonPublic);
        field.Should().NotBeNull();
        field!.SetValue(client, fakeClient);
        return client;
    }

    private static DiscogsSearchResults SearchPage(int page, int pages, params DiscogsSearchResult[] results)
    {
        return new DiscogsSearchResults
        {
            pagination = new DiscogsPaginedResult
            {
                page = page,
                pages = pages,
                per_page = results.Length,
                items = pages * results.Length,
                urls = new DiscogsPaginedUrls()
            },
            results = results
        };
    }

    private static DiscogsSearchResult Result(int id, string title)
    {
        return new DiscogsSearchResult
        {
            id = id,
            title = title,
            type = DiscogsEntityType.release
        };
    }

    private static bool QueryParameter(Parameter parameter, string name, object value)
    {
        return parameter.Type == ParameterType.QueryString &&
               parameter.Name == name &&
               Equals(parameter.Value?.ToString(), value);
    }

    private sealed class FakeDiscogsWebClient : IDiscogsWebClient
    {
        public Queue<DiscogsSearchResults> SearchPages { get; } = new();
        public List<Parameter[]> SearchRequests { get; } = new();
        public DiscogsIdentity Identity { get; set; } = new();
        public WebClientException SearchException { get; set; }
        public int IdentityExecuteCount { get; private set; }
        public int IdentityRequestCount { get; private set; }

        public RestRequest GetSearchRequest()
        {
            return new RestRequest("database/search");
        }

        public RestRequest GetUserIdentityRequest()
        {
            IdentityRequestCount++;
            return new RestRequest("oauth/identity");
        }

        public Task<T> Execute<T>(RestRequest request, CancellationToken cancellationToken)
        {
            if (typeof(T) == typeof(DiscogsIdentity))
            {
                IdentityExecuteCount++;
                return Task.FromResult((T)(object)Identity);
            }

            if (typeof(T) == typeof(DiscogsSearchResults))
            {
                if (SearchException != null)
                {
                    throw SearchException;
                }

                SearchRequests.Add(request.Parameters.ToArray());
                return Task.FromResult((T)(object)SearchPages.Dequeue());
            }

            throw new NotSupportedException(typeof(T).FullName);
        }

        public RestRequest GetReleaseRequest(int relaseId) => throw new NotSupportedException();
        public RestRequest GetMasterRequest(int masterId) => throw new NotSupportedException();
        public RestRequest GetMasterReleaseVersionRequest(int masterId) => throw new NotSupportedException();
        public RestRequest GetArtistRequest(int artistId) => throw new NotSupportedException();
        public RestRequest GetLabelRequest(int artistId) => throw new NotSupportedException();
        public RestRequest GetArtistReleaseVersionRequest(int artistId) => throw new NotSupportedException();
        public RestRequest GetAllLabelReleasesRequest(int labelId) => throw new NotSupportedException();
        public RestRequest GetGetUserReleaseRatingRequest(string userName, int releaseId) => throw new NotSupportedException();
        public RestRequest GetPutUserReleaseRatingRequest(string username, int releaseId) => throw new NotSupportedException();
        public RestRequest GetDeleteUserReleaseRatingRequest(string userName, int releaseId) => throw new NotSupportedException();
        public RestRequest GetCommunityReleaseRatingRequest(int releaseId) => throw new NotSupportedException();
        public Task<HttpStatusCode> Execute(RestRequest request, CancellationToken cancellationToken) => throw new NotSupportedException();
        public Task Download(string url, Stream copyStream, CancellationToken cancellationToken, int timeOut = 15000) => throw new NotSupportedException();
        public Task<string> SaveFile(string url, string path, string fileName, CancellationToken cancellationToken, int timeOut = 15000) => throw new NotSupportedException();
    }
}
