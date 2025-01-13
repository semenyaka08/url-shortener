using Microsoft.AspNetCore.Mvc;
using UrlShortener.Core.DTOs.URLs;
using UrlShortener.Core.Services.Interfaces;

namespace UrlShortener.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UrlsController(IUrlsService urlsService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> GenerateLink([FromBody] GenerateUrlRequest request)
    {
        if (!Uri.TryCreate(request.OriginalUrl, UriKind.Absolute, out _))
            return BadRequest("The given url is invalid");

        var shortenedUrl = await urlsService.GenerateUrlAsync(request, HttpContext.Request.Scheme, HttpContext.Request.Host.ToString());

        return Ok(shortenedUrl);
    }

    [HttpGet("{code}")]
    public async Task<IActionResult> GetLink([FromRoute] string code)
    {
        var originalUrl = await urlsService.GetUrlByCodeAsync(code);

        return Redirect(originalUrl);
    }
}