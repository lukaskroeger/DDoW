using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUIApp.Models;

[Table("Interactions")]
public class Interaction
{
    [PrimaryKey, AutoIncrement, NotNull]
    public int Id { get; set; }

    [NotNull]
    [System.Diagnostics.CodeAnalysis.NotNull]
    public string? ArticleId { get; set; }
        
    public DateTime DateTime { get; set; }

    public InteractionType Type { get; set; }

    public string? PredecessorArticleId { get; set; }

    public static Interaction FromWikiArticleNow(WikiArticle wikiArticle, InteractionType type)
    {
        return new()
        {
            ArticleId = wikiArticle.Id,
            DateTime = DateTime.Now,
            Type = type
        };
    }
}

public enum InteractionType
{
    Like,
    Dislike
}