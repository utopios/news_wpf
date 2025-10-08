using WpfDIExample.Models;

namespace WpfDIExample.Services;

/// <summary>
/// Interface pour la logique métier liée aux utilisateurs
/// </summary>
public interface IUserService
{
    [LogExecution("Récupération des utilisateurs")]
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<User?> GetUserByIdAsync(int id);
    Task<User> CreateUserAsync(string firstName, string lastName, string email);
    Task<bool> UpdateUserAsync(User user);
    Task<bool> DeleteUserAsync(int id);
    Task<bool> ValidateEmailAsync(string email);
}
