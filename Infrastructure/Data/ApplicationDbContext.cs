using Microsoft.EntityFrameworkCore;

using SampleApi.Domain.Models;
using SampleApi.Infrastructure.Data.Converters;
using SampleApi.Infrastructure.Data.Extensions;


namespace SampleApi.Infrastructure.Data
{
  public class ApplicationDbContext : DbContext
  {
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      base.OnModelCreating(builder);

      builder
              .Entity<User>()
              .Property(u => u.Role)
              .HasConversion(new SnakeCaseEnumConverter<UserRole>());

      // Apply snake_case naming convention globally
      builder.UseSnakeCaseNames();

    }

    public DbSet<Employee> Employees { get; set; }
    public DbSet<User> Users { get; set; }

  }
}
