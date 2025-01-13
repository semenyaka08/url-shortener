using UrlShortener.Core.DTOs.URLs;
using UrlShortener.Core.Entities;

namespace UrlShortener.Core.Mapper;

public static class UrlsMapper
{
    public static UrlInfo ToEntity(this GenerateUrlRequest request, string code, string schema, string host)
    {
        return new UrlInfo
        {
            Id = Guid.NewGuid(),
            ShortenedUrl = $"{schema}://{host}/api/{code}",
            Code = code,
            OriginalUrl = request.OriginalUrl,
            CreatedAt = DateTime.Now
        };
    }
}