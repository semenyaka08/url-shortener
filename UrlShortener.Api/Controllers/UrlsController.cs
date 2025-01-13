using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.Core.DTOs.URLs;
using UrlShortener.Core.Services.Interfaces;

namespace UrlShortener.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UrlController(IUrlsService urlsService) : ControllerBase
{
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateShortenedUrl([FromBody] GenerateUrlRequest request)
    {
        if (!Uri.TryCreate(request.OriginalUrl, UriKind.Absolute, out _))
            return BadRequest("The given url is invalid");

        var shortenedUrl = await urlsService.GenerateUrlAsync(request, HttpContext.Request.Scheme, HttpContext.Request.Host.ToString());

        return Ok(shortenedUrl);
    }
    
    [HttpGet("code/{code}")]
    public async Task<IActionResult> RedirectToOriginalUrl([FromRoute] string code)
    {
        var originalUrl = await urlsService.GetUrlByCodeAsync(code);

        return Redirect(originalUrl);
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUrlById ([FromRoute] Guid id)
    {
        var urlInfo = await urlsService.GetUrlByIdAsync(id);

        return Ok(urlInfo);
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetUrls([FromQuery] UrlsGetRequest request)
    {
        var urls = await urlsService.GetUrlsAsync(request);

        return Ok(urls);
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUrl([FromRoute] Guid id)
    {
        await urlsService.DeleteUrlAsync(id);

        return NoContent();
    }
}