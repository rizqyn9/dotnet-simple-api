using SampleApi.Models;

namespace SampleApi.Repositories
{
  public interface IUserRepository
  {
    Task<IEnumerable<User>> GetAllAsync();
    Task<User?> GetByIdAsync(int id);
    Task<User> CreateAsync(User user);
    Task<User?> UpdateAsync(int id, User user);
    Task<bool> DeleteAsync(int id);
  }
}
