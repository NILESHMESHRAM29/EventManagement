using System.ComponentModel.DataAnnotations;

namespace EventManagement.Models
{
    public class Section
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        public bool IsDelete { get; set; } = false;

        public int? AddedBy { get; set; }
        public User? AddedByUser { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    }
}
