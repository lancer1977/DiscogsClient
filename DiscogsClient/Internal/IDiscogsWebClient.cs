using RestSharp;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace DiscogsClient.Internal;

internal interface IDiscogsWebClient
{
    RestRequest GetSearchRequest();

    RestRequest GetReleaseRequest(int relaseId);

    RestRequest GetMasterRequest(int masterId);

    RestRequest GetMasterReleaseVersionRequest(int masterId);

    RestRequest GetArtistRequest(int artistId);

    RestRequest GetLabelRequest(int artistId);

    RestRequest GetArtistReleaseVersionRequest(int artistId);

    RestRequest GetAllLabelReleasesRequest(int labelId);

    RestRequest GetGetUserReleaseRatingRequest(string userName, int releaseId);

    RestRequest GetPutUserReleaseRatingRequest(string username, int releaseId);

    RestRequest GetDeleteUserReleaseRatingRequest(string userName, int releaseId);

    RestRequest GetCommunityReleaseRatingRequest(int releaseId);

    RestRequest GetUserIdentityRequest();

    Task<T> Execute<T>(RestRequest request, CancellationToken cancellationToken);

    Task<HttpStatusCode> Execute(RestRequest request, CancellationToken cancellationToken);

    Task Download(string url, Stream copyStream, CancellationToken cancellationToken, int timeOut = 15000);

    Task<string> SaveFile(string url, string path, string fileName, CancellationToken cancellationToken, int timeOut = 15000);
}