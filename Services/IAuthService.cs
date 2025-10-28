using SampleApi.DTOs;

namespace SampleApi.Services
{
  public interface IAuthService
  {
    Task<AuthResponseDto?> LoginAsync(string email, string password);
    Task<AuthResponseDto?> RegisterAsync(string username, string email, string password, string role);
  }
}
