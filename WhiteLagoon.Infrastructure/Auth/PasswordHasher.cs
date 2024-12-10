using System.Security.Cryptography;
using System.Text;
using WhiteLagoon.Application.Common.Interfaces;

namespace WhiteLagoon.Infrastructure.Auth
{
    public class PasswordHasher : IPasswordHasher
    {
        public string Hash(string password)
        {
            using var sha256 = SHA256.Create();
            return Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(password)));
        }
    }
}
