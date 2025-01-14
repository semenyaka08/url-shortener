using UrlShortener.Core.Common;
using UrlShortener.Core.DTOs.Admin;
using UrlShortener.Core.DTOs.URLs;

namespace UrlShortener.Core.Services.Interfaces;

public interface IUrlsService
{
    Task<string> GenerateUrlAsync(GenerateUrlRequest addRequest, string schema, string host, string userEmail);

    Task<string> GetUrlByCodeAsync(string code);

    Task<UrlGetResponse> GetUrlByIdAsync(Guid id, string userEmail, bool isAdmin);

    Task<PageResult<UrlGetResponse>> GetUrlsAsync(UrlsGetRequest request, string userEmail);

    Task DeleteUrlAsync(Guid id, string userEmail, bool isAdmin);

    Task<PageResult<UrlGetResponse>> GetAllUrlsAsync(AdminUrlsGetRequest request);
}