namespace UrlShortener.Core.Services.Interfaces;

public interface IUrlShortenerService
{
    public Task<string> GenerateUniqueCode();
}