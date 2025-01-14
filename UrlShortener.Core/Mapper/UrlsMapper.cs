using UrlShortener.Core.DTOs.URLs;
using UrlShortener.Core.Entities;

namespace UrlShortener.Core.Mapper;

public static class UrlsMapper
{
    public static UrlInfo ToEntity(this GenerateUrlRequest request, string code, string schema, string host, string userEmail)
    {
        return new UrlInfo
        {
            Id = Guid.NewGuid(),
            ShortenedUrl = $"{schema}://{host}/api/url/code/{code}",
            Code = code,
            OriginalUrl = request.OriginalUrl,
            CreatedAt = DateTime.Now,
            UserEmail = userEmail
        };
    }

    public static UrlGetResponse ToDto(this UrlInfo urlInfo)
    {
        return new UrlGetResponse
        {
            Id = urlInfo.Id,
            ShortenedUrl = urlInfo.ShortenedUrl,
            Code = urlInfo.Code,
            OriginalUrl = urlInfo.OriginalUrl,
            CreatedAt = urlInfo.CreatedAt,
            UserEmail = urlInfo.UserEmail
        };
    }
}