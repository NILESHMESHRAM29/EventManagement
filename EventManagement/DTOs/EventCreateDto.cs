namespace EventManagement.DTOs
{
    public class EventCreateDto
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime EventDate { get; set; }
    }
}
