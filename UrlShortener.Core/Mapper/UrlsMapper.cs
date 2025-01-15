using UrlShortener.Core.DTOs.Admin;
using UrlShortener.Core.DTOs.URLs;
using UrlShortener.Dal.DTOs.Admin;
using UrlShortener.Dal.DTOs.Urls;
using UrlShortener.Dal.Entities;

namespace UrlShortener.Core.Mapper;

public static class UrlsMapper
{
    public static UrlInfo ToEntity(this GenerateUrlRequest request, string code, string shortenedUrl, string userEmail)
    {
        return new UrlInfo
        {
            ShortenedUrl = shortenedUrl,
            Code = code,
            OriginalUrl = request.OriginalUrl,
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

    public static UrlsDalGetRequest ToDalDto(this UrlsGetRequest request)
    {
        return new UrlsDalGetRequest
        {
            PageSize = request.PageSize,
            PageNumber = request.PageNumber,
            SortBy = request.SortBy,
            SortDirection = request.SortDirection,
            SearchParam = request.SearchParam
        };
    }
    
    public static AdminDalUrlsGetRequest ToDalDto(this AdminUrlsGetRequest request)
    {
        return new AdminDalUrlsGetRequest
        {
            PageSize = request.PageSize,
            PageNumber = request.PageNumber,
            SortBy = request.SortBy,
            SortDirection = request.SortDirection,
            SearchParam = request.SearchParam
        };
    }
}