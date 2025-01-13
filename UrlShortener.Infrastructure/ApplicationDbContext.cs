using Microsoft.EntityFrameworkCore;
using UrlShortener.Core.Entities;

namespace UrlShortener.Infrastructure;

public class ApplicationDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<UrlInfo> UrlInfos { get; set; }
}