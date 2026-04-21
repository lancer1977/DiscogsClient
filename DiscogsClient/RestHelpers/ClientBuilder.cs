using System;
using RestSharp;

namespace DiscogsClient.RestHelpers
{
    public class ClientBuilder
    {
        static ClientBuilder()
        {
            Build = url => new RestClient(url);
        }

        public static Func<string, IRestClient> Build { get; set; }
    }
}