//using RestSharp.Serializers;

//namespace DiscogsClient.RestHelpers;

//public class NewtonsoftJsonSerializer : IDeserializer, ISerializer
//{
//    public string DateFormat { get; set; }

//    public string Namespace { get; set; }

//    public string RootElement { get; set; }

//    public string ContentType
//    {
//        get { return "application/json"; }
//        set { }
//    }

//    public T Deserialize<T>(RestRequest response)
//    {
//        return JsonConvert.DeserializeObject<T>(response.Content);
//    }

//    public string Serialize(object obj)
//    {
//        return JsonConvert.SerializeObject(obj);
//    }

//    public static NewtonsoftJsonSerializer Default => new NewtonsoftJsonSerializer();
//}