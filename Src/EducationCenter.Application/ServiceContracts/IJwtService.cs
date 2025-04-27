using System.Security.Claims;
using EducationCenter.Application.Dtos;
using EducationCenter.Core.Entities;

namespace EducationCenter.Application.ServiceContracts;

public interface IJwtService
{
    JwtDto GenerateToken(User user);
    ClaimsPrincipal? ValidateJwt(string token);
}
