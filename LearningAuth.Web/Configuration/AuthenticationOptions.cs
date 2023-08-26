using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace LearningAuth.Web.Configuration;

public class AuthenticationOptions
{
    public const string Authentication = "Authentication";

    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public string Key { get; set; } = string.Empty;
    
    public SymmetricSecurityKey GetSymmetricSecurityKey() => new(Encoding.UTF8.GetBytes(Key));
}