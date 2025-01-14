using UrlShortener.Core.Entities;

namespace UrlShortener.Core.Repositories.Interfaces;

public interface IAlgorithmRepository
{
    Task UpdateAlgorithm(string title, string description);

    Task<Algorithm?> GetAlgorithm();
}