using Microsoft.AspNetCore.Mvc;
using SimilarityService.Models;

namespace SimilarityService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LinksController : ControllerBase
{
    private readonly ILogger<LinksController> _logger;

    public LinksController(ILogger<LinksController> logger)
    {
        _logger = logger;
    }

    [HttpGet("seealso/{pageTitle}")]
    public ActionResult<IEnumerable<SimilarArticle>> GetDependingOnSeeAlso(string pageTitle)
    {
        return Problem(statusCode: 501);
    }
}
