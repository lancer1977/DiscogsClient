using System.ComponentModel;
using System.Text.Json.Serialization;
using DiscogsClient.Data.Query;
using DiscogsClient.RestHelpers;

namespace DiscogsClient.Data.Result;

public class DiscogsSearchResult : DiscogsEntity
{
    public string[] genre { get; set; }
    public string[] style { get; set; }
    public string[] label { get; set; }
    public string[] format { get; set; }
    public string[] barcode { get; set; }
    public string? year { get; set; }
    public string title { get; set; }
    public string thumb { get; set; }
    public string country { get; set; }
    public DiscogsCommunityInfo community { get; set; }
    public string catno { get; set; }
    public string resource_url { get; set; }
    public string uri { get; set; }

    [JsonConverter(typeof(EnumConverter<DiscogsEntityType>))]
    public DiscogsEntityType type { get; set; }
}