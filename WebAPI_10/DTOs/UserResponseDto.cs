namespace WebAPI_10.DTOs
{
    public class UserResponseDto
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
