using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.Core.DTOs.Admin;
using UrlShortener.Core.Services.Interfaces;
using UrlShortener.Dal;

namespace UrlShortener.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AdminController(IUrlsService urlsService) : ControllerBase
{
    [HttpGet("urls")]
    [Authorize(Roles = ApplicationRoles.Admin)]
    public async Task<IActionResult> GetAllUrls([FromQuery] AdminUrlsGetRequest request)
    {
        var urls = await urlsService.GetAllUrlsAsync(request);

        return Ok(urls);
    }
}