using MAUIApp.Models;
using MAUIApp.Utils;

namespace MAUIApp.ViewModels;
public class WikiCardViewModel
{
    private WikiArticle _article;
    public WikiCardViewModel(WikiArticle article)
    {
        _article = article;
        Text = TextUtils.GetSupportedHTML(_article.Text);
    }
    public WikiArticle Article { get => _article; }
    public string ArticleId { get => _article.Id; }
    public string Text { get; }
    public string? PredecessorArticleText { get => $"Source: {_article.PredecessorArticleId}"; }
}
