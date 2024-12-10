using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WhiteLagoon.Domain.Entities
{
    public class UserSession {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public required string Token { get; set; }

        [ForeignKey("User")]
        public required int UserId { get; set; }
        public User? User { get; set; }

        public DateTime ExpiresAt { get; set; }

        public bool IsValid()
        {
            return DateTime.UtcNow < ExpiresAt;
        }
    }
}
