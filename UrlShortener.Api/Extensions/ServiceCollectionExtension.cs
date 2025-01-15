using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using UrlShortener.Api.Middlewares;
using UrlShortener.Core.Services;
using UrlShortener.Core.Services.Interfaces;
using UrlShortener.Dal;
using UrlShortener.Dal.Entities;
using UrlShortener.Dal.Repositories;
using UrlShortener.Dal.Repositories.Interfaces;
using UrlShortener.Dal.Seeders;

namespace UrlShortener.Api.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddPresentation(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((context, configuration) =>
        {
            configuration.ReadFrom.Configuration(context.Configuration);
        });

        builder.Services.AddScoped<ExceptionHandlingMiddleware>();
        builder.Services.AddControllers();

        builder.Services.AddAuthorization();
        builder.Services.AddIdentityApiEndpoints<AppUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();
        
        //Enabling CORS
        builder.Services.AddCors();
    }

    public static void AddCore(this IServiceCollection services)
    {
        services.AddScoped<IUrlsService, UrlsService>();
        services.AddScoped<IUrlShortenerService, UrlShortenerService>();
        services.AddScoped<IAlgorithmService, AlgorithmService>();
        services.AddSingleton<IUniqueCodeCacheService, UniqueCodeCacheService>();
    }

    public static void AddDal(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUrlsRepository, UrlsRepository>();
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });
        services.AddScoped<IDataSeeder, DataSeeder>();
        services.AddScoped<IAlgorithmRepository, AlgorithmRepository>();
    }
}