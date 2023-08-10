using LearningAuth.Web.Commands;

namespace LearningAuth.Web.Services;

public interface IUserService
{
    Task<Guid> AddUserAsync(AddUserCommand command, CancellationToken cancellationToken = default);
    Task<string> GenerateTokenForUserAsync(string name, string password, CancellationToken cancellationToken = default);
}