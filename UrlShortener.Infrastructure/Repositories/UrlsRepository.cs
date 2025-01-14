using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Core.DTOs.Admin;
using UrlShortener.Core.DTOs.URLs;
using UrlShortener.Core.Entities;
using UrlShortener.Core.Repositories.Interfaces;

namespace UrlShortener.Infrastructure.Repositories;

public class UrlsRepository(ApplicationDbContext context) : IUrlsRepository
{
    public async Task<UrlInfo> AddUrlAsync(UrlInfo urlInfo)
    {
        await context.UrlInfos.AddAsync(urlInfo);

        await context.SaveChangesAsync();

        return urlInfo;
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

    public async Task<(IEnumerable<UrlInfo>, int)> GetUrlsAsync(UrlsGetRequest request, string userEmail)
    {
        var query = context.UrlInfos.Where(z => (request.SearchParam == null
                                                || z.Id.ToString() == request.SearchParam)
                                                && z.UserEmail == userEmail);

        int totalCount = await query.CountAsync();
        
        query = request.SortDirection == "asc" ? query.OrderBy(GetSelectorKey(request.SortBy)) : query.OrderByDescending(GetSelectorKey(request.SortBy));

        return (await query.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize).ToListAsync(), totalCount);
    }

    public async Task DeleteUrlAsync(UrlInfo urlInfo)
    {
        context.UrlInfos.Remove(urlInfo);

        await context.SaveChangesAsync();
    }

    public async Task<(IEnumerable<UrlInfo>, int)> GetAllUrlsAsync(AdminUrlsGetRequest request)
    {
        var query = context.UrlInfos.Where(z => request.SearchParam == null
                                                 || z.UserEmail.Contains(request.SearchParam)
                                                 || z.Id.ToString() == request.SearchParam);

        int totalCount = await query.CountAsync();
        
        query = request.SortDirection == "asc" ? query.OrderBy(GetSelectorKey(request.SortBy)) : query.OrderByDescending(GetSelectorKey(request.SortBy));

        return (await query.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize).ToListAsync(), totalCount);
    }

    public async Task<bool> IsCodeAlreadyExist(string code)
    {
        return await context.UrlInfos.AnyAsync(x=>x.Code == code);
    }
    
    private Expression<Func<UrlInfo, object>> GetSelectorKey(string? sortItem)
    {
        return sortItem switch
        {
            "date" => z => z.CreatedAt,
            "id" => z => z.Id,
            _ => z => z.Id
        };
    }
}