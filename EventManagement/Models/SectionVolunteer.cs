using static System.Collections.Specialized.BitVector32;

namespace EventManagement.Models
{
    public class SectionVolunteer
    {
        public long Id { get; set; }

        // Foreign Keys
        public long SectionId { get; set; }
        public long UserId { get; set; }

        // Navigation Properties (recommended)
        public Section? Section { get; set; }
        public User? User { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
