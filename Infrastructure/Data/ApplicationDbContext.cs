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

      // One-to-One: User ↔ Employee
      builder.Entity<User>()
          .HasOne(u => u.Employee)
          .WithOne(e => e.User)
          .HasForeignKey<User>(u => u.EmployeeId)
          .OnDelete(DeleteBehavior.Cascade);

      // Many-to-One: User → Role
      builder.Entity<User>()
          .HasOne(u => u.Role)
          .WithMany(r => r.Users)
          .HasForeignKey(u => u.RoleId)
          .OnDelete(DeleteBehavior.Restrict);

      // Apply snake_case naming convention globally
      builder.UseSnakeCaseNames();
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();

  }
}
