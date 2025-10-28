using System.Security.Cryptography;
using System.Text;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using SampleApi.Application.DTOs;
using SampleApi.Domain.Models;
using SampleApi.Infrastructure.Repositories;
using SampleApi.Presentation.Common.Responses;


namespace SampleApi.Controllers.V2
{
  [ApiController]
  [ApiVersion("2.0")]
  [Route("api/v{version:apiVersion}/[controller]")]
  public class UserControllerV2 : ControllerBase
  {
    private readonly IUserRepository _repo;

    public UserControllerV2(IUserRepository repo)
    {
      _repo = repo;
    }

    // ✅ Authenticated user endpoint
    [Authorize]
    [HttpGet("me")]
    public IActionResult GetCurrentUser()
    {
      var username = User.Claims.FirstOrDefault(c => c.Type == "username")?.Value;
      var role = User.Claims.FirstOrDefault(c => c.Type == "role")?.Value;

      return Ok(new
      {
        username,
        role,
        message = "Authenticated user info retrieved successfully."
      });
    }

    // ✅ Admin-only endpoint
    [Authorize(Roles = "admin")]
    [HttpGet("all")]
    public async Task<IActionResult> GetAllUsers()
    {
      var users = await _repo.GetAllAsync();

      var dto = users.Select(u => new UserDto
      {
        Username = u.Username,
        Email = u.Email,
        Role = u.Role.ToString().ToLower()
      });

      return Ok(dto);
    }

    // ✅ Public endpoint (for dev/testing)
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
      var users = await _repo.GetAllAsync();
      var dto = users.Select(u => new UserDto
      {
        Username = u.Username,
        Email = u.Email,
        Role = u.Role.ToString().ToLower()
      });

      return Ok(dto);
    }

    // ✅ Get by ID
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
      var user = await _repo.GetByIdAsync(id);
      if (user == null)
        return NotFound(ApiResponse<string>.Fail("User not found."));

      var dto = new UserDto
      {
        Username = user.Username,
        Email = user.Email,
        Role = user.Role.ToString().ToLower()
      };

      return Ok(ApiResponse<UserDto>.SuccessResponse(dto, "User retrieved successfully."));
    }

    // ✅ Create user
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserDto dto)
    {
      if (string.IsNullOrWhiteSpace(dto.Password))
        return BadRequest("Password is required.");

      var passwordHash = HashPassword(dto.Password);

      var user = new User
      {
        Username = dto.Username,
        Email = dto.Email,
        PasswordHash = passwordHash,
        Role = Enum.TryParse<UserRole>(dto.Role, true, out var role)
              ? role
              : UserRole.User
      };

      var created = await _repo.CreateAsync(user);
      return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    // ✅ Update user
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] CreateUserDto dto)
    {
      var passwordHash = HashPassword(dto.Password);

      var updatedUser = new User
      {
        Username = dto.Username,
        Email = dto.Email,
        PasswordHash = passwordHash,
        Role = Enum.TryParse<UserRole>(dto.Role, true, out var role)
              ? role
              : UserRole.User
      };

      var result = await _repo.UpdateAsync(id, updatedUser);
      if (result == null) return NotFound();

      return Ok(result);
    }

    // ✅ Delete user
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
      var success = await _repo.DeleteAsync(id);
      if (!success) return NotFound();
      return NoContent();
    }

    // 🧠 Utility: Hash password
    private static string HashPassword(string password)
    {
      using var sha = SHA256.Create();
      var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
      return Convert.ToHexString(bytes);
    }
  }
}
