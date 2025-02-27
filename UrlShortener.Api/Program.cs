using UrlShortener.Api.Extensions;
using UrlShortener.Api.Middlewares;
using UrlShortener.Core.Services.Interfaces;
using UrlShortener.Dal.Entities;
using UrlShortener.Dal.Seeders;

var builder = WebApplication.CreateBuilder(args);

builder.AddPresentation();
builder.Services.AddCore();
builder.Services.AddDal(builder.Configuration);

var app = builder.Build();

//seeding data
var scope = app.Services.CreateScope();
var seeder = scope.ServiceProvider.GetRequiredService<IDataSeeder>();
await seeder.SeedAsync();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseCors(x => x.AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials()
    .WithOrigins("http://localhost:4200", "https://localhost:4200"));

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/{code}", async (string code, IUrlsService urlsService) =>
{
    var originalUrl = await urlsService.GetUrlByCodeAsync(code);

    return Results.Redirect(originalUrl);
});

app.MapControllers();
app.MapGroup("api").MapIdentityApi<AppUser>();

app.Run();
