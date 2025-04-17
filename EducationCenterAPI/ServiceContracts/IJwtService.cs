using System.Security.Claims;
using EducationCenterAPI.Dtos;

namespace EducationCenterAPI.ServiceContracts
{
    public interface IJwtService
    {
        JwtDto GenerateToken(string email, int id);
        ClaimsPrincipal? ValidateJwt(string token);
    }
}
