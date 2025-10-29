namespace SampleApi.Domain.Models
{
  public class UserRole
  {
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;

    public ICollection<User> Users { get; set; } = [];
  }
}
