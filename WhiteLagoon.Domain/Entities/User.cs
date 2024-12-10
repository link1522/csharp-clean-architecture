using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace WhiteLagoon.Domain.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string PasswordHash { get; set; }

        public bool VerifyPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedPassword = Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(password)));
            return hashedPassword == PasswordHash;
        }
    }
}
