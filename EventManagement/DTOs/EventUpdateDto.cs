namespace EventManagement.DTOs
{
    public class EventUpdateDto
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime EventDate { get; set; }
    }
}
