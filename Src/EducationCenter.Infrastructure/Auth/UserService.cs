using EducationCenter.Core;
using EducationCenter.Core.Entities;
using EducationCenter.Core.Exceptions;
using EducationCenter.Application.Dtos;
using EducationCenter.Application.ServiceContracts;
using Microsoft.AspNetCore.Http;
using EducationCenter.Core.RepositoryContracts;

namespace EducationCenterAPI.Services;

public class UserService : IUserService
{
    private readonly IJwtService _jwtService;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUnitOfWork _unitOfWork;
    public UserService(IJwtService jwtService, IPasswordHasher passwordHasher, IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork)
    {
        _jwtService = jwtService;
        _passwordHasher = passwordHasher;
        _httpContextAccessor = httpContextAccessor;
        _unitOfWork = unitOfWork;
    }

    public async Task ChangePasswordAsync(ChangePasswordDto changePasswordDto)
    {
        var email = _httpContextAccessor?.HttpContext?.User.FindFirst("email")?.Value;
        var user = await _unitOfWork.Users.FindAsync(user => user.Email == email);
        if (user is null || !_passwordHasher.VerifyPassword(changePasswordDto.OldPassword, user.Password)) throw new BadRequestException("Old password is incorrect");
        user.Password = _passwordHasher.HashPassword(changePasswordDto.NewPassword);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<AuthenticatedUserDto> LoginAsync(LoginDto loginDto)
    {
        var user = await _unitOfWork.Users.FindAsync(u => u.Email == loginDto.Email);
        if (user is null || !_passwordHasher.VerifyPassword(loginDto.Password, user.Password)) throw new BadRequestException("Invalid email or password");

        var jwtData = _jwtService.GenerateToken(user);
        user.RefreshToken = jwtData.RefreshToken;
        user.RefreshTokenExpirationDate = jwtData.RefreshTokenExpirationDate;
        await _unitOfWork.SaveChangesAsync();

        return new AuthenticatedUserDto()
        {
            Email = user.Email,
            Name = user.Name,
            Token = jwtData.Token,
            RefreshToken = jwtData.RefreshToken
        };
    }

    public async Task LogoutAsync(string token)
    {
        var claimsPrincipal = _jwtService.ValidateJwt(token);
        if (claimsPrincipal is null) throw new Exception("Invalid token");

        var userEmail = claimsPrincipal.FindFirst("email")?.Value;
        var user = await _unitOfWork.Users.FindAsync(u => u.Email == userEmail);
        if (user is null) throw new BadRequestException("User not logged in");

        user.RefreshToken = null;
        user.RefreshTokenExpirationDate = null;
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<AuthenticatedUserDto> RefreshTokenAsync(RefreshTokenDto refreshTokenDto)
    {
        var claimsPrincipal = _jwtService.ValidateJwt(refreshTokenDto.Token);
        if (claimsPrincipal is null) throw new BadRequestException("Invalid token");

        var userEmail = claimsPrincipal.FindFirst("email")?.Value;
        var user = await _unitOfWork.Users.FindAsync(u => u.Email == userEmail);

        if (user is null || user.RefreshToken != refreshTokenDto.RefreshToken || user.RefreshTokenExpirationDate <= DateTime.Now) throw new BadRequestException("Invalid refresh token or token expired");

        var jwtData = _jwtService.GenerateToken(user);
        user.RefreshToken = jwtData.RefreshToken;
        user.RefreshTokenExpirationDate = jwtData.RefreshTokenExpirationDate;
        await _unitOfWork.SaveChangesAsync();

        return new AuthenticatedUserDto()
        {
            Email = user.Email,
            Name = user.Name,
            Token = jwtData.Token,
            RefreshToken = jwtData.RefreshToken
        };
    }

    public async Task<AuthenticatedUserDto> RegisterAsync(RegisterDto registerDto)
    {
        var existingUser = await _unitOfWork.Users.FindAsync(u => u.Email == registerDto.Email);
        if (existingUser is not null) throw new UniqueException("Email already exists");

        User user = new()
        {
            Email = registerDto.Email,
            Name = registerDto.Name,
            Password = _passwordHasher.HashPassword(registerDto.Password),
            Phone = registerDto.Phone,
            Role = registerDto.Role.Value,
        };
        _unitOfWork.Users.Add(user);

        var jwtData = _jwtService.GenerateToken(user);
        user.RefreshToken = jwtData.RefreshToken;
        user.RefreshTokenExpirationDate = jwtData.RefreshTokenExpirationDate;

        await _unitOfWork.SaveChangesAsync();

        return new AuthenticatedUserDto()
        {
            Email = user.Email,
            Name = user.Name,
            Token = jwtData.Token,
            RefreshToken = jwtData.RefreshToken
        };
    }
}
