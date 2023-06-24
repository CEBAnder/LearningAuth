using Dapper;

namespace LearningAuth.Data;

public class Database
{
    private readonly DbContext _dbContext;

    public Database(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CreateDatabaseAsync(string dbName, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var query = $"SHOW DATABASES LIKE '{dbName}';";

        using var connection = _dbContext.CreateConnection();
        var databases = await connection.QueryAsync(query);
        if (!databases.Any())
        {
            var createQuery = $"CREATE DATABASE {dbName};";
            await connection.ExecuteAsync(createQuery);
        }
    }
}