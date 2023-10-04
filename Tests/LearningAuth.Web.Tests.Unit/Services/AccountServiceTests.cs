using LearningAuth.Web.Services;
using Moq;
using NUnit.Framework;

namespace LearningAuth.Web.Tests.Unit.Services;

public class AccountServiceTests
{
    [Test]
    public async Task GenerateTokenForUserAsync_ReturnsToken_WhenValidInput()
    {
        var accountService = GetAccountServiceMock();

        var token = await accountService.GenerateTokenForUserAsync("name", "password");
        
        Assert.That(token, Is.Not.Empty);
    }

    private IAccountService GetAccountServiceMock()
    {
        var mock = new Mock<IAccountService>();
        mock
            .Setup(x => x.GenerateTokenForUserAsync(
                It.IsAny<string>(), 
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync("some_token");

        return mock.Object;
    }
}