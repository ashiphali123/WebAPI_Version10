namespace WebAPI_10.DTOs
{
    public class LoginResponseDto
    {
        public string? MyProperty { get; set; }
        public UserResponseDto? User { get; set; }
        public DateTime Expiriesat { get; set; }
    }
}
