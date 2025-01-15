using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using UrlShortener.Core.Services.Interfaces;
using UrlShortener.Dal.Repositories.Interfaces;

namespace UrlShortener.Core.Services;

public class UniqueCodeCacheService(ILogger<UniqueCodeCacheService> logger, IServiceScopeFactory scopeFactory) : IUniqueCodeCacheService
{
    private readonly HashSet<string> _cachedCodes = new();
    private bool _isInitialized = false; 
    
    public async Task InitializeCacheAsync()
    {
        if (_isInitialized)
        {
            logger.LogWarning("Cache has already been initialized.");
            return;
        }

        using var scope = scopeFactory.CreateScope();
        var urlsRepository = scope.ServiceProvider.GetRequiredService<IUrlsRepository>();
            
        logger.LogInformation("Initializing the unique code cache.");
        
        var existingCodes = await urlsRepository.GetAllCodesAsync();

        if (existingCodes.Any())
        {
            foreach(var code in existingCodes)
            {
                _cachedCodes.Add(code);
            }
            
            logger.LogInformation("Loaded {Count} codes from the database.", _cachedCodes.Count);
        }
        else
        {
            logger.LogInformation("No codes found in the database during initialization.");
        }

        _isInitialized = true;
    }

    public bool IsCodeUnique(string code)
    {
        if (!_isInitialized)
        {
            logger.LogWarning("Cache has not been initialized, please initialize cache before usage.");
            return true;
        }

        return !_cachedCodes.Contains(code);
    }

    public void AddCode(string code)
    {
        if(!_isInitialized)
        {
            logger.LogError("Cannot add code to cache, cache is not initialized.");
            return;
        }

        _cachedCodes.Add(code);
        logger.LogDebug("Added code {Code} to cache.", code);
    }
}