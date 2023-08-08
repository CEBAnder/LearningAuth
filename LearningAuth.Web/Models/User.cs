namespace LearningAuth.Web.Models;

public class User
{
    public string? Name { get; set; }
    public string Password { get; set; } = null!;
    public DateTime? DateOfBirth { get; set; }
    public string Roles { get; set; } = null!;
}