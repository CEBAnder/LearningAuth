using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using LearningAuth.Web.Models;
using LearningAuth.Web.Requests;
using LearningAuth.Web.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

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
        var user = await _userService.FindUserAsync(request.Name, request.Password, cancellationToken);
        var claims = new List<Claim> {new(ClaimTypes.Name, user.Name!) };
        var roles = user.Roles.Split(',');
        foreach (var role in roles)
        {
            claims.Add(new(ClaimTypes.Role, role));
        }
        var jwt = new JwtSecurityToken(
            issuer: AuthOptions.ISSUER,
            audience: AuthOptions.AUDIENCE,
            claims: claims,
            expires: DateTime.UtcNow.Add(TimeSpan.FromDays(2)),
            signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            
        return Ok(new JwtSecurityTokenHandler().WriteToken(jwt));
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("Got secret string");
    }
}