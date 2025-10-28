namespace SampleApi.Application.DTOs
{
  public class UserDto
  {
    public string Username { get; set; } = "";
    public string Email { get; set; } = "";
    public string Role { get; set; } = "";
  }

  public class CreateUserDto
  {
    public string Username { get; set; } = "";
    public string Email { get; set; } = "";
    public string Password { get; set; } = "";
    public string Role { get; set; } = "user";
  }
}
