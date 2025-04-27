using System;

namespace EducationCenter.Application.ServiceContracts;

public interface IPasswordHasher
{
    string HashPassword(string password);
    bool VerifyPassword(string password, string hashedPassword);
}
