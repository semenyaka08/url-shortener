using UrlShortener.Core.DTOs.Algorithm;
using UrlShortener.Dal.Entities;

namespace UrlShortener.Core.Mapper;

public static class AlgorithmMapper
{
    public static AlgorithmGetResponse ToDto(this Algorithm algorithm)
    {
        return new AlgorithmGetResponse
        {
            Title = algorithm.Title,
            Description = algorithm.Description
        };
    }
}