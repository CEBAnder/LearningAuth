using System.Security.Claims;
using LearningAuth.Web.Requests;
using LearningAuth.Web.Services;
using Microsoft.AspNetCore.Authorization;
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

    [HttpPut]
    public async Task<IActionResult> AddUser([FromBody]AddUserRequest request, CancellationToken cancellationToken = default)
    {
        var id = await _userService.AddUserAsync(request.ToCommand(), cancellationToken);
        return Ok(id);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request,
        CancellationToken cancellationToken = default)
    {
        var token = await _userService.GenerateTokenForUserAsync(request.Name, request.Password, cancellationToken);
        return Ok(token);
    }

    [Authorize]
    [HttpGet]
    public IActionResult Get()
    {
        var userName = HttpContext.User.Claims.First(claim => claim.Type == ClaimTypes.Name).Value;
        return Ok($"User's name = {userName}");
    }
}