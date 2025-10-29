namespace SampleApi.Domain.Models
{
  public class Employee
  {
    public Guid Id { get; set; }
    public string FullName { get; set; } = default!;
    public DateTime HireDate { get; set; }

    public User? User { get; set; }
  }
}
