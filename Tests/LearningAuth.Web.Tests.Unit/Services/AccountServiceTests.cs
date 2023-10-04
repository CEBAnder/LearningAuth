using LearningAuth.Data.Models;
using LearningAuth.Data.Repositories;
using LearningAuth.Web.Configuration;
using LearningAuth.Web.Exceptions;
using LearningAuth.Web.Services;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;

namespace LearningAuth.Web.Tests.Unit.Services;

public class AccountServiceTests
{
    [Test]
    public async Task GenerateTokenForUserAsync_ReturnsToken_WhenValidInput()
    {
        var accountService = GetAccountService();

        var token = await accountService.GenerateTokenForUserAsync("name", "password");
        
        Assert.That(token, Is.Not.Empty);
    }
    
    [Test]
    public void GenerateTokenForUserAsync_ThrowsException_WhenUserNotFound()
    {
        var accountService = GetAccountService();

        Assert.ThrowsAsync<UserNotFoundException>(async () =>
            await accountService.GenerateTokenForUserAsync("missing_name", "password"));
    }

    private IAccountService GetAccountService()
    {
        var userRepositoryMock = new Mock<IUserRepository>();
        userRepositoryMock
            .Setup(x => x.FindUser(
                It.Is<string>(s => s.StartsWith('n')), 
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(GenerateFakeUser());

        return new AccountService(userRepositoryMock.Object, Options.Create(GetTestOptions()));
    }

    private User GenerateFakeUser()
    {
        return new User
        {
            Id = Guid.NewGuid(),
            Name = "Name",
            PasswordHash = "PasswordHash",
            DateOfBirth = DateTime.MinValue,
            Roles = "[0,1,2]"
        };
    }

    private AuthenticationOptions GetTestOptions()
    {
        return new AuthenticationOptions
        {
            Audience = "TestAudience",
            Issuer = "TestIssuer",
            Key = "TestKeyLongEnoughToFitKeyLengthRequirements"
        };
    }
}