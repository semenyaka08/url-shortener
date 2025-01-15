using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using UrlShortener.Core.DTOs.Admin;
using UrlShortener.Core.DTOs.URLs;
using UrlShortener.Core.Exceptions;
using UrlShortener.Core.Mapper;
using UrlShortener.Core.Services;
using UrlShortener.Core.Services.Interfaces;
using UrlShortener.Dal.Entities;
using UrlShortener.Dal.Repositories.Interfaces;

namespace UrlShortener.Core.Test;

public class UrlsServiceTests
{
    private readonly IUrlsRepository _urlsRepository;
    private readonly IUrlShortenerService _urlShortenerService;
    private readonly ILogger<UrlsService> _logger;
    private readonly UrlsService _sut;

    public UrlsServiceTests()
    {
        _urlsRepository = Substitute.For<IUrlsRepository>();
        _urlShortenerService = Substitute.For<IUrlShortenerService>();
        _logger = Substitute.For<ILogger<UrlsService>>();
        _sut = new UrlsService(_urlsRepository, _urlShortenerService, _logger);
    }

    [Fact]
    public async Task GenerateUrlAsync_WhenUrlIsNew_ReturnsShortenedUrl()
    {
        // Arrange
        var addRequest = new GenerateUrlRequest { OriginalUrl = "https://example.com" };
        var code = "shortCode";
        var userEmail = "test@test.com";
        var schema = "https";
        var host = "localhost";
        addRequest.ToEntity(code, $"{schema}://{host}/api/url/code/{code}", userEmail);

        var urlInfo = new UrlInfo
        {
            ShortenedUrl = $"{schema}://{host}/api/url/code/{code}",
            Code = code,
            OriginalUrl = addRequest.OriginalUrl,
            UserEmail = userEmail
        };

        _urlsRepository.GetUrlByOriginalUrlAsync(addRequest.OriginalUrl).Returns(Task.FromResult<UrlInfo?>(null));
        _urlShortenerService.GenerateUniqueCode().Returns(code);
        _urlsRepository.AddUrlAsync(Arg.Any<UrlInfo>()).Returns(Task.FromResult(urlInfo));

        // Act
        var result = await _sut.GenerateUrlAsync(addRequest, schema, host, userEmail);

        // Assert
        result.Should().NotBeNull();
        result.ShortenedUrl.Should().Be(urlInfo.ShortenedUrl);
        result.OriginalUrl.Should().Be(urlInfo.OriginalUrl);

        await _urlsRepository.Received(1).AddUrlAsync(Arg.Is<UrlInfo>(x =>
            x.OriginalUrl == addRequest.OriginalUrl && x.ShortenedUrl == $"{schema}://{host}/api/url/code/{code}" &&
            x.Code == code));
    }

    [Fact]
    public async Task GenerateUrlAsync_WhenUrlIsAlreadyShortened_ThrowsUrlAlreadyShortenedException()
    {
        // Arrange
        var addRequest = new GenerateUrlRequest { OriginalUrl = "https://example.com" };
        var userEmail = "test@test.com";
        var code = "shortCode";
        var schema = "https";
        var host = "localhost";

        var urlInfo = new UrlInfo
        {
            ShortenedUrl = $"{schema}://{host}/api/url/code/{code}",
            Code = code,
            OriginalUrl = addRequest.OriginalUrl,
            UserEmail = userEmail
        };
        _urlsRepository.GetUrlByOriginalUrlAsync(addRequest.OriginalUrl).Returns(Task.FromResult<UrlInfo?>(urlInfo));

        // Act
        Func<Task> act = async () => await _sut.GenerateUrlAsync(addRequest, "https", "localhost", userEmail);

        // Assert
        await Assert.ThrowsAsync<UrlAlreadyShortenedException>(act);
    }

    [Fact]
    public async Task GetUrlByCodeAsync_WhenCodeExists_ReturnsOriginalUrl()
    {
        // Arrange
        var addRequest = new GenerateUrlRequest { OriginalUrl = "https://example.com" };
        var originalUrl = "https://example.com";
        var userEmail = "test@test.com";
        var code = "shortCode";
        var schema = "https";
        var host = "localhost";

        var urlInfo = new UrlInfo
        {
            ShortenedUrl = $"{schema}://{host}/api/url/code/{code}",
            Code = code,
            OriginalUrl = addRequest.OriginalUrl,
            UserEmail = userEmail
        };

        _urlsRepository.GetUrlByCodeAsync(code).Returns(Task.FromResult<UrlInfo?>(urlInfo));

        // Act
        var result = await _sut.GetUrlByCodeAsync(code);

        // Assert
        result.Should().Be(originalUrl);
    }

    [Fact]
    public async Task GetUrlByCodeAsync_WhenCodeDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        var code = "nonExistentCode";
        _urlsRepository.GetUrlByCodeAsync(code).Returns(Task.FromResult<UrlInfo?>(null));

        // Act
        Func<Task> act = async () => await _sut.GetUrlByCodeAsync(code);

