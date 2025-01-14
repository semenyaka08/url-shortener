namespace UrlShortener.Core.DTOs.Admin;

public record AdminUrlsGetRequest(int PageSize = 12, int PageNumber = 1, string? SortBy = null, string? SortDirection = null, string? SearchParam = null);