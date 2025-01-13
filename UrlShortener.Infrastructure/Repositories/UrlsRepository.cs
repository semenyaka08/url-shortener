using Microsoft.EntityFrameworkCore;
using UrlShortener.Core.Entities;
using UrlShortener.Core.Repositories.Interfaces;

namespace UrlShortener.Infrastructure.Repositories;

public class UrlsRepository(ApplicationDbContext context) : IUrlsRepository
{
    public async Task<string> AddUrlAsync(UrlInfo urlInfo)
    {
        await context.UrlInfos.AddAsync(urlInfo);

        await context.SaveChangesAsync();

        return urlInfo.ShortenedUrl;
    }

    public async Task<UrlInfo?> GetUrlByIdAsync(Guid id)
    {
        return await context.UrlInfos.FirstOrDefaultAsync(x=>x.Id == id);
    }

    public async Task<UrlInfo?> GetUrlByOriginalUrlAsync(string originalUrl)
    {
        return await context.UrlInfos.FirstOrDefaultAsync(x=>x.OriginalUrl == originalUrl);
    }

    public async Task<UrlInfo?> GetUrlByCodeAsync(string code)
    {
        return await context.UrlInfos.FirstOrDefaultAsync(x=>x.Code == code);
    }
    
    public async Task<bool> IsCodeAlreadyExist(string code)
    {
        return await context.UrlInfos.AnyAsync(x=>x.Code == code);
    }
}