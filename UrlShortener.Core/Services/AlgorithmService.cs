using UrlShortener.Core.DTOs.Algorithm;
using UrlShortener.Core.Exceptions;
using UrlShortener.Core.Mapper;
using UrlShortener.Core.Repositories.Interfaces;
using UrlShortener.Core.Services.Interfaces;

namespace UrlShortener.Core.Services;

public class AlgorithmService(IAlgorithmRepository algorithmRepository) : IAlgorithmService
{
    public async Task UpdateAlgorithm(UpdateAlgorithmRequest request)
    {
        await algorithmRepository.UpdateAlgorithm(request.Title, request.Description);
    }

    public async Task<AlgorithmGetResponse> GetAlgorithm()
    {
        var algorithm = await algorithmRepository.GetAlgorithm();

        if (algorithm == null)
            throw new NotFoundException("Algorithm was not found");
        
        return algorithm.ToDto();
    }
}