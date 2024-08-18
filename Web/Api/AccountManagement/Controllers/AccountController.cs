using Core.Dtos;
using Core.Identity;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Web.Api.AccountManagement.Models;
using Web.Errors;

namespace Web.Api.AccountManagement.Controllers;

[ApiController]
[Route("account-management")]
public class AccountController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly ITokenService _tokenService;
    
    public AccountController(
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        ITokenService tokenService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
    }

    /// <summary>
    /// Logs in a user using the specified login model.
    /// </summary>
    /// <param name="loginModel">The login model containing the user's username.</param>
    /// <returns>Returns an <see cref="ActionResult&lt;UserDto&gt;"/> representing the result of the login operation.
    /// </returns>
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginModel loginModel)
    {
        var user = await _userManager.FindByNameAsync(loginModel.UserName);
        
        if (user == null) return Unauthorized(new ApiResponse(401));
        
        var result = await _signInManager.CheckPasswordSignInAsync(user, loginModel.Password, false);
        
        if (!result.Succeeded) return Unauthorized(new ApiResponse(401));

        return Ok(new UserDto
        {
            Token = _tokenService.CreateToken(user),
            UserName = user.UserName,
            Role = user.Role.ToString(),
            Station = user.Station
        });
    }

    /// <summary>
    /// Registers a new user.
    /// </summary>
    /// <param name="registerModel">The registration model containing user information.</param>
    /// <returns>The registered user DTO.</returns>
    [Authorize(Roles = "Admin")]
    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(RegisterModel registerModel)
    {
        var existingUser = await _userManager.FindByNameAsync(registerModel.UserName);

        if (existingUser != null)
        {
            return BadRequest("Username already exists.");
        }
        
        var user = new AppUser
        {
            UserName = registerModel.UserName,
            Role =  registerModel.Role,
            Station = registerModel.Station,
        };
        
        var result = await _userManager.CreateAsync(user, registerModel.Password);
        
        if (!result.Succeeded) return BadRequest(new ApiResponse(400));
        
        return Ok(new UserDto
        {
            Token = "This will be a token",
            UserName = user.UserName,
            Role = user.Role.ToString(),
            Station = user.Station
        });
    }

    /// <summary>
    /// Checks if a username exists in the system.
    /// </summary>
    /// <param name="userName">The username to check.</param>
    /// <returns>Returns a boolean value indicating whether the username exists or not.</returns>
    [HttpGet("username-exists")]
    public async Task<ActionResult<bool>> CheckUserNameExistsAsync([FromQuery] string userName)
    {
        return await _userManager.FindByNameAsync(userName) != null;
    }
}