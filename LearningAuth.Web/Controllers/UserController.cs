using LearningAuth.Web.Requests;
using LearningAuth.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace LearningAuth.Web.Controllers;

[Route("User")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    public async Task<IActionResult> AddUser([FromBody]AddUserRequest request, CancellationToken cancellationToken = default)
    {
        var id = await _userService.AddUserAsync(request.ToCommand(), cancellationToken);
        return Ok(id);
    }
}