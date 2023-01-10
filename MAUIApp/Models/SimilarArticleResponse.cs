using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MAUIApp.Models;
internal class SimilarArticleResponse
{
    [JsonPropertyName("title")]
    public string Title { get; set; }
}
