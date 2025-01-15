namespace UrlShortener.Dal.DTOs.Admin;

public record AdminDalUrlsGetRequest(int PageSize = 12, int PageNumber = 1, string? SortBy = null, string? SortDirection = null, string? SearchParam = null);