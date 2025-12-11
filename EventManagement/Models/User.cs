using System.ComponentModel.DataAnnotations;

namespace EventManagement.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Required, EmailAddress]
        public string Email { get; set; } = null!;

        public string? Mobile { get; set; }

        public bool IsSend { get; set; } = false;
        public bool IsIdCardDownloaded { get; set; } = false;

        public DateTime? EmailVerifiedAt { get; set; }

        [Required]
        [MaxLength(255)]
        public string Password { get; set; } = null!;

        // Foreign Key
        public int? RoleId { get; set; }
        public Role? Role { get; set; }

        public int Dept { get; set; } = 0;

        public string? RememberToken { get; set; }

        public bool IsDelete { get; set; } = false;

        
        public bool IsApproved { get; set; } = false;

        public DateTime CreatedAt { get; set; }= DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; }= DateTime.UtcNow;

        public ICollection<Student>? Students { get; set; }
        public int FailedLoginAttempts { get; set; }
        public DateTime? LockoutEnd { get; set; }

        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }

    }
}
