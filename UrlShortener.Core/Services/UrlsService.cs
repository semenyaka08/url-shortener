using Microsoft.Extensions.Logging;
using UrlShortener.Core.DTOs.URLs;
using UrlShortener.Core.Entities;
using UrlShortener.Core.Exceptions;
using UrlShortener.Core.Mapper;
using UrlShortener.Core.Repositories.Interfaces;
using UrlShortener.Core.Services.Interfaces;

namespace UrlShortener.Core.Services;

public class UrlsService(IUrlsRepository urlsRepository, IUrlShortenerService urlShortenerService, ILogger<UrlsService> logger) : IUrlsService
{
    public async Task<string> GenerateUrlAsync(GenerateUrlRequest addRequest, string schema, string host)
    {
        logger.LogInformation("Generating a short URL for the original URL: {OriginalUrl}", addRequest.OriginalUrl);
        
        var isAlreadyExist = await urlsRepository.GetUrlByOriginalUrlAsync(addRequest.OriginalUrl);

        if (isAlreadyExist != null)
        {
            logger.LogWarning("The URL {OriginalUrl} has already been shortened.", addRequest.OriginalUrl);
            throw new UrlAlreadyShortenedException();
        }

        var code = await urlShortenerService.GenerateUniqueCode();
        logger.LogInformation("Generated unique code: {Code} for URL: {OriginalUrl}", code, addRequest.OriginalUrl);
        
        var entity = addRequest.ToEntity(code, schema, host);

        var shortenedUrl = await urlsRepository.AddUrlAsync(entity);
        logger.LogInformation("Successfully added the shortened URL to the repository: {ShortenedUrl}", shortenedUrl);

        return shortenedUrl;
    }

    public async Task<string> GetUrlByCodeAsync(string code)
    {
        logger.LogInformation("Fetching the original URL for the shortened code: {Code}", code);
        
        var urlInfo = await urlsRepository.GetUrlByCodeAsync(code);

        if (urlInfo is null)
        {
            logger.LogWarning("No URL found for the code: {Code}.", code);

            throw new NotFoundException($"Resource type: {nameof(UrlInfo)} with code: {code} does not exist");
        }

        logger.LogInformation("Successfully retrieved the original URL for code {Code}: {OriginalUrl}", code, urlInfo.OriginalUrl);
        
        return urlInfo.OriginalUrl;
    }

    public async Task<UrlGetResponse> GetUrlByIdAsync(Guid id)
    {
        var url = await urlsRepository.GetUrlByIdAsync(id);

        if (url is null)
            throw new NotFoundException($"Resource type: {nameof(UrlInfo)} with id: {id} does not exist");

        return url.ToDto();
    }

    public async Task<IEnumerable<UrlGetResponse>> GetUrlsAsync(UrlsGetRequest request)
    {
        var urls = await urlsRepository.GetUrlsAsync(request);

        return urls.Select(x => x.ToDto());
    }

    public async Task DeleteUrlAsync(Guid id)
    {
        var url = await urlsRepository.GetUrlByIdAsync(id);

        if (url is null)
            throw new NotFoundException($"Resource type: {nameof(UrlInfo)} with id: {id} does not exist");

        await urlsRepository.DeleteUrlAsync(url);
    }
}