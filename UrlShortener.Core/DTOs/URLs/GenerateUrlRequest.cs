using System.ComponentModel.DataAnnotations;

namespace UrlShortener.Core.DTOs.URLs;

public class GenerateUrlRequest
{
    [Required] public string OriginalUrl { get; set; } = string.Empty;
}