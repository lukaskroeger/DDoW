using System.Text.Json.Serialization;

namespace MAUIApp.Models;
internal class QueryResponse
{
    [JsonPropertyName("batchcomplete")]
    public bool Batchcomplete { get; set; }

    [JsonPropertyName("query")]
    public Query Query { get; set; }
}
public class Normalized
{
    [JsonPropertyName("fromencoded")]
    public bool Fromencoded { get; set; }

    [JsonPropertyName("from")]
    public string From { get; set; }

    [JsonPropertyName("to")]
    public string To { get; set; }
}

public class Page
{
    [JsonPropertyName("pageid")]
    public int Pageid { get; set; }

    [JsonPropertyName("ns")]
    public int Ns { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("extract")]
    public string Extract { get; set; }
}

public class Query
{
    [JsonPropertyName("pages")]
    public List<Page> Pages { get; set; }
}