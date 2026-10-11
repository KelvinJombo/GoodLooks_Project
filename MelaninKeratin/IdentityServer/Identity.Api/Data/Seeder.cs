using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Mappers;
using Identity.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Identity.Api.Data;

public static class Seeder
{
    public static async Task SeedAllAsync(IServiceProvider serviceProvider)
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        var services = scope.ServiceProvider;

        // 1. Apply migrations to all three databases/contexts.
        await services.GetRequiredService<AppDbContext>()
            .Database.MigrateAsync();

        await services.GetRequiredService<ConfigurationDbContext>()
            .Database.MigrateAsync();

        await services.GetRequiredService<PersistedGrantDbContext>()
            .Database.MigrateAsync();

        // 2. Seed ASP.NET Identity.
        var roleManager =
            services.GetRequiredService<RoleManager<IdentityRole>>();

        var userManager =
            services.GetRequiredService<UserManager<User>>();

        await SeedUsersAndRolesAsync(roleManager, userManager);

        // 3. Seed Duende configuration.
        var configContext =
            services.GetRequiredService<ConfigurationDbContext>();

        var db = services.GetRequiredService<AppDbContext>();
        var connection = db.Database.GetDbConnection();

        Console.WriteLine("=== IDENTITY SEEDER STARTED ===");
        Console.WriteLine($"Database: {connection.Database}");
        Console.WriteLine($"Data source: {connection.DataSource}");
        Console.WriteLine($"Connection state: {connection.State}");

        await SeedDuendeConfigurationAsync(configContext);

        Console.WriteLine("=== IDENTITY SEEDER COMPLETED ===");
    }


    private static async Task SeedUsersAndRolesAsync(RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
    {
        const string roleName = "Admin";
        const string email = "admin@example.com";

        if (!await roleManager.RoleExistsAsync(roleName))
        {
            var roleResult = await roleManager.CreateAsync(
                new IdentityRole(roleName));

            if (!roleResult.Succeeded)
            {
                throw new InvalidOperationException(
                    string.Join("; ",
                        roleResult.Errors.Select(e => e.Description)));
            }
        }

        var user = await userManager.FindByEmailAsync(email);

        if (user is null)
        {
            user = new User
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true,
                FirstName = "System",
                LastName = "Administrator",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            // Use a development-only password from configuration
            // or user secrets instead of hard-coding credentials.
            var password = "ChangeThis_DevOnly_123!";

            var result = await userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                throw new InvalidOperationException(
                    string.Join("; ",
                        result.Errors.Select(e => e.Description)));
            }
        }

        if (!await userManager.IsInRoleAsync(user, roleName))
        {
            var result = await userManager.AddToRoleAsync(
                user, roleName);

            if (!result.Succeeded)
            {
                throw new InvalidOperationException(
                    string.Join("; ",
                        result.Errors.Select(e => e.Description)));
            }
        }
    }

    private static async Task SeedDuendeConfigurationAsync(
        ConfigurationDbContext context)
    {
        if (!await context.IdentityResources.AnyAsync())
        {
            foreach (var resource in DuendeConfig.GetIdentityResources)
            {
                context.IdentityResources.Add(resource.ToEntity());
            }
        }

        if (!await context.ApiScopes.AnyAsync())
        {
            foreach (var apiScope in DuendeConfig.GetApiScopes)
            {
                context.ApiScopes.Add(apiScope.ToEntity());
            }
        }

        if (!await context.Clients.AnyAsync())
        {
            foreach (var client in DuendeConfig.GetClients)
            {
                context.Clients.Add(client.ToEntity());
            }
        }

        await context.SaveChangesAsync();
    }
}