using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;
using UrlShortener.Core.Services;
using UrlShortener.Dal.Repositories.Interfaces;

namespace UrlShortener.Core.Test;

public class UniqueCodeCacheServiceTests
{
    private readonly ILogger<UniqueCodeCacheService> _logger;
    private readonly IUrlsRepository _urlsRepository;
    private readonly UniqueCodeCacheService _service;

    public UniqueCodeCacheServiceTests()
    {
        _logger = Substitute.For<ILogger<UniqueCodeCacheService>>();
        var scopeFactory = Substitute.For<IServiceScopeFactory>();
        var scope = Substitute.For<IServiceScope>();
        _urlsRepository = Substitute.For<IUrlsRepository>();

        var serviceProvider = Substitute.For<IServiceProvider>();
        serviceProvider.GetService(typeof(IUrlsRepository)).Returns(_urlsRepository);
        scope.ServiceProvider.Returns(serviceProvider);
        scopeFactory.CreateScope().Returns(scope);

        _service = new UniqueCodeCacheService(_logger, scopeFactory);
    }

    [Fact]
    public async Task InitializeCacheAsync_Should_LoadCodesIntoCache()
    {
        // Arrange
        var codes = new List<string> { "Code1", "Code2", "Code3" };
        _urlsRepository.GetAllCodesAsync().Returns(codes);

        // Act
        await _service.InitializeCacheAsync();

        // Assert
        codes.All(code => _service.IsCodeUnique(code) == false).Should().BeTrue();
    }

    [Fact]
    public async Task InitializeCacheAsync_Should_NotInitializeTwice()
    {
        // Arrange
        _urlsRepository.GetAllCodesAsync().Returns(new List<string>());

        // Act
        await _service.InitializeCacheAsync();
        await _service.InitializeCacheAsync();

        // Assert
        _logger.Received(1).LogWarning("Cache has already been initialized.");
    }

    [Fact]
    public void IsCodeUnique_Should_ReturnTrueIfCacheNotInitialized()
    {
        // Act
        var result = _service.IsCodeUnique("Code1");

        // Assert
        result.Should().BeTrue();
        _logger.Received(1).LogWarning("Cache has not been initialized, please initialize cache before usage.");
    }

    [Fact]
    public async Task IsCodeUnique_Should_ReturnFalseIfCodeExists()
    {
        // Arrange
        var codes = new List<string> { "Code1" };
        _urlsRepository.GetAllCodesAsync().Returns(codes);
        await _service.InitializeCacheAsync();

        // Act
        var result = _service.IsCodeUnique("Code1");

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void AddCode_Should_LogErrorIfCacheNotInitialized()
    {
        // Act
        _service.AddCode("Code1");

        // Assert
        _logger.Received(1).LogError("Cannot add code to cache, cache is not initialized.");
    }
}