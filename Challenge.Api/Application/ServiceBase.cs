using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Challenge.Api.Application;

public abstract class ServiceBase
{
    protected static StringContent GetContent(object dado)
    {
        return new StringContent(JsonSerializer.Serialize(dado), Encoding.UTF8, "application/json");
    }

    protected static async Task<T> DeserializarObjetoResponse<T>(HttpResponseMessage responseMessage)
    {
        try
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = false,
                Converters = { new DateTimeOffsetConverter() }
            };

            return JsonSerializer.Deserialize<T>(await responseMessage.Content.ReadAsStringAsync(), options);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error on Deserialize, JSON:{await responseMessage.Content.ReadAsStringAsync()};", ex);
        }
    }

    public static string SerializeObject<T>(T obj)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = false,
            WriteIndented = true
        };

        return JsonSerializer.Serialize(obj, options);
    }

    private class DateTimeOffsetConverter : JsonConverter<DateTimeOffset>
    {
        public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return DateTimeOffset.Parse(reader.GetString());
        }

        public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }

}
