using UrlShortener.Core.DTOs.URLs;
using UrlShortener.Core.Entities;
using UrlShortener.Core.Exceptions;
using UrlShortener.Core.Mapper;
using UrlShortener.Core.Repositories.Interfaces;
using UrlShortener.Core.Services.Interfaces;

namespace UrlShortener.Core.Services;

public class UrlsService(IUrlsRepository urlsRepository, IUrlShortenerService urlShortenerService) : IUrlsService
{
    public async Task<string> GenerateUrlAsync(GenerateUrlRequest addRequest, string schema, string host)
    {
        var isAlreadyExist = await urlsRepository.GetUrlByOriginalUrlAsync(addRequest.OriginalUrl);

        if (isAlreadyExist != null)
            throw new UrlAlreadyShortenedException();

        var code = await urlShortenerService.GenerateUniqueCode();

        var entity = addRequest.ToEntity(code, schema, host);

        var shortenedUrl = await urlsRepository.AddUrlAsync(entity);

        return shortenedUrl;
    }

    public async Task<string> GetUrlByCodeAsync(string code)
    {
        var urlInfo = await urlsRepository.GetUrlByCodeAsync(code);

        if (urlInfo is null)
            throw new NotFoundException($"Resource type: {nameof(UrlInfo)} with code: {code} does not exist");

        return urlInfo.OriginalUrl;
    }
}