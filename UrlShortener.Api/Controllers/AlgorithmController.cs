using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.Core.DTOs.Algorithm;
using UrlShortener.Core.Services.Interfaces;

namespace UrlShortener.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AlgorithmController(IAlgorithmService algorithmService) : ControllerBase
{
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> UpdateAlgorithm([FromBody] UpdateAlgorithmRequest request)
    {
        await algorithmService.UpdateAlgorithm(request);

        return Ok();
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAlgorithm()
    {
        var algorithm = await algorithmService.GetAlgorithm();

        return Ok(algorithm);
    }
}