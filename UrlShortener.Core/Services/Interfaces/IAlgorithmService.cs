using UrlShortener.Core.DTOs.Algorithm;

namespace UrlShortener.Core.Services.Interfaces;

public interface IAlgorithmService
{
    Task UpdateAlgorithm(UpdateAlgorithmRequest request);

    Task<AlgorithmGetResponse> GetAlgorithm();
}