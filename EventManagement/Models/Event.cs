namespace EventManagement.Models
{
    public class Event
    {
        public long Id { get; set; }

        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public DateTime EventDate { get; set; }

        public bool IsDelete { get; set; } = false;

        // Foreign key fields (optional relationship)
        public long? AddedBy { get; set; }
        public long? UpdatedBy { get; set; }

        // Navigation properties (optional)
        public User? AddedByUser { get; set; }
        public User? UpdatedByUser { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
