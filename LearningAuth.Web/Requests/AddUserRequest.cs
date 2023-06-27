using LearningAuth.Data.Commands;

namespace LearningAuth.Web.Requests;

public class AddUserRequest
{
    public string? Name { get; set; }
    public string Password { get; set; } = null!;
    public DateTime? DateOfBirth { get; set; }
    public string Roles { get; set; } = null!;

    public AddUserCommand ToCommand()
    {
        return new AddUserCommand
        {
            Name = Name,
            Password = Password,
            DateOfBirth = DateOfBirth,
            Roles = Roles
        };
    }
}