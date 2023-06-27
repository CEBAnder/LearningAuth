using LearningAuth.Data.Repositories;
using LearningAuth.Web.Requests;
using Microsoft.AspNetCore.Mvc;

namespace LearningAuth.Web.Controllers;

[Route("User")]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;

    public UserController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpPost]
    public async Task<IActionResult> AddUser([FromBody]AddUserRequest request, CancellationToken cancellationToken = default)
    {
        await _userRepository.AddUserAsync(request.ToCommand(), cancellationToken);
        return Ok("User successfully created!");
    }
}