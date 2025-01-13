using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
}