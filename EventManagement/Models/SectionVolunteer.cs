namespace EventManagement.Models
{
    public class SectionVolunteer
    {
        public int Id { get; set; }

        // Foreign Keys
        public int SectionId { get; set; }
        public Section? Section { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
