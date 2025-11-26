using System.ComponentModel.DataAnnotations;

namespace EventManagement.Models
{
    public class Event
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(255)]
        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public DateTime EventDate { get; set; }

        public bool IsDelete { get; set; } = false;

        // Foreign key fields (optional relationship)
        public int? AddedBy { get; set; }
        public User? AddedByUser { get; set; }
        public int? UpdatedBy { get; set; }

        // Navigation properties (optional)
        public User? UpdatedByUser { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
