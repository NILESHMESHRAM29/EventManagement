namespace EventManagement.Models
{
    public class ImportBatch
    {
        public int Id { get; set; }
        public int UploadedBy { get; set; }
        public User UploadedByUser { get; set; }
        public int TotalRows { get; set; } = 0;
        public int ProcessedRows { get; set; } = 0;
        public bool IsCompleted { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
