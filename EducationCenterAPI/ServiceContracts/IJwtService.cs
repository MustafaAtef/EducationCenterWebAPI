using System.Security.Claims;
using EducationCenterAPI.Database.Entities;
using EducationCenterAPI.Dtos;

namespace EducationCenterAPI.ServiceContracts
{
    public interface IJwtService
    {
        JwtDto GenerateToken(User user);
        ClaimsPrincipal? ValidateJwt(string token);
    }
}
