using MAUIApp.Models;
using MAUIApp.ViewModels;
using Microsoft.AspNetCore.Http.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

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
		List<string> initial = new() { "API", "Wikipedia", "2022_Russian_invasion_of_Ukraine" };
		List<WikiArticle> articles = new();
		foreach (var item in initial)
		{
			WikiArticle article = await GetArticleById(item);
			if(article is not null)
			{
				articles.Add(article);
			}
		}
		return articles;
	}

	internal async Task<List<WikiArticle>> LikeArticle(string articleId)
	{
        List<WikiArticle> articles = new();
        List<SimilarArticleResponse> result = null;
        QueryBuilder qb = new();
        var uri = new Uri($"https://localhost:7105/api/Links/seealso/{articleId}");        
        HttpResponseMessage response = await _httpClient.GetAsync(uri);
        if (response.IsSuccessStatusCode)
        {
            result = await response.Content.ReadFromJsonAsync<List<SimilarArticleResponse>>();
            foreach (var item in result)
            {
                articles.Add(await GetArticleById(item.Title));
            }
        }		
		return articles;
		//return result.Select(async x => await GetArticleById(x.Title)).Select(x => x.Result).ToList();
    }

	private async Task<WikiArticle?> GetArticleById(string id)
	{
		QueryResponse? result = null;
        QueryBuilder qb = new();
        qb.Add("action", "query");
        qb.Add("format", "json");
		qb.Add("prop", "extracts");
		qb.Add("exsentences", "10");
		qb.Add("exintro", "true");
        qb.Add("titles", id);
		qb.Add("formatversion", "2");
        var baseUri = new Uri("https://en.wikipedia.org/w/api.php");
		var uri = new Uri(baseUri, qb.ToQueryString().ToUriComponent());
        HttpResponseMessage response = await _httpClient.GetAsync(uri);
        if (response.IsSuccessStatusCode)
        {
            result = await response.Content.ReadFromJsonAsync<QueryResponse>();
        }
		if(result is null || result.Query?.Pages?.FirstOrDefault() is null)
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
