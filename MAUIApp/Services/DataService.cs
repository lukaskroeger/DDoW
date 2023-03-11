using MAUIApp.Models;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;

namespace MAUIApp.Services;
public class DataService
{
    private HttpClient _httpClient;
    private IConfiguration _config;
    private SettingsService _settings;

    public DataService(HttpClient httpClient, IConfiguration config, SettingsService settings)
    {
        _httpClient = httpClient;
        _config= config;
        _settings= settings;
    }

    internal async Task<IEnumerable<WikiArticle>> GetRandom(int amount)
    {
        RandomResponse? result = null;
        QueryBuilder qb = new();
        qb.Add("action", "query");
        qb.Add("format", "json");
        qb.Add("list", "random");
        qb.Add("rnnamespace", "0");
        qb.Add("rnlimit", $"{amount}");
        Uri baseUri = new($"https://{_settings.LanguageKey}.{_config.GetRequiredSection("Wikipedia")["ApiUrl"]}");
        Uri uri = new(baseUri, qb.ToQueryString().ToUriComponent());
        HttpResponseMessage response = await _httpClient.GetAsync(uri);
        if (response.IsSuccessStatusCode)
        {
            result = await response.Content.ReadFromJsonAsync<RandomResponse>();
        }
        var requests = result?.Query.Random.Select(x => GetArticleById(x.Title)).ToList();
        return await Task.WhenAll(requests);
    }

    internal async Task<List<WikiArticle>> LikeArticle(string articleId)
    {
        List<WikiArticle> articles = new();
        QueryBuilder qb = new();
        string baseAddress = _settings.SimilarityServiceUri;
        Uri uri = new($"{baseAddress}/api/Links/seealso/{articleId}");
        HttpResponseMessage response = await _httpClient.GetAsync(uri);
        if (response.IsSuccessStatusCode)
        {
            List<SimilarArticleResponse> result = await response.Content.ReadFromJsonAsync<List<SimilarArticleResponse>>();
            foreach (SimilarArticleResponse item in result)
            {
                articles.Add(await GetArticleById(item.Title));
            }
        }
        return articles;
    }

    private async Task<WikiArticle?> GetArticleById(string id)
    {
        QueryResponse? result = null;
        QueryBuilder qb = new();
        qb.Add("action", "query");
        qb.Add("format", "json");
        qb.Add("prop", "extracts");
        //qb.Add("explaintext", "true");
        qb.Add("exintro", "true");
        qb.Add("titles", id);
        qb.Add("formatversion", "2");
        Uri baseUri = new($"https://{_settings.LanguageKey}.{_config.GetRequiredSection("Wikipedia")["ApiUrl"]}");
        Uri uri = new(baseUri, qb.ToQueryString().ToUriComponent());
        HttpResponseMessage response = await _httpClient.GetAsync(uri);
        if (response.IsSuccessStatusCode)
        {
            result = await response.Content.ReadFromJsonAsync<QueryResponse>();
        }
        if (result is null || result.Query?.Pages?.FirstOrDefault() is null)
        {
            return null;
        }
        WikiArticle article = new()
        {
            Id = id,
            Title = result.Query.Pages.First().Title,
            Text = result.Query.Pages.First().Extract,
        };
        return article;
    }
}
