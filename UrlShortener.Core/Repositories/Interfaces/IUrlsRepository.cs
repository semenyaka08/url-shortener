﻿using UrlShortener.Core.DTOs.URLs;
using UrlShortener.Core.Entities;

namespace UrlShortener.Core.Repositories.Interfaces;

public interface IUrlsRepository
{
    Task<string> AddUrlAsync(UrlInfo urlInfo);

    Task<UrlInfo?> GetUrlByIdAsync(Guid id);

    Task<UrlInfo?> GetUrlByOriginalUrlAsync(string originalUrl);

    Task<bool> IsCodeAlreadyExist(string code);

    Task<UrlInfo?> GetUrlByCodeAsync(string code);

    Task<IEnumerable<UrlInfo>> GetUrlsAsync(UrlsGetRequest request);

    Task DeleteUrlAsync(UrlInfo urlInfo);
}