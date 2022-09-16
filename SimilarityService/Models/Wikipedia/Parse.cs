using System.Text.Json.Serialization;

namespace SimilarityService.Models.Wikipedia;



public class Parse
{
    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("pageid")]
    public int PageId { get; set; }

    [JsonPropertyName("revid")]
    public int RevisionId { get; set; }

    [JsonPropertyName("sections")]
    public IList<Section>? Sections { get; set; }

    [JsonPropertyName("links")]
    public IList<WikiLink>? Links { get; set; }
}