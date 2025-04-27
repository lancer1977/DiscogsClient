using DiscogsClient.RestHelpers;
using System.Text.Json.Serialization;

namespace DiscogsClient.Data.Result;

public class DiscogsImage
{
    public string uri { get; set; }
    public string resource_url { get; set; }
    public string uri150 { get; set; }
    [JsonConverter(typeof(EnumConverter<DiscogsImageType>))]
    public DiscogsImageType type { get; set; }
    public int height { get; set; }
    public int width { get; set; }
}