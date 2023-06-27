using LearningAuth.Data.Commands;

namespace LearningAuth.Data.Repositories;

public interface IUserRepository
{
    Task AddUserAsync(AddUserCommand command, CancellationToken cancellationToken = default);
}