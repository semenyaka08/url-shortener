using Microsoft.Extensions.Options;
using UrlShortener.Api.Extensions;
using UrlShortener.Api.Middlewares;
using UrlShortener.Core.Entities;
using UrlShortener.Core.Extensions;
using UrlShortener.Infrastructure.Extensions;
using UrlShortener.Infrastructure.Seeders;

var builder = WebApplication.CreateBuilder(args);

builder.AddPresentation();
builder.Services.AddCore();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

//seeding data
var scope = app.Services.CreateScope();
var seeder = scope.ServiceProvider.GetRequiredService<IDataSeeder>();
await seeder.SeedAsync();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapGroup("api").MapIdentityApi<AppUser>();

app.Run();
