using Dapper;
using LearningAuth.Data.Commands;

namespace LearningAuth.Data.Repositories;

public class UserRepository : IUserRepository
{
    private readonly DbContext _dbContext;

    public UserRepository(DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddUserAsync(AddUserCommand command, CancellationToken cancellationToken = default)
    {
        var connection = _dbContext.CreateConnection();
        var sql = "INSERT INTO User VALUES (@id, @pwdHash, @name, @dateOfBirth, @roles)";
        await connection.ExecuteAsync(sql, new
        {
            id = Guid.NewGuid(),
            name = command.Name,
            pwdHash = command.Password.GetHashCode(),
            dateOfBirth = command.DateOfBirth,
            roles = command.Roles
        });
    }
}