using SQLite;

namespace MAUIApp.Models;

[Table("CardStack")]
public class WikiArticle
{
    
    [PrimaryKey]
    [NotNull]
    [System.Diagnostics.CodeAnalysis.NotNull]
    public string? Id { get; set; }
    public string? Text { get; set; }
    public string? Title { get; set; }
    public string? PredecessorArticleId { get; set; }
}