        // Assert
        await Assert.ThrowsAsync<NotFoundException>(act);
    }

    [Fact]
    public async Task GetUrlByIdAsync_WhenUrlExistsAndUserIsAdmin_ReturnsUrl()
    {
        // Arrange
        var addRequest = new GenerateUrlRequest { OriginalUrl = "https://example.com" };
        var originalUrl = "https://example.com";
        var userEmail = "test@test.com";
        var code = "shortCode";
        var schema = "https";
        var host = "localhost";
        var id = Guid.NewGuid();

        var urlInfo = new UrlInfo
        {
            Id = id,
            ShortenedUrl = $"{schema}://{host}/api/url/code/{code}",
            Code = code,
            OriginalUrl = addRequest.OriginalUrl,
            UserEmail = userEmail
        };

        _urlsRepository.GetUrlByIdAsync(id).Returns(Task.FromResult<UrlInfo?>(urlInfo));

        // Act
        var result = await _sut.GetUrlByIdAsync(id, userEmail, true);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(id);
        await _urlsRepository.Received(1).GetUrlByIdAsync(id);
    }

    [Fact]
    public async Task GetUrlByIdAsync_WhenUrlExistsAndUserIsNotAdminButHasRights_ReturnsUrl()
    {
        // Arrange
        var addRequest = new GenerateUrlRequest { OriginalUrl = "https://example.com" };
        var originalUrl = "https://example.com";
        var userEmail = "test@test.com";
        var code = "shortCode";
        var schema = "https";
        var host = "localhost";
        var id = Guid.NewGuid();

        var urlInfo = new UrlInfo
        {
            Id = id,
            ShortenedUrl = $"{schema}://{host}/api/url/code/{code}",
            Code = code,
            OriginalUrl = addRequest.OriginalUrl,
            UserEmail = userEmail
        };
        _urlsRepository.GetUrlByIdAsync(id).Returns(Task.FromResult<UrlInfo?>(urlInfo));

        // Act
        var result = await _sut.GetUrlByIdAsync(id, userEmail, false);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(id);
        await _urlsRepository.Received(1).GetUrlByIdAsync(id);
    }

    [Fact]
    public async Task GetUrlByIdAsync_WhenUrlDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        var id = Guid.NewGuid();
        var userEmail = "test@test.com";
        _urlsRepository.GetUrlByIdAsync(id).Returns(Task.FromResult<UrlInfo?>(null));
    
        // Act
        Func<Task> act = async () => await _sut.GetUrlByIdAsync(id, userEmail, true);
    
        // Assert
        await Assert.ThrowsAsync<NotFoundException>(act);
    }
    
    
    [Fact]
    public async Task DeleteUrlAsync_WhenUrlExistsAndUserIsAdmin_DeletesUrl()
    {
        // Arrange
        var addRequest = new GenerateUrlRequest { OriginalUrl = "https://example.com" };
        var originalUrl = "https://example.com";
        var userEmail = "test@test.com";
        var code = "shortCode";
        var schema = "https";
        var host = "localhost";
        var id = Guid.NewGuid();

        var urlInfo = new UrlInfo
        {
            Id = id,
            ShortenedUrl = $"{schema}://{host}/api/url/code/{code}",
            Code = code,
            OriginalUrl = addRequest.OriginalUrl,
            UserEmail = userEmail
        };
        _urlsRepository.GetUrlByIdAsync(id).Returns(Task.FromResult<UrlInfo?>(urlInfo));
    
        // Act
        await _sut.DeleteUrlAsync(id, userEmail, true);
    
        // Assert
        await _urlsRepository.Received(1).DeleteUrlAsync(urlInfo);
    }
    
    [Fact]
    public async Task DeleteUrlAsync_WhenUrlExistsAndUserIsNotAdminButHasRights_DeletesUrl()
    {
        // Arrange
        var addRequest = new GenerateUrlRequest { OriginalUrl = "https://example.com" };
        var originalUrl = "https://example.com";
        var userEmail = "test@test.com";
        var code = "shortCode";
        var schema = "https";
        var host = "localhost";
        var id = Guid.NewGuid();

        var urlInfo = new UrlInfo
        {
            Id = id,
            ShortenedUrl = $"{schema}://{host}/api/url/code/{code}",
            Code = code,
            OriginalUrl = addRequest.OriginalUrl,
            UserEmail = userEmail
        };
        _urlsRepository.GetUrlByIdAsync(id).Returns(Task.FromResult<UrlInfo?>(urlInfo));
    
        // Act
        await _sut.DeleteUrlAsync(id, userEmail, false);
    
        // Assert
        await _urlsRepository.Received(1).DeleteUrlAsync(urlInfo);
    }
    
    [Fact]
    public async Task DeleteUrlAsync_WhenUrlDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        var id = Guid.NewGuid();
        var userEmail = "test@test.com";
        _urlsRepository.GetUrlByIdAsync(id).Returns(Task.FromResult<UrlInfo?>(null));
    
        // Act
        Func<Task> act = async () => await _sut.DeleteUrlAsync(id, userEmail, true);
    
        // Assert
        await Assert.ThrowsAsync<NotFoundException>(act);
    }
}