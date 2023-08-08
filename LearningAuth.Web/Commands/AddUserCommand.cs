namespace LearningAuth.Web.Commands;

public class AddUserCommand
{
    public string? Name { get; set; }
    public string Password { get; set; } = null!;
    public DateTime? DateOfBirth { get; set; }
    public string Roles { get; set; } = null!;
}