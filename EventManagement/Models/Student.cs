using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace EventManagement.Models
{
    public class Student
    {
        public int Id { get; set; }

        // Foreign Key
        public int UserId { get; set; }
        public User User { get; set; }

        [Required]
        [MaxLength(255)]
        public string UniqueId { get; set; }

        [MaxLength(255)]
        public string? QrCodePath { get; set; }

        [MaxLength(255)]
        public string? IdCardPath { get; set; }

        public bool IsDelete { get; set; } = false;

        public int? AddedBy { get; set; }
        public User? AddedByUser { get; set; }

        public int? UpdateBy { get; set; }
        public User? UpdateByUser { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}
