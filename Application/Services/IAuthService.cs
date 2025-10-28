using SampleApi.Application.DTOs;

namespace SampleApi.Application.Services
{
  public interface IAuthService
  {
    Task<AuthResponseDto?> LoginAsync(string email, string password);
    Task<AuthResponseDto?> RegisterAsync(string username, string email, string password, string role);
  }
}
