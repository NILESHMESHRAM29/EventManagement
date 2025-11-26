using System.ComponentModel.DataAnnotations;

namespace EventManagement.DTOs
{
    public class PasswordResetToken
    {
        // $table->string('email')->primary()
        [Key]
        [Required]
        [MaxLength(255)] // Define max length for consistency
        public string Email { get; set; } = string.Empty;

        // $table->string('token')
        [Required]
        public string Token { get; set; } = string.Empty;

        // $table->timestamp('created_at')->nullable()
        public DateTime? CreatedAt { get; set; }
    }
}
