namespace UrlShortener.Core.Entities;

public class UrlInfo
{
    public Guid Id { get; set; }

    public required string ShortenedUrl { get; set; }

    public required string OriginalUrl { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}