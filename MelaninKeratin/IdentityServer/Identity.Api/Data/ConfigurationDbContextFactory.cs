
using Duende.IdentityServer.EntityFramework.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Identity.Api.Data;

public sealed class ConfigurationDbContextFactory
    : IDesignTimeDbContextFactory<ConfigurationDbContext>
{
    public ConfigurationDbContext CreateDbContext(string[] args)
    {
        var connectionString =
            Environment.GetEnvironmentVariable(
                "IDENTITY_DB_CONNECTION_STRING");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                "Set IDENTITY_DB_CONNECTION_STRING before running migrations.");
        }

        var services = new ServiceCollection();

        services.AddLogging();

        services.AddIdentityServer()
     .AddConfigurationStore(options =>
     {
         options.ConfigureDbContext = db =>
             db.UseNpgsql(
                 connectionString,
                 sql =>
                 {
                     sql.MigrationsAssembly(
                         typeof(ConfigurationDbContextFactory)
                             .Assembly.GetName().Name);

                     sql.MigrationsHistoryTable(
                         "__EFMigrationsHistory_Configuration");
                 });
     });

        var serviceProvider = services.BuildServiceProvider();

        return serviceProvider
            .GetRequiredService<ConfigurationDbContext>();
    }
}
