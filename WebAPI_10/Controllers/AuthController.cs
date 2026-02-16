using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI_10.DTOs;
using WebAPI_10.Models;
using WebAPI_10.Services;

namespace WebAPI_10.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly TokenService _token;
        public AuthController(IAuthService authService,TokenService tokenService)
        {
            _authService = authService;
            _token = tokenService;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                var result = await _authService.RegisterAsync(registerDto);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch(Exception ex)
            {
                return StatusCode(500, new { message = "An error encountered" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                var result = await _authService.LoginAsync(loginDto);

                if (result == null)
                {
                    return Unauthorized(new { message = "Invalid email or password" });
                }
                
                // Map DTO -> User
                var userModel = new User
                {
                    Id = result.User.Id,
                    UserName = result.User.UserName,
                    Email = result.User.Email
                };

                var token = _token.GenerateToken(userModel);

                return Ok(new{token,user = result.User});
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

    }
}
