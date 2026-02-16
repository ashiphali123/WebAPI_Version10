using MySql.Data.MySqlClient;
using Org.BouncyCastle.Crypto.Generators;
using System;
using System.Security.Cryptography;
using System.Text;
using WebAPI_10.DTOs;
using BCrypt.Net;

namespace WebAPI_10.Services
{
    public class AuthService : IAuthService
    {
        private readonly string _connectionStrings;
        public AuthService(IConfiguration configuration)
        {
            _connectionStrings = configuration.GetConnectionString("Dbcs");
        }
        public async Task<UserResponseDto> RegisterAsync(RegisterDto registerDto)
        {
            
            using(MySqlConnection con = new MySqlConnection(_connectionStrings))
            {
                await con.OpenAsync();
                var existingUser = "select count(*) from CouponRegisterUsers where UserName = @name and UserEmailId = @email;";
                MySqlCommand checkcmd = new MySqlCommand(existingUser, con);
                checkcmd.CommandType = System.Data.CommandType.Text;
                checkcmd.Parameters.AddWithValue("@name", registerDto.UserName);
                checkcmd.Parameters.AddWithValue("@email", registerDto.Email);
                int count = Convert.ToInt32(await checkcmd.ExecuteScalarAsync());
                if(count > 0)
                {
                    throw new InvalidOperationException("User with the same username and email already exists.");
                }
                MySqlCommand cmd = new MySqlCommand("sp_registerCouponUser", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("p_UserName", registerDto.UserName);
                cmd.Parameters.AddWithValue("p_UserEmailId", registerDto.Email);
                var hashedPassword = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(registerDto.Password!)));
                
                cmd.Parameters.AddWithValue("p_UserPassWord", hashedPassword);
                await cmd.ExecuteNonQueryAsync();
                return new UserResponseDto
                {
                    UserName = registerDto.UserName,
                    Email = registerDto.Email,
                    CreatedAt = DateTime.Now,
                };
            }

        }
        public async Task<LoginResponseDto> LoginAsync(LoginDto loginDto)
        {
            using(MySqlConnection con = new MySqlConnection(_connectionStrings))
            {
                await con.OpenAsync();
                var query = "select * from CouponRegisterUsers where UserEmailId = @email and UserPassWord=@password;";
                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@email", loginDto.Email);
                var hasspassword = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(loginDto.Password!)));
                cmd.Parameters.AddWithValue("@password", hasspassword);

                MySqlDataReader reader =  cmd.ExecuteReader();
                while (await reader.ReadAsync())
                {
                    return new LoginResponseDto
                    {

                        User = new UserResponseDto
                        {
                            UserName = reader["UserName"].ToString(),
                            Email = reader["UserEmailId"].ToString(),
                            
                        },
                        Expiriesat = DateTime.Now.AddHours(1)
                    };
                }  
            }
            return null;
        }
    }
}
