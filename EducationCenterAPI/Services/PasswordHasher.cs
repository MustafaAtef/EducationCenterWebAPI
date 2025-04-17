using System;
using System.Security.Cryptography;
using EducationCenterAPI.ServiceContracts;

namespace EducationCenterAPI.Services;

public class PasswordHasher : IPasswordHasher
{
    private const int SaltSize = 16;
    private const int HashSize = 32;
    private const int Iterations = 200000;
    private static readonly HashAlgorithmName Algorithm = HashAlgorithmName.SHA512;
    public string HashPassword(string password)
    {
        byte[] salt = new byte[SaltSize];
        using var randomNumber = RandomNumberGenerator.Create();
        randomNumber.GetBytes(salt);
        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, Algorithm, HashSize);

        return Convert.ToBase64String(salt) + "." + Convert.ToBase64String(hash);
    }

    public bool VerifyPassword(string password, string hashedPassword)
    {

        var parts = hashedPassword.Split('.');
        if (parts.Length != 2) return false;

        byte[] salt = Convert.FromBase64String(parts[0]);
        byte[] hash = Convert.FromBase64String(parts[1]);

        byte[] hashToVerify = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, Algorithm, HashSize);

        return CryptographicOperations.FixedTimeEquals(hash, hashToVerify);
    }
}
