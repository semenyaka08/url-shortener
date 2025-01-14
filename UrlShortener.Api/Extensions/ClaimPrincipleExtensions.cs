using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Core.Entities;

namespace UrlShortener.Api.Extensions;

public static class ClaimPrincipleExtensions
{
    public static async Task<AppUser?> GetUserByEmailAsync(this UserManager<AppUser> userManager, ClaimsPrincipal claimsPrincipal)
    {
        return await userManager.Users.FirstOrDefaultAsync(z=>z.Email == claimsPrincipal.GetEmail());
    }
    
    public static string? GetEmail(this ClaimsPrincipal claimsPrincipal)
    {
        return claimsPrincipal.FindFirstValue(ClaimTypes.Email);
    }
}