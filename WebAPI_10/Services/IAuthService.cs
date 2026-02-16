using WebAPI_10.DTOs;
using WebAPI_10.Models;

namespace WebAPI_10.Services
{
    public interface IAuthService
    {
        Task<UserResponseDto> RegisterAsync(RegisterDto registerDto);
        Task<LoginResponseDto> LoginAsync(LoginDto loginDto);
    }
    
}
