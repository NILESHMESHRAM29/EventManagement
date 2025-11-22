namespace EventManagement.Models
{
    public class Role
    {
        public long Id { get; set; }

        public string Name { get; set; } = null!;

        public bool IsDelete { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
