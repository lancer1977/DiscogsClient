﻿using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace DiscogsClient.RestHelpers.OAuth1;

public abstract class OAuthAuthentifierClient : IOAuthAuthentifierClient
{
    private const string _RequestTokenUrl = "oauth/request_token";
    private const string _AccessTokenUrl = "oauth/access_token";
    private const string _AuthorizeUrl = "oauth/authorize";
    private const string _Token = "oauth_token";
    private const string _TokenSecret = "oauth_token_secret";

    private readonly OAuthConsumerInformation _ConsumerInformation;
    private OAuthTokenInformation _TokenInformation;
    private OAuthCompleteInformation _CompleteInformation;

    protected abstract string RequestUrl { get; }
    protected abstract string AuthorizeUrl { get; }

    private bool TokenIsPartialOrValid => ((_TokenInformation != null) && (_TokenInformation.PartialOrValid));
    private bool TokenIsValid => (_TokenInformation != null) && (_TokenInformation.Valid);

    protected OAuthAuthentifierClient(OAuthConsumerInformation consumerInformation)
    {
        _ConsumerInformation = consumerInformation;
    }

    public async Task<OAuthCompleteInformation> Authorize(Func<string, Task<string>> extractVerifier)
    {
        var res = await RequestToken();
        if ((res == null) || (!TokenIsPartialOrValid))
            return null;

        var url = GetAuthorizeUrl();
        var verifier = await extractVerifier(url);

        return _CompleteInformation = await Access(verifier);
    }

    private async Task<OAuthCompleteInformation> RequestToken()
    {
        if (_CompleteInformation != null)
            return _CompleteInformation;

        if (_TokenInformation?.PartialOrValid == true)
            return null;

        var requestTokenClient = GetRequestTokenClient(RequestUrl);
        _TokenInformation = await GetTokenInformationFromRequest(requestTokenClient, _RequestTokenUrl);

        if (!TokenIsValid)
            return null;

        return _CompleteInformation = new OAuthCompleteInformation(_ConsumerInformation, _TokenInformation);
    }

    private string GetAuthorizeUrl()
    {
        var authorizeTokenClient = GetRequestTokenClient(AuthorizeUrl);
        var request = new RestRequest(_AuthorizeUrl);
        request.AddParameter(_Token, _TokenInformation.Token);
        return authorizeTokenClient.BuildUri(request).ToString();
    }

    private async Task<OAuthCompleteInformation> Access(string verifier)
    {
        if (verifier == null)
            return null;

        var accessTokenClient = GetAccessTokenClient(RequestUrl, verifier);
        var tokenInformation = await GetTokenInformationFromRequest(accessTokenClient, _AccessTokenUrl);

        if ((tokenInformation == null) || (!tokenInformation.Valid))
            return null;

        return new OAuthCompleteInformation(_ConsumerInformation, tokenInformation);
    }

    public async Task<OAuthTokenInformation> GetTokenInformationFromRequest(IRestClient client, string relativeUrl)
    {
        var request = new RestRequest(relativeUrl, Method.Post);
        var response = await client.ExecuteAsync(request);

        return !CheckResponse(response) ? null : GetTokenInformationFromBodyResponse(response);
    }

    private OAuthTokenInformation GetTokenInformationFromBodyResponse(RestResponse response)
    {
        var qs = HttpUtility.ParseQueryString(response.Content);
        return new OAuthTokenInformation(qs[_Token], qs[_TokenSecret]);
    }

    private bool CheckResponse(RestResponse response)
    {
        return ((response != null) && (response.StatusCode == HttpStatusCode.OK));
    }

    private IRestClient GetRequestTokenClient(string baseUrl)
    {
        IAuthenticator auth = _ConsumerInformation.GetAuthenticatorForRequestToken();
        return new RestClient(baseUrl, configureRestClient: headers => { headers.Authenticator = auth; });
    }

    private IRestClient GetAccessTokenClient(string baseUrl, string verifier)
    {
        IAuthenticator auth = _CompleteInformation.GetAuthenticatorForAccessToken(verifier);
        var client = new RestClient(baseUrl, configureRestClient: headers => { headers.Authenticator = auth; });
        return client;
    }
}