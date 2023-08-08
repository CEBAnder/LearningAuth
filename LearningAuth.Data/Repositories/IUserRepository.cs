using LearningAuth.Data.Models;

namespace LearningAuth.Data.Repositories;

public interface IUserRepository
{
    Task AddUserAsync(User user, CancellationToken cancellationToken = default);
    Task<User> FindUser(Guid userId, CancellationToken cancellationToken = default);
    Task<User> FindUser(string name, string passwordHash, CancellationToken cancellationToken = default);
}