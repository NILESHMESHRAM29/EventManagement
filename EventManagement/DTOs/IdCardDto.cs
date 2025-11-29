namespace EventManagement.DTOs
{
    public class IdCardDto
    {
        public string Name { get; set; } = null!;
        public string? Role { get; set; }
        public string IdNumber { get; set; } = null!;
        public string? EventName { get; set; }
    }
}
