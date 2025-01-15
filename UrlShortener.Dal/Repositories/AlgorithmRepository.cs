using Microsoft.EntityFrameworkCore;
using UrlShortener.Dal.Entities;
using UrlShortener.Dal.Repositories.Interfaces;

namespace UrlShortener.Dal.Repositories;

public class AlgorithmRepository(ApplicationDbContext context) : IAlgorithmRepository
{
    public async Task UpdateAlgorithm(string title, string description)
    {
        var algo = await context.Algorithms.FirstOrDefaultAsync();

        if (algo is null)
        {
            algo = new Algorithm
            {
                Title = title,
                Description = description
            };
            await context.Algorithms.AddAsync(algo);
            await context.SaveChangesAsync();
            return;
        }

        algo.Description = description;
        algo.Title = title;
        await context.SaveChangesAsync();
    }

    public async Task<Algorithm?> GetAlgorithm()
    {
        return await context.Algorithms.FirstOrDefaultAsync();
    }
}