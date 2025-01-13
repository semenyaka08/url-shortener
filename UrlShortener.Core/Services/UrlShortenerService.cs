using UrlShortener.Core.Repositories.Interfaces;
using UrlShortener.Core.Services.Interfaces;

namespace UrlShortener.Core.Services;

public class UrlShortenerService(IUrlsRepository urlsRepository) : IUrlShortenerService
{
    public const int NumberOfCharsInShortenedLink = 7;
    private const string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

    private readonly Random _random = new();

    public async Task<string> GenerateUniqueCode()
    {
        var codeChars = new char[NumberOfCharsInShortenedLink];

        while (true)
        {
            for (int i = 0;i<NumberOfCharsInShortenedLink;i++)
            {
                var randomIndex = _random.Next(Alphabet.Length - 1);

                codeChars[i] = Alphabet[randomIndex];
            }

            var code = new string(codeChars);
            if (!await urlsRepository.IsCodeAlreadyExist(code))
                return code;
        }
    }
}