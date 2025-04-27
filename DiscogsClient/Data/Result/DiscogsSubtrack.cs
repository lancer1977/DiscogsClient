using DiscogsClient.RestHelpers;
using System;
using System.Text.Json.Serialization;

namespace DiscogsClient.Data.Result;

public class DiscogsSubtrack
{
    public string title { get; set; }
    public string type_ { get; set; }
    [JsonConverter(typeof(BasicTimeSpanConverter))]
    public TimeSpan? duration { get; set; }
    public string position { get; set; }
    public DiscogsReleaseArtist[] extraartists { get; set; }
    public DiscogsReleaseArtist[] artists { get; set; }
}