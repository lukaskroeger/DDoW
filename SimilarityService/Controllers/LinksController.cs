using Microsoft.AspNetCore.Mvc;
using SimilarityService.Models;
using SimilarityService.Services;

namespace SimilarityService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LinksController : ControllerBase
{
    private readonly ILogger<LinksController> _logger;
    private readonly IWikipediaService _wikipediaService;

    public LinksController(ILogger<LinksController> logger, IWikipediaService wikipediaService)
    {
        _logger = logger;
        _wikipediaService = wikipediaService;
    }

    [HttpGet("seealso/{pageTitle}")]
    public async Task<ActionResult<IEnumerable<RelatedArticle>>> GetDependingOnSeeAlso(string pageTitle)
    {
        IEnumerable<RelatedArticle> result = (await _wikipediaService.GetSeeAlsoLinks(pageTitle)).Select(x => new RelatedArticle() { Title = x });
        return Ok(result);
    }
}
