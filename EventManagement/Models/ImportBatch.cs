namespace EventManagement.Models
{
    public class ImportBatch
    {
        public long Id { get; set; }
        public long UploadedBy { get; set; }
        public int TotalRows { get; set; } = 0;
        public int ProcessedRows { get; set; } = 0;
        public bool IsCompleted { get; set; } = false;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
