using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace LearningAuth.Web.Models;

public class AuthOptions
{
    public const string ISSUER = "LearningAuth";
    public const string AUDIENCE = "AUDIENCE";
    private const string KEY = "mysupersecret_secretkey!123";   // ключ для шифрации
    public static SymmetricSecurityKey GetSymmetricSecurityKey() => new(Encoding.UTF8.GetBytes(KEY));
}