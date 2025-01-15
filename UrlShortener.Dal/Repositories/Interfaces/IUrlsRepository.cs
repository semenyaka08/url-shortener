using UrlShortener.Dal.DTOs.Admin;
using UrlShortener.Dal.DTOs.Urls;
using UrlShortener.Dal.Entities;

namespace UrlShortener.Dal.Repositories.Interfaces;

public interface IUrlsRepository
{
    Task<UrlInfo> AddUrlAsync(UrlInfo urlInfo);

    Task<UrlInfo?> GetUrlByIdAsync(Guid id);

    Task<UrlInfo?> GetUrlByOriginalUrlAsync(string originalUrl);

    Task<bool> IsCodeAlreadyExist(string code);

    Task<UrlInfo?> GetUrlByCodeAsync(string code);

    Task<(IEnumerable<UrlInfo>, int)> GetUrlsAsync(UrlsDalGetRequest request, string userEmail);

    Task DeleteUrlAsync(UrlInfo urlInfo);

    Task<(IEnumerable<UrlInfo>, int)> GetAllUrlsAsync(AdminDalUrlsGetRequest request);
}