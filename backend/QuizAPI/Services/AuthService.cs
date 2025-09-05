using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using QuizAPI.Data;
using QuizAPI.DTOs;
using QuizAPI.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace QuizAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly QuizDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly ICosmicService _cosmicService;

        public AuthService(QuizDbContext context, IConfiguration configuration, ICosmicService cosmicService)
        {
            _context = context;
            _configuration = configuration;
            _cosmicService = cosmicService;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto)
        {
            // Check if user already exists
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == registerDto.Email || u.Username == registerDto.Username);

            if (existingUser != null)
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "User with this email or username already exists"
                };
            }

            // Create new user
            var user = new User
            {
                Email = registerDto.Email,
                Username = registerDto.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
                Role = UserRole.User,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Sync with Cosmic
            await _cosmicService.SyncUserToCosmicAsync(user);

            var token = GenerateJwtToken(user.Id, user.Username, user.Role.ToString());

            return new AuthResponseDto
            {
                Success = true,
                Token = token,
                User = new UserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    Username = user.Username,
                    Role = user.Role.ToString(),
                    TotalScore = user.TotalScore,
                    QuizzesTaken = user.QuizzesTaken
                }
            };
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == loginDto.EmailOrUsername || u.Username == loginDto.EmailOrUsername);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = "Invalid email/username or password"
                };
            }

            var token = GenerateJwtToken(user.Id, user.Username, user.Role.ToString());

            return new AuthResponseDto
            {
                Success = true,
                Token = token,
                User = new UserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    Username = user.Username,
                    Role = user.Role.ToString(),
                    TotalScore = user.TotalScore,
                    QuizzesTaken = user.QuizzesTaken,
                    AvatarUrl = user.AvatarUrl
                }
            };
        }

        public async Task<AuthResponseDto> RefreshTokenAsync(string token)
        {
            // Validate and refresh token logic
            // This is a simplified version
            return new AuthResponseDto
            {
                Success = true,
                Token = token
            };
        }

        public string GenerateJwtToken(int userId, string username, string role)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"] ?? "");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Role, role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}