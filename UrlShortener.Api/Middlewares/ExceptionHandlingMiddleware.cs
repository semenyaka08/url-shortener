using UrlShortener.Core.Exceptions;

namespace UrlShortener.Api.Middlewares;

public class ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next.Invoke(context);
        }
        catch (NotFoundException exception)
        {
            context.Response.StatusCode = 404;

            await context.Response.WriteAsync(exception.Message);
            logger.LogWarning(exception.Message);
        }
        catch (UrlAlreadyShortenedException exception)
        {
            context.Response.StatusCode = 400;

            await context.Response.WriteAsync(exception.Message);
            logger.LogWarning(exception.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);

            context.Response.StatusCode = 500;
            await context.Response.WriteAsync("Something went wrong");
        }
    }
}