using EventManagement.Data;
using EventManagement.DTOs;
using EventManagement.Models;
using EventManagement.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventManagement.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly JwtService _jwt;
        private readonly IPasswordHasherService _hasher;

        public AuthController(AppDbContext db, JwtService jwt, IPasswordHasherService hasher)
        {
            _db = db;
            _jwt = jwt;
            _hasher = hasher;
        }

        // ---------------- REGISTER ----------------
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            if (await _db.Users.AnyAsync(x => x.Email == dto.Email))
                return BadRequest("Email already exists");

            if (await _db.Users.AnyAsync(x => x.Mobile == dto.Mobile))
                return BadRequest("Mobile number already exists");

            // 🔐 Strong password validation
            if (!IsValidPassword(dto.Password))
                return BadRequest("Password must be at least 6 characters long and include uppercase, lowercase, number, and special character.");

            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                Mobile = dto.Mobile,
                Password = _hasher.HashPassword(dto.Password),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            return Ok("Registration successful");
        }

        // ==============================
        // 🔐 Password Validation Method
        // ==============================
        private bool IsValidPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
                return false;

            bool hasUpper = password.Any(char.IsUpper);
            bool hasLower = password.Any(char.IsLower);
            bool hasDigit = password.Any(char.IsDigit);
            bool hasSpecial = password.Any(ch => !char.IsLetterOrDigit(ch));

            return hasUpper && hasLower && hasDigit && hasSpecial;
        }


        // ---------------- LOGIN ----------------
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var user = await _db.Users.Include(u => u.Role)
                                      .FirstOrDefaultAsync(x => x.Email == dto.Email);

            if (user == null)
                return Unauthorized("Invalid credentials");

            // Lockout check
            if (user.LockoutEnd > DateTime.UtcNow)
                return Unauthorized("Account locked. Try later.");

            // verify hashed password
            if (!_hasher.VerifyPassword(user.Password, dto.Password))
            {
                user.FailedLoginAttempts++;

                if (user.FailedLoginAttempts >= 5)
                {
                    user.LockoutEnd = DateTime.UtcNow.AddMinutes(10);
                }

                await _db.SaveChangesAsync();
                return Unauthorized("Invalid credentials");
            }

            // Reset failed attempts
            user.FailedLoginAttempts = 0;

            string accessToken = _jwt.GenerateAccessToken(user);
            string refreshToken = _jwt.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            user.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();

            return Ok(new TokenResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            });
        }

        // ---------------- REFRESH TOKEN ----------------
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(TokenResponseDto dto)
        {
            var user = await _db.Users.FirstOrDefaultAsync(x => x.RefreshToken == dto.RefreshToken);

            if (user == null || user.RefreshTokenExpiryTime < DateTime.UtcNow)
                return Unauthorized("Invalid refresh token");

            string newAccessToken = _jwt.GenerateAccessToken(user);
            string newRefreshToken = _jwt.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            user.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();

            return Ok(new TokenResponseDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            });
        }
    }
}
