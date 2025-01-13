using Microsoft.AspNetCore.Identity;
using Serilog;
using UrlShortener.Api.Middlewares;
using UrlShortener.Core.Entities;
using UrlShortener.Infrastructure;

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
    }
}