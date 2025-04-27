using EducationCenter.Core.Enumerations;

namespace EducationCenter.Core.Entities;
public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Phone { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpirationDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public UserRoles Role { get; set; }
}
