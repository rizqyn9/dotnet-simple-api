using SampleApi.Application.DTOs;
using SampleApi.Domain.Models;
using SampleApi.Infrastructure.Repositories;


namespace SampleApi.Application.Services
{
  public interface IUserService
  {
    Task<IEnumerable<UserDto>> GetAllAsync();
    Task<UserDto?> GetByIdAsync(int id);
    Task<UserDto> CreateAsync(CreateUserDto dto);
    Task<UserDto?> UpdateAsync(int id, CreateUserDto dto);
    Task<bool> DeleteAsync(int id);
  }

  public class UserService : IUserService
  {
    private readonly IUserRepository _repo;

    public UserService(IUserRepository repo)
    {
      _repo = repo;
    }

    public async Task<IEnumerable<UserDto>> GetAllAsync()
    {
      var users = await _repo.GetAllAsync();
      return users.Select(u => new UserDto
      {
        Username = u.Username,
        Email = u.Email,
        Role = u.Role.ToString().ToLower()
      });
    }

    public async Task<UserDto?> GetByIdAsync(int id)
    {
      var user = await _repo.GetByIdAsync(id);
      if (user == null) return null;

      return new UserDto
      {
        Username = user.Username,
        Email = user.Email,
        Role = user.Role.ToString().ToLower()
      };
    }

    public async Task<UserDto> CreateAsync(CreateUserDto dto)
    {
      var passwordHash = HashPassword(dto.Password);
      var user = new User
      {
        Username = dto.Username,
        Email = dto.Email,
        PasswordHash = passwordHash,
        Role = Enum.TryParse<UserRole>(dto.Role, true, out var role) ? role : UserRole.User
      };

      var created = await _repo.CreateAsync(user);
      return new UserDto
      {
        Username = created.Username,
        Email = created.Email,
        Role = created.Role.ToString().ToLower()
      };
    }

    public async Task<UserDto?> UpdateAsync(int id, CreateUserDto dto)
    {
      var passwordHash = HashPassword(dto.Password);
      var updated = new User
      {
        Username = dto.Username,
        Email = dto.Email,
        PasswordHash = passwordHash,
        Role = Enum.TryParse<UserRole>(dto.Role, true, out var role) ? role : UserRole.User
      };

      var result = await _repo.UpdateAsync(id, updated);
      if (result == null) return null;

      return new UserDto
      {
        Username = result.Username,
        Email = result.Email,
        Role = result.Role.ToString().ToLower()
      };
    }

    public Task<bool> DeleteAsync(int id) => _repo.DeleteAsync(id);

    private static string HashPassword(string password)
    {
      using var sha = System.Security.Cryptography.SHA256.Create();
      var bytes = sha.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
      return Convert.ToHexString(bytes);
    }
  }
}
