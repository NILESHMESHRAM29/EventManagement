using System.ComponentModel.DataAnnotations;

namespace EventManagement.Models
{
    public class Permission
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;

        public bool IsDelete { get; set; } = false;

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}
