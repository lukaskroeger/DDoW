using MAUIApp.Models;

namespace MAUIApp.ViewModels;
public class WikiCardViewModel
{
    private WikiArticle _article;
    public WikiCardViewModel(WikiArticle article)
    {
        _article = article;
    }
    public WikiArticle Article { get => _article; }
    public string ArticleId { get => _article.Id; }
    public string Text { get => _article.Text; }
}
