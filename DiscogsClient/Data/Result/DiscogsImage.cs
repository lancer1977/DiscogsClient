﻿using System.ComponentModel;
using System.Text.Json.Serialization;
using DiscogsClient.Data.Query;
using DiscogsClient.RestHelpers;

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