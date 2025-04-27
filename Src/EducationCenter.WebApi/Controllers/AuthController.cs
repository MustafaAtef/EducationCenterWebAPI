using EducationCenter.Application.Dtos;
using EducationCenter.Application.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EducationCenter.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;

    public AuthController(IUserService userService)
    {
        _userService = userService;
    }
    [Authorize(Roles = "Admin")]
    [HttpPost("register")]
    public async Task<ActionResult<AuthenticatedUserDto>> RegisterUser(RegisterDto registerDto)
    {
        return await _userService.RegisterAsync(registerDto);
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthenticatedUserDto>> Login(LoginDto loginDto)
    {
        return await _userService.LoginAsync(loginDto);
    }

    [HttpPost("refresh-token")]
    public async Task<ActionResult<AuthenticatedUserDto>> RefreshToken(RefreshTokenDto refreshTokenDto)
    {
        return await _userService.RefreshTokenAsync(refreshTokenDto);
    }

    [HttpPost("logout")]
    public async Task<ActionResult> Logout(LogoutDto logoutDto)
    {

        await _userService.LogoutAsync(logoutDto.Token);
        return Ok();
    }

    [Authorize]
    [HttpPost("change-password")]
    public async Task<ActionResult> ChangePassword(ChangePasswordDto changePasswordDto)
    {
        await _userService.ChangePasswordAsync(changePasswordDto);
        return Ok();
    }

}
