using System.ComponentModel.DataAnnotations;

namespace EventManagement.Models
{
    public class Session
    {
        // $table->string('id')->primary()
        [Key]
        [Required]
        [MaxLength(450)] // Ensure max length is defined for the primary key string
        public string Id { get; set; } = string.Empty;

        // $table->foreignId('user_id')->nullable()->index()
        public int? UserId { get; set; }

        // Navigation Property for the User Relationship
        public User? User { get; set; }

        // $table->string('ip_address', 45)->nullable()
        [MaxLength(45)]
        public string? IpAddress { get; set; }

        // $table->text('user_agent')->nullable()
        // Default string maps to text/nvarchar(max)
        public string? UserAgent { get; set; }

        // $table->longText('payload')
        [Required]
        public string Payload { get; set; } = string.Empty;

        // $table->integer('last_activity')->index()
        // Assuming this integer stores a Unix timestamp
        public int LastActivity { get; set; }
    }
}
