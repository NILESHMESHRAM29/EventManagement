using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace EventManagement.Models
{
    public class User
    {
        public long Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Required, EmailAddress]
        public string Email { get; set; } = null!;

        public string? Mobile { get; set; }

        public bool IsSend { get; set; } = false;

        public bool IsIdCardDownloaded { get; set; } = false;

        public DateTime? EmailVerifiedAt { get; set; }

        [Required]
        public string Password { get; set; } = null!;

        public long? RoleId { get; set; }
        [ForeignKey("RoleId")]
        public Role? Role { get; set; }

        public int Dept { get; set; } = 0;

        public string? RememberToken { get; set; }

        public bool IsDelete { get; set; } = false;

        public bool IsApproved { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
