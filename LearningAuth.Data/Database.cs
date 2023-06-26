using Dapper;

namespace LearningAuth.Data;

public class Database
{
    private readonly DbContext _dbContext;

    public Database(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void CreateDatabase(string dbName)
    {
        var query = $"SHOW DATABASES LIKE '{dbName}';";

        using var connection = _dbContext.CreateConnection();
        var databases = connection.Query(query);
        if (!databases.Any())
        {
            var createQuery = $"CREATE DATABASE {dbName};";
            connection.Execute(createQuery);
        }
    }
}