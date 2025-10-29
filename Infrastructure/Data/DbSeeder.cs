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
          RoleId = Guid.Parse("f6a3e1d0-b9e0-4e2d-a5f3-e3f4f7f0f5d1")
        };

        var normalUser = new User
        {
          Username = "johndoe",
          Email = "john@example.com",
          PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
          RoleId = Guid.Parse("f6a3e1d0-b9e0-4e2d-a5f3-e3f4f7f0f5d1")
        };

        await context.Users.AddRangeAsync(adminUser, normalUser);
        await context.SaveChangesAsync();
      }
    }
  }
}
