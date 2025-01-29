using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using RateLimiter;
using RestSharp;

namespace DiscogsClient.RestHelpers;

public abstract class RestSharpWebClient
{
    private const string _ErrorMessage = "Error During Request Processing";
    protected abstract string UrlBase { get; }
    protected abstract string UserAgentFallBack { get; }
    protected abstract TimeLimiter TimeLimiter { get; }

    private IRestClient _Client;
    protected IRestClient Client
    {
        get { return _Client ?? (_Client = GetClient(UrlBase, _TimeOut)); }
    }

    private string _UserAgent;
    public string UserAgent
    {
        get { return _UserAgent ?? UserAgentFallBack; }
        set { _UserAgent = value; }
    }

    private int _TimeOut;
    protected RestSharpWebClient(string userAgent=null, int timeOut = 10000) 
    {
        _TimeOut = timeOut;
        UserAgent = userAgent;
    }

    protected IRestClient GetClient(string urlBase, int timeOut = 10000)
    {
        var client = new RestClient(options: new RestClientOptions(){UserAgent = UserAgent, Timeout = TimeSpan.FromSeconds(10), BaseUrl = new Uri(urlBase)});
        
        return Mature(client);
    }

    protected virtual IRestClient Mature(IRestClient client)
    {
        return client;
    }

    public async Task<T> Execute<T>(RestRequest request, CancellationToken cancellationToken)
    {
        try
        {
            request = Mature(request);
            var response = await GetResponse<T>(request, cancellationToken);
            return response.Data;
        }
        catch(Exception)
        {
            return default(T);
        }    
    }

    private async Task<RestResponse<T>> GetResponse<T>(RestRequest request, CancellationToken cancellationToken, IRestClient client = null)
    {
        client = client ?? Client;
        var response = await TimeLimiter.Enqueue(async () => await ExecuteBasic<T>(client, request, cancellationToken), cancellationToken);

        if (response.ErrorException != null)
            throw new WebClientException(_ErrorMessage, response.ErrorException);

        CheckCallResult(response.StatusCode, client, request);

        return response;
    }

    protected virtual void CheckCallResult(HttpStatusCode code, IRestClient client, RestRequest request)
    {
    }

    private static Task<RestResponse<T>> ExecuteBasic<T>(IRestClient client, RestRequest request, CancellationToken cancellationToken)
    {
        return client.ExecuteAsync<T>(request, cancellationToken);
    }

    protected virtual RestRequest Mature(RestRequest request)
    {
        return request;
    }

    public async Task<HttpStatusCode> Execute(RestRequest request, CancellationToken cancellationToken)
    {
        var response = await GetResponse(request, cancellationToken);
        return response.StatusCode;
    }

    public async Task<string> SaveFile(string url, string path, string fileName, CancellationToken cancellationToken, int timeOut = 15000)
    {
        var extension = Path.GetExtension(url);
        var fullPath = Path.Combine(path, fileName + extension);
        using (var writer = File.Create(fullPath))
        {
            await Download(url, writer, cancellationToken, timeOut);
        }
        return fullPath;
    }

    public async Task Download(string url, Stream copyStream, CancellationToken cancellationToken, int timeOut = 15000)
    {
        var client = GetClient(url, timeOut);
        var request = new RestRequest()
        {
            Method = Method.Get,
            ResponseWriter = ResponseWriter
        };

        Stream ResponseWriter(Stream arg)
        {
             arg.CopyTo(copyStream);
             return copyStream;
        };
         
        await GetResponse(request, cancellationToken, client);
    }

    private async Task<RestResponse> GetResponse(RestRequest request, CancellationToken cancellationToken, IRestClient client = null)
    {
        client = client ?? Client;
        var response = await TimeLimiter.Enqueue(async () => await ExecuteBasic(client, request, cancellationToken), cancellationToken);

        if (response.ErrorException != null)
            throw new WebClientException(_ErrorMessage, response.ErrorException);

        return response;
    }

    private static Task<RestResponse> ExecuteBasic(IRestClient client, RestRequest request, CancellationToken cancellationToken)
    {
        return client.ExecuteAsync(request, cancellationToken);
    }
}