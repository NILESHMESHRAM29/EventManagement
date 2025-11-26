using System.ComponentModel.DataAnnotations;

namespace EventManagement.Models
{
    public class Scan
    {
        public int Id { get; set; }

        public int StudentId { get; set; }
        public Student Student { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int SectionId { get; set; }
        public Section Section { get; set; }
        public DateTime ScannedAt { get; set; }
        [MaxLength(45)]
        public string? DeviceIp { get; set; }
        public bool IsDelete { get; set; } = false;

        public DateTime CreatedAt { get; set; }   
        public DateTime UpdatedAt { get; set; }

    }
}
