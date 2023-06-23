namespace LearningAuth.Data.Models;

public class User
{
    public Guid Id { get; set; }
    public string PasswordHash { get; set; } = null!;
    public string? Name { get; set; }
    public DateTime? DateOfBirth { get; set; }
}