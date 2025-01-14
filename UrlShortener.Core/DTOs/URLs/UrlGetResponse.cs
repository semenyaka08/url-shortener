namespace UrlShortener.Core.DTOs.URLs;

public class UrlGetResponse
{
    public Guid Id { get; set; }

    public required string ShortenedUrl { get; set; }

    public required string Code { get; set; }
    
    public required string OriginalUrl { get; set; }

    public DateTime CreatedAt { get; set; }

    public required string UserEmail { get; set; }
}