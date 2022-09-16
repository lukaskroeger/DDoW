namespace SimilarityService.Services;

public interface IWikipediaService
{
    public Task<IEnumerable<string>> GetSeeAlsoLinks(string pageTilte);
}
