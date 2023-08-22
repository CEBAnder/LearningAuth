using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using LearningAuth.Data.Models;
using LearningAuth.Data.Repositories;
using LearningAuth.Web.Commands;

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
            PasswordHash = HashPassword(command.Password),
            DateOfBirth = command.DateOfBirth,
            Roles = JsonSerializer.Serialize(command.Roles)
        };

        await _userRepository.AddUserAsync(userToAdd, cancellationToken);
        return newUserId;
    }

    private string HashPassword(string password)
    {
        return Convert.ToHexString(MD5.HashData(Encoding.UTF8.GetBytes(password)));
    }
}