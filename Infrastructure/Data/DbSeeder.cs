using Microsoft.EntityFrameworkCore;

using SampleApi.Domain.Models;


namespace SampleApi.Infrastructure.Data
{
  public static class DbSeeder
  {
    public static async Task SeedAsync(ApplicationDbContext context)
    {
      // Apply pending migrations (if any)
      await context.Database.MigrateAsync();

      // Seed Users
      if (!await context.Users.AnyAsync())
      {
        var adminUser = new User
        {
          Username = "admin",
          Email = "admin@example.com",
          PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"), // hash for security
          Role = UserRole.Admin
        };

        var normalUser = new User
        {
          Username = "johndoe",
          Email = "john@example.com",
          PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
          Role = UserRole.User
        };

        await context.Users.AddRangeAsync(adminUser, normalUser);
        await context.SaveChangesAsync();
      }
    }
  }
}
