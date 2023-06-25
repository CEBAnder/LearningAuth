using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LearningAuth.Data;

public static class MigrationManager
{
    public static async Task<IHost> MigrateDatabaseAsync(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        var databaseService = scope.ServiceProvider.GetRequiredService<Database>();
        await databaseService.CreateDatabaseAsync("Users");

        var migrationRunner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
        migrationRunner.MigrateUp();
        
        return host;
    }
}