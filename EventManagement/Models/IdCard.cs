using System.ComponentModel.DataAnnotations;

namespace EventManagement.Models
{
    public class IdCard
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public Student Student { get; set; }

        [Required]
        [MaxLength(255)]
        public string GeneratedBy { get; set; } = string.Empty;
        [Required]
        public string FilePath { get; set; } = string.Empty;
        public bool IsDelete { get; set; } = false;

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
