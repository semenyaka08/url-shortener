using UrlShortener.Core.DTOs.Admin;
using UrlShortener.Core.DTOs.URLs;
using UrlShortener.Core.Entities;

namespace UrlShortener.Core.Repositories.Interfaces;

public interface IUrlsRepository
{
    Task<UrlInfo> AddUrlAsync(UrlInfo urlInfo);

    Task<UrlInfo?> GetUrlByIdAsync(Guid id);

    Task<UrlInfo?> GetUrlByOriginalUrlAsync(string originalUrl);

    Task<bool> IsCodeAlreadyExist(string code);

    Task<UrlInfo?> GetUrlByCodeAsync(string code);

    Task<(IEnumerable<UrlInfo>, int)> GetUrlsAsync(UrlsGetRequest request, string userEmail);

    Task DeleteUrlAsync(UrlInfo urlInfo);

    Task<(IEnumerable<UrlInfo>, int)> GetAllUrlsAsync(AdminUrlsGetRequest request);
}