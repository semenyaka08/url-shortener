namespace UrlShortener.Core.Services.Interfaces;

public interface IUniqueCodeCacheService
{
    Task InitializeCacheAsync();
    
    bool IsCodeUnique(string code);
    
    void AddCode(string code);
}