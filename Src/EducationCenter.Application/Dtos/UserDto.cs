using System.ComponentModel.DataAnnotations;
using EducationCenter.Core.Enumerations;
using EducationCenter.Application.CustomValidations;

namespace EducationCenter.Application.Dtos;
public class RegisterDto
{
    [StringLength(100, ErrorMessage = "User name maximum length is 100 characters")]
    public string Name { get; set; }

    [StringLength(100, ErrorMessage = "User email maximum length is 100 characters")]
    [EmailAddress(ErrorMessage = "Invalid provided email")]
    public string Email { get; set; }

    [StringLength(100, ErrorMessage = "User password maximum length is 100 characters")]
    public string Password { get; set; }

    [StringLength(11, ErrorMessage = "User phone maximum length is 11 numbers")]
    [PhoneNumber(ErrorMessage = "Invalid provided phone number")]
    public string Phone { get; set; }

    [EnumValue(typeof(UserRoles), ErrorMessage = "Invalid provided role")]
    [Required(ErrorMessage = "User role is required")]
    public UserRoles? Role { get; set; } = null;
}

public class LoginDto
{
    [EmailAddress]
    [StringLength(100, ErrorMessage = "User email maximum length is 100 characters")]
    public string Email { get; set; }

    [StringLength(100, ErrorMessage = "User password maximum length is 100 characters")]
    public string Password { get; set; }
}

public class RefreshTokenDto
{
    public string Token { get; set; }
    public string RefreshToken { get; set; }
}

public class LogoutDto
{
    public string Token { get; set; }
}

public class ChangePasswordDto
{
    public string OldPassword { get; set; }

    [StringLength(100, ErrorMessage = "New password maximum length is 100 characters")]
    public string NewPassword { get; set; }
}


public class AuthenticatedUserDto
{
    public string Email { get; set; }
    public string Name { get; set; }
    public string Token { get; set; }
    public string RefreshToken { get; set; }
}
