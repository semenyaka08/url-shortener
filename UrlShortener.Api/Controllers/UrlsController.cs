using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.Api.Extensions;
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

        string? userEmail = User.GetEmail();
        
        if (userEmail == null)
            return BadRequest("Current user has no email");
        
        var shortenedUrl = await urlsService.GenerateUrlAsync(request, HttpContext.Request.Scheme, HttpContext.Request.Host.ToString(), userEmail);

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
        string? userEmail = User.GetEmail();
        bool isAdmin = User.IsInRole("Admin");
        
        if (userEmail == null)
            return BadRequest("Current user has no email");
        
        var urlInfo = await urlsService.GetUrlByIdAsync(id, userEmail, isAdmin);

        return Ok(urlInfo);
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetUrls([FromQuery] UrlsGetRequest request)
    {
        string? userEmail = User.GetEmail();
        
        if (userEmail == null)
            return BadRequest("Current user has no email");
        
        var urls = await urlsService.GetUrlsAsync(request, userEmail);

        return Ok(urls);
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUrl([FromRoute] Guid id)
    {
        string? userEmail = User.GetEmail();
        bool isAdmin = User.IsInRole("Admin");
        
        if (userEmail == null)
            return BadRequest("Current user has no email");
        
        await urlsService.DeleteUrlAsync(id, userEmail, isAdmin);

        return NoContent();
    }
}