using Microsoft.Extensions.Logging;
using UrlShortener.Core.Services.Interfaces;
using UrlShortener.Dal;
using UrlShortener.Dal.Repositories.Interfaces;

namespace UrlShortener.Core.Services;

public class UrlShortenerService(IUrlsRepository urlsRepository, ILogger<UrlShortenerService> logger) : IUrlShortenerService
{
    private readonly Random _random = new();

    public async Task<string> GenerateUniqueCode()
    {
        logger.LogInformation("Starting to generate a unique short URL code.");

        var codeChars = new char[UrlShorteningConfig.NumberOfCharsInShortenedLink];

        while (true)
        {
            for (int i = 0;i<UrlShorteningConfig.NumberOfCharsInShortenedLink;i++)
            {
                var randomIndex = _random.Next(UrlShorteningConfig.Alphabet.Length - 1);

                codeChars[i] = UrlShorteningConfig.Alphabet[randomIndex];
            }

            var code = new string(codeChars);
            
            logger.LogInformation("Generated code: {Code}", code);

            if (!await urlsRepository.IsCodeAlreadyExist(code))
            {
                logger.LogInformation("Code {Code} is unique and ready to be used.", code);
                return code;
            }
            
            logger.LogWarning("Code {Code} already exists, regenerating.", code);
        }
    }
}