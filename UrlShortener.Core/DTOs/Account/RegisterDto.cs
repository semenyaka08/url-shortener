using System.ComponentModel.DataAnnotations;

namespace UrlShortener.Core.DTOs.Account;

public class RegisterDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}