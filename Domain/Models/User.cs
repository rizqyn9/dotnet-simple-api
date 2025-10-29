namespace SampleApi.Domain.Models
{
  public class User
  {
    public Guid Id { get; set; }
    public string Username { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string PasswordHash { get; set; } = default!;

    // ✅ One-to-One (User → Employee)
    public Guid EmployeeId { get; set; }
    public Employee Employee { get; set; } = default!;

    // ✅ Many-to-One (User → UserRole)
    public Guid RoleId { get; set; }
    public UserRole Role { get; set; } = default!;
  }
}
