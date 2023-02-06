﻿using MAUIApp.Models;
using Microsoft.AspNetCore.Http.Extensions;
using System.Net.Http.Json;

namespace MAUIApp.Services;
public class DataService
{
    private HttpClient _httpClient;
    public DataService(HttpClient httpClient)
    {
        _httpClient = httpClient;

    }

    internal async Task<IEnumerable<WikiArticle>> GetInitial()
    {
        //List<string> initial = new() { "API", "Wikipedia", "2022_Russian_invasion_of_Ukraine" };
        List<WikiArticle> articles = new();
        QueryBuilder qb = new();
        string baseAddress = DeviceInfo.Current.Platform == DevicePlatform.Android ? "http://10.0.2.2:5178" : "https://localhost:7105";
        Uri uri = new($"{baseAddress}/api/getRandom/3");
        HttpResponseMessage response = await _httpClient.GetAsync(uri);
        if (response.IsSuccessStatusCode)
        {
            var initial = await response.Content.ReadFromJsonAsync<IEnumerable<string>>();
            foreach (string item in initial)
            {
                WikiArticle article = await GetArticleById(item);
                if (article is not null)
                {
                    articles.Add(article);
                }
            }
        }
        return articles;
    }

    internal async Task<List<WikiArticle>> LikeArticle(string articleId)
    {
        List<WikiArticle> articles = new();
        QueryBuilder qb = new();
        string baseAddress = DeviceInfo.Current.Platform == DevicePlatform.Android ? "http://10.0.2.2:5178" : "https://localhost:7105";
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
        Uri baseUri = new("https://en.wikipedia.org/w/api.php");
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
