using System.Data;
using Microsoft.Extensions.Configuration;
using MySqlConnector;

namespace LearningAuth.Data;

public class DbContext
{
    private readonly string _connectionString;

    public DbContext(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("SqlConnection")!;
    }

    public IDbConnection CreateConnection()
    {
        return new MySqlConnection(_connectionString);
    }
}