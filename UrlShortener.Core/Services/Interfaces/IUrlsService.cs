using UrlShortener.Core.DTOs.URLs;

namespace UrlShortener.Core.Services.Interfaces;

public interface IUrlsService
{
    Task<string> GenerateUrlAsync(GenerateUrlRequest addRequest, string schema, string host);

    Task<string> GetUrlByCodeAsync(string code);

    Task<UrlGetResponse> GetUrlByIdAsync(Guid id);

    Task<IEnumerable<UrlGetResponse>> GetUrlsAsync(UrlsGetRequest request);

    Task DeleteUrlAsync(Guid id);
}