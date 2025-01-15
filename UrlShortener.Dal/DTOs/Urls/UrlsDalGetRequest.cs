namespace UrlShortener.Dal.DTOs.Urls;

public record UrlsDalGetRequest(int PageSize = 12, int PageNumber = 1, string? SortBy = null, string? SortDirection = null, string? SearchParam = null);