using LearningAuth.Data.Repositories;
using LearningAuth.Web.Commands;
using User = LearningAuth.Data.Models.User;

namespace LearningAuth.Web.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Guid> AddUserAsync(AddUserCommand command, CancellationToken cancellationToken = default)
    {
        var newUserId = Guid.NewGuid();
        var userToAdd = new User
        {
            Id = newUserId,
            Name = command.Name,
            PasswordHash = command.Password.GetHashCode().ToString(),
            DateOfBirth = command.DateOfBirth,
            Roles = command.Roles
        };

        await _userRepository.AddUserAsync(userToAdd, cancellationToken);
        return newUserId;
    }

    public async Task<User> FindUserAsync(string name, string password, CancellationToken cancellationToken = default)
    {
        return await _userRepository.FindUser(name, password.GetHashCode().ToString(), cancellationToken);
    }
}