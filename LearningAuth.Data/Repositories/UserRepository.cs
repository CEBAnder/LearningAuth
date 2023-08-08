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

    public async Task<User> FindUser(Guid userId, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var connection = _dbContext.CreateConnection();
        var sql = "SELECT * FROM User WHERE Id = @id";
        var user = await connection.QuerySingleOrDefaultAsync<User>(sql, new
        {
            id = userId
        });

        return user ?? throw new Exception($"Not found user with Id = {userId}");
    }

    public async Task<User> FindUser(string name, string passwordHash, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var connection = _dbContext.CreateConnection();
        var sql = "SELECT * FROM User WHERE Name = @name AND PasswordHash = @passwordHash";
        var user = await connection.QuerySingleOrDefaultAsync<User>(sql, new
        {
            name,
            passwordHash
        });

        return user ?? throw new Exception($"Not found user with Name = {name}");
    }
}