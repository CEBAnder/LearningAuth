using System.Security.Claims;
using LearningAuth.Web.Requests;
using LearningAuth.Web.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearningAuth.Web.Controllers;

[Authorize]
[Route("Account")]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }
    
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request,
        CancellationToken cancellationToken = default)
    {
        var token = await _accountService.GenerateTokenForUserAsync(request.Name, request.Password, cancellationToken);
        return Ok(token);
    }

    [AllowAnonymous]
    [HttpPost("login/cookie")]
    public async Task<IActionResult> LoginAndGetCookie([FromBody] LoginRequest request,
        CancellationToken cancellationToken = default)
    {
        var principal = await _accountService.GetCookiesPrincipalAsync(request.Name, request.Password, cancellationToken);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        
        return Ok();
    }

    [HttpGet]
    public IActionResult Get()
    {
        var userName = HttpContext.User.Claims.First(claim => claim.Type == ClaimTypes.Name).Value;
        return Ok($"User's name = {userName}");
    }

    [Authorize(Constants.AuthenticationPolicies.CookieAdmin)]
    [HttpGet("cookie")]
    public IActionResult GetByCookie()
    {
        var userName = HttpContext.User.Claims.First(claim => claim.Type == ClaimTypes.Name).Value;
        return Ok($"User's name = {userName}");
    }
}