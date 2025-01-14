using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.Api.Extensions;
using UrlShortener.Core.Entities;

namespace UrlShortener.Api.Controllers;

[ApiController]
[Route("api/account")]
public class AccountController(SignInManager<AppUser> signInManager) : Controller
{
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await signInManager.SignOutAsync();

        return NoContent();
    }
    
    [Authorize]
    [HttpGet("user-info")]
    public async Task<IActionResult> GetUserInfo()
    {
        var user = await signInManager.UserManager.GetUserByEmailAsync(User);

        if (user == null) return Unauthorized();

        return Ok(new
        {
            user.Email,
            Roles = User.FindFirstValue(ClaimTypes.Role)
        });
    }
}