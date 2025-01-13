using Microsoft.Extensions.Logging;
using UrlShortener.Core.Repositories.Interfaces;
using UrlShortener.Core.Services.Interfaces;

namespace UrlShortener.Core.Services;

public class UrlShortenerService(IUrlsRepository urlsRepository, ILogger<UrlShortenerService> logger) : IUrlShortenerService
{
    public const int NumberOfCharsInShortenedLink = 7;
    private const string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

    private readonly Random _random = new();

    public async Task<string> GenerateUniqueCode()
    {
        logger.LogInformation("Starting to generate a unique short URL code.");

        var codeChars = new char[NumberOfCharsInShortenedLink];

        while (true)
        {
            for (int i = 0;i<NumberOfCharsInShortenedLink;i++)
            {
                var randomIndex = _random.Next(Alphabet.Length - 1);

                codeChars[i] = Alphabet[randomIndex];
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