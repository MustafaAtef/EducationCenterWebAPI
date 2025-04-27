using EducationCenter.Application.Dtos;

namespace EducationCenter.Application.ServiceContracts;

public interface IUserService
{
    Task<AuthenticatedUserDto> RegisterAsync(RegisterDto registerDto);
    Task<AuthenticatedUserDto> LoginAsync(LoginDto loginDto);
    Task<AuthenticatedUserDto> RefreshTokenAsync(RefreshTokenDto refreshTokenDto);
    Task LogoutAsync(string token);
    Task ChangePasswordAsync(ChangePasswordDto changePasswordDto);
}
