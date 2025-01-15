using Microsoft.Extensions.Logging;
using UrlShortener.Core.Common;
using UrlShortener.Core.DTOs.Admin;
using UrlShortener.Core.DTOs.URLs;
using UrlShortener.Core.Exceptions;
using UrlShortener.Core.Mapper;
using UrlShortener.Core.Services.Interfaces;
using UrlShortener.Dal.Entities;
using UrlShortener.Dal.Repositories.Interfaces;


namespace UrlShortener.Core.Services;

public class UrlsService(IUrlsRepository urlsRepository, IUrlShortenerService urlShortenerService, ILogger<UrlsService> logger) : IUrlsService
{
    public async Task<UrlGetResponse> GenerateUrlAsync(GenerateUrlRequest addRequest, string schema, string host, string userEmail)
    {
        logger.LogInformation("Generating a short URL for the original URL: {OriginalUrl}", addRequest.OriginalUrl);
        
        var isAlreadyExist = await urlsRepository.GetUrlByOriginalUrlAsync(addRequest.OriginalUrl);

        if (isAlreadyExist != null)
        {
            logger.LogWarning("The URL {OriginalUrl} has already been shortened.", addRequest.OriginalUrl);
            throw new UrlAlreadyShortenedException();
        }

        var code = urlShortenerService.GenerateUniqueCode();
        logger.LogInformation("Generated unique code: {Code} for URL: {OriginalUrl}", code, addRequest.OriginalUrl);

        var shortenedUrl = $"{schema}://{host}/api/url/code/{code}";
        
        var entity = addRequest.ToEntity(code, shortenedUrl, userEmail);

        var generatedUrl = await urlsRepository.AddUrlAsync(entity);
        logger.LogInformation("Successfully added the shortened URL to the repository: {ShortenedUrl}", shortenedUrl);

        return generatedUrl.ToDto();
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

    public async Task<UrlGetResponse> GetUrlByIdAsync(Guid id, string userEmail, bool isAdmin)
    {
        var url = await urlsRepository.GetUrlByIdAsync(id);

        if (url is null)
            throw new NotFoundException($"Resource type: {nameof(UrlInfo)} with id: {id} does not exist");

        if (!isAdmin && url.UserEmail != userEmail)
            throw new ForbiddenException("This operation is forbidden for you!");
        
        return url.ToDto();
    }
    
    public async Task<PageResult<UrlGetResponse>> GetUrlsAsync(UrlsGetRequest request, string userEmail)
    {
        var (urls, totalCount) = await urlsRepository.GetUrlsAsync(request.ToDalDto(), userEmail);

        var mappedUrls = urls.Select(x=>x.ToDto());
        
        return new PageResult<UrlGetResponse>(mappedUrls, totalCount, request.PageSize, request.PageNumber);
    }

    public async Task DeleteUrlAsync(Guid id, string userEmail, bool isAdmin)
    {
        var url = await urlsRepository.GetUrlByIdAsync(id);

        if (url is null)
            throw new NotFoundException($"Resource type: {nameof(UrlInfo)} with id: {id} does not exist");

        if (!isAdmin && url.UserEmail != userEmail)
            throw new ForbiddenException("This operation is forbidden for you!");
        
        await urlsRepository.DeleteUrlAsync(url);
    }

    public async Task<PageResult<UrlGetResponse>> GetAllUrlsAsync(AdminUrlsGetRequest request)
    {
        var (urls, totalCount) = await urlsRepository.GetAllUrlsAsync(request.ToDalDto());

        var mappedUrls = urls.Select(x=>x.ToDto());

        return new PageResult<UrlGetResponse>(mappedUrls, totalCount, request.PageSize, request.PageNumber);
    }
}