using Dapper;
using LearningAuth.Data.Models;

namespace LearningAuth.Data.Repositories;

public class UserRepository : IUserRepository
{
    private readonly DbContext _dbContext;

    public UserRepository(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddUserAsync(User user, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var connection = _dbContext.CreateConnection();
        var sql = "INSERT INTO User VALUES (@id, @pwdHash, @name, @dateOfBirth, @roles)";
        await connection.ExecuteAsync(sql, new
        {
            id = user.Id,
            name = user.Name,
            pwdHash = user.PasswordHash,
            dateOfBirth = user.DateOfBirth,
            roles = user.Roles
        });
    }
}