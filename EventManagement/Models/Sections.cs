namespace EventManagement.Models
{
    public class Sections
    {
        public long Id { get; set; }

        public string Name { get; set; } = null!;

        public bool IsDelete { get; set; } = false;

        public long? AddedBy { get; set; }

        // Navigation property to User (optional)
        public User? AddedByUser { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation for many-to-many with volunteers (if needed)
        public ICollection<SectionVolunteer>? SectionVolunteers { get; set; }

    }
}
