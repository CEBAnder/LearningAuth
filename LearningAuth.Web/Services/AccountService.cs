using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using LearningAuth.Contracts.Shared;
using LearningAuth.Data.Repositories;
using LearningAuth.Web.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Tokens;

namespace LearningAuth.Web.Services;

public class AccountService : IAccountService
{
    private readonly IUserRepository _userRepository;

    public AccountService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<string> GenerateTokenForUserAsync(string name, string password, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.FindUser(name, HashPassword(password), cancellationToken);
        var claims = new List<Claim> {new(ClaimTypes.Name, user.Name!) };
        var roles = JsonSerializer.Deserialize<IEnumerable<Role>>(user.Roles)!;
        foreach (var role in roles)
        {
            claims.Add(new(ClaimTypes.Role, role.ToString()));
        }
        var jwt = new JwtSecurityToken(
            issuer: AuthOptions.ISSUER,
            audience: AuthOptions.AUDIENCE,
            claims: claims,
            expires: DateTime.UtcNow.Add(TimeSpan.FromDays(2)),
            signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }

    public async Task<ClaimsPrincipal> GetCookiesPrincipalAsync(string name, string password, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.FindUser(name, HashPassword(password), cancellationToken);
        var claims = new List<Claim> {new(ClaimTypes.Name, user.Name!) };
        var roles = JsonSerializer.Deserialize<IEnumerable<Role>>(user.Roles)!;
        foreach (var role in roles)
        {
            claims.Add(new(ClaimTypes.Role, role.ToString()));
        }

        return new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme));
    }
    
    private string HashPassword(string password)
    {
        return Convert.ToHexString(MD5.HashData(Encoding.UTF8.GetBytes(password)));
    }
}