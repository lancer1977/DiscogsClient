using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DiscogsClient.RestHelpers;

public class EnumConverter<T> : JsonConverter<T> where T : struct
{
    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return Enum.Parse<T>(reader.GetString());
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}

public class BasicTimeSpanConverter : JsonConverter<TimeSpan?>
{

    public override TimeSpan? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var result = reader.GetString();

        try
        {
            var values = result.Split(':').Select(int.Parse).ToList();
            switch (values.Count)
            {
                case 2:
                    return new TimeSpan(0, values[0], values[1]);

                case 3:
                    return new TimeSpan(values[0], values[1], values[2]);
            }

            return null;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public override void Write(Utf8JsonWriter writer, TimeSpan? value, JsonSerializerOptions options)
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




}

