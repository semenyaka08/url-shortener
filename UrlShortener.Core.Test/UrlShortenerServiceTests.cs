using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using UrlShortener.Core.Services;
using UrlShortener.Core.Services.Interfaces;
using UrlShortener.Dal;

namespace UrlShortener.Core.Test;

public class UrlShortenerServiceTests
{
    private readonly IUniqueCodeCacheService _codeCacheService;
    private readonly UrlShortenerService _sut;

    public UrlShortenerServiceTests()
    {
        ILogger<UrlShortenerService> logger = Substitute.For<ILogger<UrlShortenerService>>();
        _codeCacheService = Substitute.For<IUniqueCodeCacheService>();
        _sut = new UrlShortenerService(logger, _codeCacheService);
    }

   [Fact]
    public async Task GenerateUniqueCode_WhenCodeIsUnique_ReturnsCodeAndAddsItToCache()
    {
        // Arrange
       _codeCacheService.IsCodeUnique(Arg.Any<string>()).Returns(true);

        // Act
        var result = await _sut.GenerateUniqueCode();

        // Assert
         result.Should().NotBeNullOrEmpty();

        _codeCacheService.Received(1).AddCode(Arg.Any<string>());
    }

    [Fact]
    public async Task GenerateUniqueCode_WhenCodeIsNotUnique_RegeneratesCodeAndReturnsUniqueCode()
    {
       // Arrange
        _codeCacheService.IsCodeUnique(Arg.Any<string>())
              .Returns(false, false, true);
        
         // Act
        var result = await _sut.GenerateUniqueCode();
          // Assert
        result.Should().NotBeNullOrEmpty();
         _codeCacheService.Received(1).AddCode(Arg.Any<string>());
    }

    [Fact]
    public async Task GenerateUniqueCode_GeneratesCodeWithCorrectLength()
    {
        // Arrange
        _codeCacheService.IsCodeUnique(Arg.Any<string>()).Returns(true);

        // Act
        var result = await _sut.GenerateUniqueCode();

        // Assert
        result.Should().HaveLength(UrlShorteningConfig.NumberOfCharsInShortenedLink);
    }
        
    [Fact]
    public async Task GenerateUniqueCode_GeneratesCodeWithValidChars()
    {
         // Arrange
        _codeCacheService.IsCodeUnique(Arg.Any<string>()).Returns(true);

          // Act
        var result = await _sut.GenerateUniqueCode();

        // Assert
         result.All(c => UrlShorteningConfig.Alphabet.Contains(c)).Should().BeTrue();
     }
}