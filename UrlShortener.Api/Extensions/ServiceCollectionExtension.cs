using Serilog;
using UrlShortener.Api.Middlewares;

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
    }
}