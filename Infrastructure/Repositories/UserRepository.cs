using Microsoft.EntityFrameworkCore;

using SampleApi.Domain.Models;
using SampleApi.Infrastructure.Data;

namespace SampleApi.Infrastructure.Repositories
{
  public class UserRepository : IUserRepository
  {
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
      _context = context;
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
      return await _context.Users.AsNoTracking().ToListAsync();
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
      return await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<User> CreateAsync(User user)
    {
      _context.Users.Add(user);
      await _context.SaveChangesAsync();
      return user;
    }

    public async Task<User?> UpdateAsync(Guid id, User updatedUser)
    {
      var existing = await _context.Users.FindAsync(id);
      if (existing == null) return null;

      existing.Username = updatedUser.Username;
      existing.Email = updatedUser.Email;
      existing.Role = updatedUser.Role;
      existing.PasswordHash = updatedUser.PasswordHash;

      await _context.SaveChangesAsync();
      return existing;
    }

    public async Task<bool> DeleteAsync(int id)
    {
      var user = await _context.Users.FindAsync(id);
      if (user == null) return false;

      _context.Users.Remove(user);
      await _context.SaveChangesAsync();
      return true;
    }
  }
}
