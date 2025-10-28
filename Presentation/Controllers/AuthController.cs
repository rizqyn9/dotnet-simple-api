using Microsoft.AspNetCore.Mvc;

using SampleApi.Application.DTOs;
using SampleApi.Application.Services;

namespace SampleApi.Presentation.Controllers
{
  [ApiController]
  [ApiVersion("1.0")]
  [Route("api/v{version:apiVersion}/[controller]")]
  public class AuthController : ControllerBase
  {
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
      _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
      var result = await _authService.LoginAsync(dto.Email, dto.Password);
      if (result == null) return Unauthorized("Invalid credentials.");
      return Ok(result);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] CreateUserDto dto)
    {
      var result = await _authService.RegisterAsync(dto.Username, dto.Email, dto.Password, dto.Role);
      if (result == null) return Conflict("Email already registered.");
      return Ok(result);
    }
  }
}
