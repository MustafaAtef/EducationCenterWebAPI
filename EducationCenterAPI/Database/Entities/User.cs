namespace EducationCenterAPI.Database.Entities;
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


public enum UserRoles
{
    NotAssigned = 0,
    Admin = 1,
    Secretary = 2,
}