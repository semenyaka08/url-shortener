using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Dal.DTOs.Admin;
using UrlShortener.Dal.DTOs.Urls;
using UrlShortener.Dal.Entities;
using UrlShortener.Dal.Repositories.Interfaces;

namespace UrlShortener.Dal.Repositories;

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

    public async Task<List<string>> GetAllCodesAsync()
    {
        return await context.UrlInfos.Select(z => z.Code).ToListAsync();
    }

    public async Task<(IEnumerable<UrlInfo>, int)> GetUrlsAsync(UrlsDalGetRequest request, string userEmail)
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

    public async Task<(IEnumerable<UrlInfo>, int)> GetAllUrlsAsync(AdminDalUrlsGetRequest request)
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