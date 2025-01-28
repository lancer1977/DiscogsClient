using System;
using System.Text.Json.Serialization;
using DiscogsClient.RestHelpers; 

namespace DiscogsClient.Data.Result;

public class DiscogsReleaseVersion : DiscogsEntity
{
    public string catno { get; set; }
    public string country { get; set; }
    public string format { get; set; }
    public string label { get; set; }
    [JsonPropertyName("released")]
    [JsonConverter(typeof(BasicDateTimeConverter))]
    public DateTime? Released { get; set; }
    public string resource_url { get; set; }
    public string status { get; set; }
    public string thumb { get; set; }
    public string title { get; set; }
}