using System.ComponentModel.DataAnnotations;
using EducationCenter.Core.Enumerations;
using EducationCenter.Application.CustomValidations;

namespace EducationCenter.Application.Dtos;

public class RegisterDto
{
    [StringLength(100, ErrorMessage = "User name maximum length is 100 characters")]
    [Required(ErrorMessage = "User name is required")]
    public string Name { get; set; }

    [StringLength(100, ErrorMessage = "User email maximum length is 100 characters")]
    [EmailAddress(ErrorMessage = "Invalid provided email")]
    [Required(ErrorMessage = "User email is required")]
    public string Email { get; set; }

    [StringLength(100, ErrorMessage = "User password maximum length is 100 characters")]
    [Required(ErrorMessage = "User password is required")]
    public string Password { get; set; }

    [StringLength(11, ErrorMessage = "User phone maximum length is 11 numbers")]
    [PhoneNumber(ErrorMessage = "Invalid provided phone number")]
    [Required(ErrorMessage = "User phone is required")]
    public string Phone { get; set; }

    [EnumValue(typeof(UserRoles), ErrorMessage = "Invalid provided role")]
    [Required(ErrorMessage = "User role is required")]
    public UserRoles? Role { get; set; } = null;
}

public class LoginDto
{
    [EmailAddress]
    [StringLength(100, ErrorMessage = "User email maximum length is 100 characters")]
    [Required(ErrorMessage = "User email is required")]
    public string Email { get; set; }

    [StringLength(100, ErrorMessage = "User password maximum length is 100 characters")]
    [Required(ErrorMessage = "User password is required")]
    public string Password { get; set; }
}

public class RefreshTokenDto
{
    [Required(ErrorMessage = "Jwt token is required")]
    public string Token { get; set; }
    [Required(ErrorMessage = "Refresh token is required")]
    public string RefreshToken { get; set; }
}

public class LogoutDto
{
    [Required(ErrorMessage = "Jwt token is required")]
    public string Token { get; set; }
}

public class ChangePasswordDto
{
    [Required(ErrorMessage = "Old password is required")]
    public string OldPassword { get; set; }
    [Required(ErrorMessage = "New password is required")]

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
