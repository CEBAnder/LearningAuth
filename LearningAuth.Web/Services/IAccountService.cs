using System.Security.Claims;

namespace LearningAuth.Web.Services;

public interface IAccountService
{
    Task<string> GenerateTokenForUserAsync(string name, string password, CancellationToken cancellationToken = default);
    
    Task<ClaimsPrincipal> GetCookiesPrincipalAsync(string name, string password,
        CancellationToken cancellationToken = default);
}