using System;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace DiscogsClient.RestHelpers;
public class BasicDateTimeConverter : JsonConverter<DateTime?>
{
    public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String  )
        {
            try
            {
                var value = reader.GetString();
                var values = value.Split('-').Select(int.Parse).ToList();

                switch (values.Count)
                {
                    case 1:
                        return new DateTime(values[0], 1, 1);

                    case 2:
                        return new DateTime(values[0], Normalize(values[1]), 1);

                    case 3:
                        return new DateTime(values[0], Normalize(values[1]), Normalize(values[2]));
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            } 
        }
        return null;
    }

    public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
    {
        if (value.HasValue)
        {
            writer.WriteStringValue(value.Value.ToString("yyyy-MM-dd"));
        }
        else
        {
            writer.WriteNullValue();
        }
    } 
 
    private int Normalize(int value)
    {
        return (value <= 0) ? 1 : value;
    }

 
}
  