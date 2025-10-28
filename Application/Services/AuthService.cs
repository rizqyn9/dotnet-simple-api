using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using SampleApi.Application.DTOs;
using SampleApi.Domain.Models;
using SampleApi.Infrastructure.Data;

namespace SampleApi.Application.Services
{
  public class AuthService : IAuthService
  {
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _config;

    public AuthService(ApplicationDbContext context, IConfiguration config)
    {
      _context = context;
      _config = config;
    }

    public async Task<AuthResponseDto?> LoginAsync(string email, string password)
    {
      var hashed = HashPassword(password);
      var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.PasswordHash == hashed);

      if (user == null) return null;

      return GenerateToken(user);
    }

    public async Task<AuthResponseDto?> RegisterAsync(string username, string email, string password, string role)
    {
      if (await _context.Users.AnyAsync(u => u.Email == email))
        return null;

      var hashed = HashPassword(password);
      var newUser = new User
      {
        Username = username,
        Email = email,
        PasswordHash = hashed,
        Role = Enum.TryParse<UserRole>(role, true, out var parsedRole) ? parsedRole : UserRole.User
      };

      _context.Users.Add(newUser);
      await _context.SaveChangesAsync();

      return GenerateToken(newUser);
    }

    private AuthResponseDto GenerateToken(User user)
    {
      var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
      var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

      var claims = new[]
      {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim("role", user.Role.ToString().ToLower()),
                new Claim("username", user.Username)
            };

      var token = new JwtSecurityToken(
          issuer: _config["Jwt:Issuer"],
          audience: _config["Jwt:Audience"],
          claims: claims,
          expires: DateTime.UtcNow.AddHours(2),
          signingCredentials: creds
      );

      return new AuthResponseDto
      {
        Token = new JwtSecurityTokenHandler().WriteToken(token),
        Username = user.Username,
        Role = user.Role.ToString().ToLower()
      };
    }

    private static string HashPassword(string password)
    {
      using var sha = SHA256.Create();
      var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
      return Convert.ToHexString(bytes);
    }
  }
}
