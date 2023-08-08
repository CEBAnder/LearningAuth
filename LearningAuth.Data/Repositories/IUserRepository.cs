using LearningAuth.Data.Models;

namespace LearningAuth.Data.Repositories;

public interface IUserRepository
{
    Task AddUserAsync(User user, CancellationToken cancellationToken = default);
}