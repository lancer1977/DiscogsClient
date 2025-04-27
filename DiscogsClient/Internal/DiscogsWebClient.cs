using DiscogsClient.RestHelpers;
using DiscogsClient.RestHelpers.OAuth1;
using RateLimiter;
using RestSharp;
using System;

namespace DiscogsClient.Internal;

internal class DiscogsWebClient : RestSharpWebClient, IDiscogsWebClient
{
    private const string _SearchUrl = "database/search";
    private const string _ReleaseUrl = "releases/{releaseId}";
    private const string _MasterUrl = "masters/{masterId}";
    private const string _MasterReleaseVersionUrl = "masters/{masterId}/versions";
    private const string _ArtistUrl = "artists/{artistId}";
    private const string _ArtistReleaseUrl = "artists/{artistId}/releases";
    private const string _AllLabelReleasesUrl = "labels/{labelId}/releases";
    private const string _LabeltUrl = "labels/{labelId}";
    private const string _ReleaseRatingByUserUrl = "releases/{releaseId}/rating/{userName}";
    private const string _CommunityReleaseRatingUrl = "releases/{releaseId}/rating";
    private const string _IdendityUrl = "oauth/identity";
    private readonly OAuthCompleteInformation _OAuthCompleteInformation;
    private readonly TokenAuthenticationInformation _TokenAuthenticationInformation;

    protected override string UrlBase => "https://api.discogs.com";
    protected override string UserAgentFallBack => @"DiscogsClient https://github.com/David-Desmaisons/DiscogsClient";
    protected override TimeLimiter TimeLimiter => SharedTimeLimiter;

    private static TimeLimiter SharedTimeLimiter { get; }

    public DiscogsWebClient(OAuthCompleteInformation oAuthCompleteInformation, string userAgent, int timeOut = 10000) :
        base(userAgent, timeOut)
    {
        _OAuthCompleteInformation = oAuthCompleteInformation;
    }

    public DiscogsWebClient(TokenAuthenticationInformation tokenAuthenticationInformation, string userAgent, int timeOut = 10000)
        : base(userAgent, timeOut)
    {
        _TokenAuthenticationInformation = tokenAuthenticationInformation;
    }

    static DiscogsWebClient()
    {
        SharedTimeLimiter = TimeLimiter.GetFromMaxCountByInterval(60, TimeSpan.FromMinutes(1));
    }

    protected override IRestClient Mature(IRestClient client)
    {
        var auth = _OAuthCompleteInformation?.GetAuthenticatorForProtectedResource();
        return new RestClient(configureRestClient: options => options.Authenticator = auth);
    }

    public RestRequest GetUserIdentityRequest()
    {
        return GetRequest(_IdendityUrl);
    }

    public RestRequest GetSearchRequest()
    {
        return GetRequest(_SearchUrl);
    }

    public RestRequest GetReleaseRequest(int releaseId)
    {
        return GetRequest(_ReleaseUrl).AddUrlSegment(nameof(releaseId), releaseId.ToString());
    }

    public RestRequest GetMasterRequest(int masterId)
    {
        return GetRequest(_MasterUrl).AddUrlSegment(nameof(masterId), masterId.ToString());
    }

    public RestRequest GetMasterReleaseVersionRequest(int masterId)
    {
        return GetRequest(_MasterReleaseVersionUrl).AddUrlSegment(nameof(masterId), masterId.ToString());
    }

    public RestRequest GetArtistRequest(int artistId)
    {
        return GetRequest(_ArtistUrl).AddUrlSegment(nameof(artistId), artistId.ToString());
    }

    public RestRequest GetLabelRequest(int labelId)
    {
        return GetRequest(_LabeltUrl).AddUrlSegment(nameof(labelId), labelId.ToString());
    }

    public RestRequest GetArtistReleaseVersionRequest(int artistId)
    {
        return GetRequest(_ArtistReleaseUrl).AddUrlSegment(nameof(artistId), artistId.ToString());
    }

    public RestRequest GetAllLabelReleasesRequest(int labelId)
    {
        return GetRequest(_AllLabelReleasesUrl).AddUrlSegment(nameof(labelId), labelId.ToString());
    }

    public RestRequest GetGetUserReleaseRatingRequest(string userName, int releaseId)
    {
        return GetUserReleaseRatingRequestRaw(userName, releaseId, Method.Get);
    }

    public RestRequest GetPutUserReleaseRatingRequest(string userName, int releaseId)
    {
        return GetUserReleaseRatingRequestRaw(userName, releaseId, Method.Put);
    }

    public RestRequest GetDeleteUserReleaseRatingRequest(string userName, int releaseId)
    {
        return GetUserReleaseRatingRequestRaw(userName, releaseId, Method.Delete);
    }

    private RestRequest GetUserReleaseRatingRequestRaw(string userName, int releaseId, Method method)
    {
        return GetRequest(_ReleaseRatingByUserUrl, method)
            .AddUrlSegment(nameof(userName), userName)
            .AddUrlSegment(nameof(releaseId), releaseId.ToString());
    }

    public RestRequest GetCommunityReleaseRatingRequest(int releaseId)
    {
        return GetRequest(_CommunityReleaseRatingUrl).AddUrlSegment(nameof(releaseId), releaseId.ToString());
    }

    private RestRequest GetRequest(string url, Method method = Method.Get)
    {
        var request = new RestRequest(url, method).AddHeader("Accept-Encoding", "gzip");
        if (_TokenAuthenticationInformation != null)
            request.AddHeader("Authorization", _TokenAuthenticationInformation.GetDiscogsSecretToken());

        return request;
    }
}