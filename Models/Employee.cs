using System.ComponentModel.DataAnnotations.Schema;

namespace SampleApi.Models
{
  [Table("employees")]
  public class Employee
  {
    [Column("id")]
    public int Id { get; set; } // Primary Key

    [Column("name")]
    public string Name { get; set; } = string.Empty;

    [Column("position")]
    public string Position { get; set; } = string.Empty;

    [Column("salary")]
    public decimal Salary { get; set; }
  }
}
