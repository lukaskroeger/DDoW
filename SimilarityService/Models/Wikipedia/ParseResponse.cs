using System.Text.Json.Serialization;

namespace SimilarityService.Models.Wikipedia;

public class ParseResponse
{
    [JsonPropertyName("parse")]
    public Parse? Parse { get; set; }
}
