namespace UrlShortener.Core.DTOs.URLs;

public record UrlsGetRequest(string? SortDirection, int PageSize = 12, int PageNumber = 1);