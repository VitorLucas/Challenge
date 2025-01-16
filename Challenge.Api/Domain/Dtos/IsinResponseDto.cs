using System.Text.Json.Serialization;

namespace Challenge.Api.Domain.Dtos;

public class IsinResponseDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("price")]
    public decimal Price { get; set; }
}
