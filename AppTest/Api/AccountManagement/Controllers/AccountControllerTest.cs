using JetBrains.Annotations;
using Moq;
using FluentAssertions;
using Core.Interfaces;
using Core.Identity;
using Web.Api.AccountManagement.Models;
using Core.Dtos;
using Core.Identity.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Web.Api.AccountManagement.Controllers;
using Web.Errors;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.

namespace TestProject.Api.AccountManagement.Controllers;

[TestSubject(typeof(AccountController))]
public class AccountControllerTest
{
    private readonly Mock<UserManager<AppUser>> _mockUserManager;
    private readonly Mock<SignInManager<AppUser>> _mockSignInManager;
    private readonly Mock<ITokenService> _mockTokenService;
    private readonly AccountController _accountController;

    public AccountControllerTest()
    {
        _mockUserManager = new Mock<UserManager<AppUser>>(
            Mock.Of<IUserStore<AppUser>>(), null, null, null, null, null, null, null, null);
        
        _mockSignInManager = new Mock<SignInManager<AppUser>>(
            _mockUserManager.Object,
            Mock.Of<IHttpContextAccessor>(),
            Mock.Of<IUserClaimsPrincipalFactory<AppUser>>(),
            null, null, null, null);
        
        _mockTokenService = new Mock<ITokenService>();

        _accountController = new AccountController(
            _mockUserManager.Object,
            _mockSignInManager.Object,
            _mockTokenService.Object);
    }

#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
    
    [Fact]
    public async Task Login_ReturnsUnauthorized_WhenUserNotFound()
    {
        // Arrange
        var loginModel = new LoginModel { UserName = "nonexistent", Password = "password" };
        _mockUserManager.Setup(um => um.FindByNameAsync(loginModel.UserName)).ReturnsAsync(null as AppUser);

        // Act
        var result = await _accountController.Login(loginModel);

        // Assert
        var unauthorizedResult = result.Result.Should().BeOfType<UnauthorizedObjectResult>().Subject;
        var response = unauthorizedResult.Value.Should().BeOfType<ApiResponse>().Subject;
        response.StatusCode.Should().Be(401);
    }

    [Fact]
    public async Task Login_ReturnsUnauthorized_WhenPasswordIsIncorrect()
    {
        // Arrange
        var user = new AppUser { UserName = "existingUser" };
        var loginModel = new LoginModel { UserName = "existingUser", Password = "wrongPassword" };
        _mockUserManager.Setup(um => um.FindByNameAsync(loginModel.UserName)).ReturnsAsync(user);
        _mockSignInManager.Setup(sm => sm.CheckPasswordSignInAsync(user, loginModel.Password, false))
            .ReturnsAsync(SignInResult.Failed);

        // Act
        var result = await _accountController.Login(loginModel);

        // Assert
        var unauthorizedResult = result.Result.Should().BeOfType<UnauthorizedObjectResult>().Subject;
        var response = unauthorizedResult.Value.Should().BeOfType<ApiResponse>().Subject;
        response.StatusCode.Should().Be(401);
    }

    [Fact]
    public async Task Login_ReturnsUserDto_WhenLoginIsSuccessful()
    {
        // Arrange
        var user = new AppUser { UserName = "existingUser", Role = RoleEnum.Admin, Station = "PRG" };
        var loginModel = new LoginModel { UserName = "existingUser", Password = "correctPassword" };
        _mockUserManager.Setup(um => um.FindByNameAsync(loginModel.UserName)).ReturnsAsync(user);
        _mockSignInManager.Setup(sm => sm.CheckPasswordSignInAsync(user, loginModel.Password, false))
            .ReturnsAsync(SignInResult.Success);
        _mockTokenService.Setup(ts => ts.CreateToken(user)).Returns("this will be a token");

        // Act
        var result = await _accountController.Login(loginModel);

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var userDto = okResult.Value.Should().BeOfType<UserDto>().Subject;
        userDto.Token.Should().Be("this will be a token");
        userDto.UserName.Should().Be("existingUser");
        userDto.Role.Should().Be(RoleEnum.Admin.ToString());
        userDto.Station.Should().Be("PRG");
    }

    [Fact]
    public async Task Register_ReturnsBadRequest_WhenUsernameAlreadyExists()
    {
        // Arrange
        var registerModel = new RegisterModel
        {
            UserName = "existingUser", Password = "password", Role = RoleEnum.PassengerService, Station = "PRG"
        };
        _mockUserManager.Setup(um => um.FindByNameAsync(registerModel.UserName)).ReturnsAsync(new AppUser());

        // Act
        var result = await _accountController.Register(registerModel);

        // Assert
        var badRequestResult = result.Result.Should().BeOfType<BadRequestObjectResult>().Subject;
        badRequestResult.Value.Should().Be("Username already exists.");
    }

    [Fact]
    public async Task Register_ReturnsBadRequest_WhenUserCreationFails()
    {
        // Arrange
        var registerModel = new RegisterModel
        {
            UserName = "newUser", Password = "password", Role = RoleEnum.PassengerService, Station = "PRG"
        };
        _mockUserManager.Setup(um => um.FindByNameAsync(registerModel.UserName)).ReturnsAsync(null as AppUser);
        _mockUserManager.Setup(um => um.CreateAsync(It.IsAny<AppUser>(), registerModel.Password))
            .ReturnsAsync(IdentityResult.Failed());

        // Act
        var result = await _accountController.Register(registerModel);

        // Assert
        var badRequestResult = result.Result.Should().BeOfType<BadRequestObjectResult>().Subject;
        var response = badRequestResult.Value.Should().BeOfType<ApiResponse>().Subject;
        response.StatusCode.Should().Be(400);
    }

    [Fact]
    public async Task Register_ReturnsUserDto_WhenRegistrationIsSuccessful()
    {
        // Arrange
        var registerModel = new RegisterModel
        {
            UserName = "newUser", Password = "password", Role = RoleEnum.PassengerService, Station = "PRG"
        };
        _mockUserManager.Setup(um => um.FindByNameAsync(registerModel.UserName)).ReturnsAsync(null as AppUser);
        _mockUserManager.Setup(um => um.CreateAsync(It.IsAny<AppUser>(), registerModel.Password))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        var result = await _accountController.Register(registerModel);

        // Assert
        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var userDto = okResult.Value.Should().BeOfType<UserDto>().Subject;
        userDto.Token.Should().Be("This will be a token");
        userDto.UserName.Should().Be("newUser");
        userDto.Role.Should().Be(RoleEnum.PassengerService.ToString());
        userDto.Station.Should().Be("PRG");
    }

    [Fact]
    public async Task CheckUserNameExistsAsync_ReturnsTrue_WhenUserExists()
    {
        // Arrange
        const string userName = "existingUser";
        _mockUserManager.Setup(um => um.FindByNameAsync(userName)).ReturnsAsync(new AppUser());

        // Act
        var result = await _accountController.CheckUserNameExistsAsync(userName);

        // Assert
        result.Value.Should().BeTrue();
    }

    [Fact]
    public async Task CheckUserNameExistsAsync_ReturnsFalse_WhenUserDoesNotExist()
    {
        // Arrange
        const string userName = "nonexistentUser";
        _mockUserManager.Setup(um => um.FindByNameAsync(userName)).ReturnsAsync(null as AppUser);

        // Act
        var result = await _accountController.CheckUserNameExistsAsync(userName);

        // Assert
        result.Value.Should().BeFalse();
    }
}