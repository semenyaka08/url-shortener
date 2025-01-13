using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using UrlShortener.Core.Entities;

namespace UrlShortener.Infrastructure.Seeders;

public class DataSeeder(ApplicationDbContext context, UserManager<AppUser> userManager, IConfiguration configuration, RoleManager<IdentityRole> roleManager) : IDataSeeder
{
    public async Task SeedAsync()
    {
        if ((await context.Database.GetPendingMigrationsAsync()).Any())
        {
            await context.Database.MigrateAsync();
        }

        if (await context.Database.CanConnectAsync())
        {
            string[] roles = ["Admin", "User"];

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
            
            if (!userManager.Users.Any(z => z.Email == configuration["AdminData:Email"]))
            {
                var (adminUser, password) = GetAdminUser();
                await userManager.CreateAsync(adminUser, password);
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }

            var email = configuration["AdminData:Email"];
        }
    }
    
    private (AppUser, string) GetAdminUser()
    {
        var adminUser = new AppUser
        {
            UserName = configuration["AdminData:Email"],
            Email = configuration["AdminData:Email"],
        };
        
        string password = configuration["AdminData:Password"]!;
        
        return (adminUser, password);
    }
}