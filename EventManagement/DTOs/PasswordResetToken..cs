namespace EventManagement.DTOs
{
    public class PasswordResetToken
    {
        public string Email { get; set; } = string.Empty;

        public string Token { get; set; } = string.Empty;

        public DateTime? CreatedAt { get; set; }
    }
}
