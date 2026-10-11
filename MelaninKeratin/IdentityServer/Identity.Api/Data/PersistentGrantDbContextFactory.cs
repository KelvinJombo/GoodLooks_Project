using Duende.IdentityServer.EntityFramework.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Identity.Api.Data;

public sealed class PersistedGrantDbContextFactory
    : IDesignTimeDbContextFactory<PersistedGrantDbContext>
{
    public PersistedGrantDbContext CreateDbContext(string[] args)
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
            .AddOperationalStore(options =>
            {
                options.ConfigureDbContext = db =>
                    db.UseNpgsql(
                        connectionString,
                        sql =>
                        {
                            sql.MigrationsAssembly(
                                typeof(PersistedGrantDbContextFactory)
                                    .Assembly.GetName().Name);

                            sql.MigrationsHistoryTable(
                                "__EFMigrationsHistory_Operational");
                        });
            });

        var serviceProvider = services.BuildServiceProvider();

        return serviceProvider
            .GetRequiredService<PersistedGrantDbContext>();
    }
}
