using System.Text.Json.Serialization;

namespace MAUIApp.Models;
internal class SimilarArticleResponse
{
    [JsonPropertyName("title")]
    public string Title { get; set; }
}
