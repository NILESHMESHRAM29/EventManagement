namespace EventManagement.Models
{
    public class Id_Card
    {
        public long Id { get; set; }
        public long StudentId { get; set; }
        public string GeneratedBy { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public bool IsDelete { get; set; } = false;

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation
        public Student Student { get; set; }
    }
}
