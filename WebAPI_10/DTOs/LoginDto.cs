using System.ComponentModel.DataAnnotations;

namespace WebAPI_10.DTOs
{
    public class LoginDto
    {
        
        [Required(ErrorMessage = "Password is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address format")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string? Password { get; set; }
    }
}
