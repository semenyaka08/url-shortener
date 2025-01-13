using UrlShortener.Api.Extensions;
using UrlShortener.Api.Middlewares;
using UrlShortener.Core.Extensions;
using UrlShortener.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddPresentation();
builder.Services.AddCore();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
