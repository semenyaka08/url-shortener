﻿using Microsoft.EntityFrameworkCore;
using UrlShortener.Core.Entities;
using UrlShortener.Core.Services;

namespace UrlShortener.Infrastructure;

public class ApplicationDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<UrlInfo> UrlInfos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UrlInfo>(builder =>
        {
            builder.Property(x=>x.Code).HasMaxLength(UrlShortenerService.NumberOfCharsInShortenedLink);

            builder.HasIndex(x=>x.Code).IsUnique();
        });
    }
}