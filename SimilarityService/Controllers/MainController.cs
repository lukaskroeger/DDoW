using Microsoft.AspNetCore.Mvc;
using SimilarityService.Models;
using SimilarityService.Services;

namespace SimilarityService.Controllers;

[ApiController]
[Route("api/")]
public class MainController : ControllerBase
{
    private readonly ILogger<LinksController> _logger;
    private readonly IWikipediaService _wikipediaService;

    public MainController(ILogger<LinksController> logger, IWikipediaService wikipediaService)
    {
        _logger = logger;
        _wikipediaService = wikipediaService;
    }

    [HttpGet("getRandom/{number}")]
    public async Task<ActionResult<IEnumerable<string>>> GetDependingOnSeeAlso(int number)
    {
        var result = _wikipediaService.GetRandomWikipages(number);
        return Ok(result);
    }
}
