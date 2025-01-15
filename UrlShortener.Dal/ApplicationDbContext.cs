using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Dal.Entities;

namespace UrlShortener.Dal;

public class ApplicationDbContext(DbContextOptions options) : IdentityDbContext<AppUser>(options)
{
    public DbSet<UrlInfo> UrlInfos { get; set; }

    public DbSet<AppUser> AppUsers { get; set; }

    public DbSet<Algorithm> Algorithms { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UrlInfo>(builder =>
        {
            builder.Property(x=>x.Code).HasMaxLength(UrlShorteningConfig.NumberOfCharsInShortenedLink);

            builder.HasIndex(x=>x.Code).IsUnique();
        });
        
        base.OnModelCreating(modelBuilder);
    }
}