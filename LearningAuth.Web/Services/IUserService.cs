using LearningAuth.Data.Models;
using LearningAuth.Web.Commands;

namespace LearningAuth.Web.Services;

public interface IUserService
{
    Task<Guid> AddUserAsync(AddUserCommand command, CancellationToken cancellationToken = default);
    Task<User> FindUserAsync(string name, string password, CancellationToken cancellationToken = default);
}