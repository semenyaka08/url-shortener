namespace UrlShortener.Dal.Entities;

public class UrlInfo
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public required string ShortenedUrl { get; set; }

    public required string Code { get; set; }
    
    public required string OriginalUrl { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public required string UserEmail { get; set; }
}