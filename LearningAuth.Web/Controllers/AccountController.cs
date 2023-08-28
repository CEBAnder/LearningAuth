using System.Security.Claims;
using LearningAuth.Web.Requests;
using LearningAuth.Web.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace LearningAuth.Web.Controllers;

[Authorize]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;
    private readonly ILogger<AccountController> _logger;

    public AccountController(IAccountService accountService, ILogger<AccountController> logger)
    {
        _accountService = accountService;
        _logger = logger;
    }
    
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request,
        CancellationToken cancellationToken = default)
    {
        var token = await _accountService.GenerateTokenForUserAsync(request.Name, request.Password, cancellationToken);
        _logger.LogInformation("User {Login} got his JWT", request.Name);
        return Ok(token);
    }

    [AllowAnonymous]
    [HttpPost("login/cookie")]
    public async Task<IActionResult> LoginAndGetCookie([FromBody] LoginRequest request,
        CancellationToken cancellationToken = default)
    {
        var principal = await _accountService.GetCookiesPrincipalAsync(request.Name, request.Password, cancellationToken);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        _logger.LogInformation("User {Login} got his cookie", request.Name);
        return Ok();
    }

    [HttpGet]
    public IActionResult Get()
    {
        _logger.LogInformation("User {Login} called Get with JWT", HttpContext.User.Identity?.Name);
        var userName = HttpContext.User.Claims.First(claim => claim.Type == ClaimTypes.Name).Value;
        return Ok($"User's name = {userName}");
    }

    [EnableRateLimiting(Constants.RateLimiterPolicies.FixedWindow)]
    [Authorize(Constants.AuthenticationPolicies.CookieAdmin)]
    [HttpGet("cookie")]
    public IActionResult GetByCookie()
    {
        _logger.LogInformation("User {Login} called Get with cookie", HttpContext.User.Identity?.Name);
        var userName = HttpContext.User.Claims.First(claim => claim.Type == ClaimTypes.Name).Value;
        return Ok($"User's name = {userName}");
    }
}