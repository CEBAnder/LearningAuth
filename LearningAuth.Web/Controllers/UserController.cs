using LearningAuth.Web.Requests;
using LearningAuth.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace LearningAuth.Web.Controllers;

[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<UserController> _logger;

    public UserController(IUserService userService, ILogger<UserController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    [HttpPut]
    public async Task<IActionResult> AddUser([FromBody] AddUserRequest request,
        CancellationToken cancellationToken = default)
    {
        var transactionId = Guid.NewGuid().ToString();
        using (_logger.BeginScope(new List<KeyValuePair<string, object>>
                   { new("TransactionId", transactionId) }))
        {
            _logger.LogInformation("Started creating user {Login}", request.Name);
            var id = await _userService.AddUserAsync(request.ToCommand(), cancellationToken);
            _logger.LogInformation("Created user {Login}", request.Name);
            return Ok(id);
        }
    }
}