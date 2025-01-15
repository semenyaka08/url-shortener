using UrlShortener.Dal.Entities;

namespace UrlShortener.Dal.Repositories.Interfaces;

public interface IAlgorithmRepository
{
    Task UpdateAlgorithm(string title, string description);

    Task<Algorithm?> GetAlgorithm();
}