using WpfDIExample.Models;

namespace WpfDIExample.Repositories;

/// <summary>
/// Interface pour l'accès aux données des utilisateurs
/// </summary>
public interface IUserRepository
{
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<User?> GetUserByIdAsync(int id);
    Task<User> AddUserAsync(User user);
    Task<bool> UpdateUserAsync(User user);
    Task<bool> DeleteUserAsync(int id);
}
