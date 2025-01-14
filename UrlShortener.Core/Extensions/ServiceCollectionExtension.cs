using Microsoft.Extensions.DependencyInjection;
using UrlShortener.Core.Services;
using UrlShortener.Core.Services.Interfaces;

namespace UrlShortener.Core.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddCore(this IServiceCollection services)
    {
        services.AddScoped<IUrlsService, UrlsService>();
        services.AddScoped<IUrlShortenerService, UrlShortenerService>();
        services.AddScoped<IAlgorithmService, AlgorithmService>();
    }
}