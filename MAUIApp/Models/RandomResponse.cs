using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MAUIApp.Models;

internal class RandomResponse
{
    [JsonPropertyName("batchcomplete")]
    public string Batchcomplete { get; set; }

    [JsonPropertyName("query")]
    public RandomQuery Query { get; set; }
}

internal class RandomQuery
{
    [JsonPropertyName("random")]
    public List<Random> Random { get; set; }
}

internal class Random
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }
}

