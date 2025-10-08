using Microsoft.Extensions.Logging;
using WpfDIExample.Models;

namespace WpfDIExample.Repositories;

/// <summary>
/// Implémentation du repository pour les utilisateurs
/// Dans un vrai projet, ceci communiquerait avec une base de données
/// </summary>
public class UserRepository : IUserRepository
{
    private readonly ILogger<UserRepository> _logger;
    private readonly List<User> _users; // Simulation d'une base de données en mémoire
    private int _nextId = 1;

    public UserRepository(ILogger<UserRepository> logger)
    {
        _logger = logger;
        _users = new List<User>();
        
        // Données de test
        SeedData();
    }

    private void SeedData()
    {
        _users.AddRange(new[]
        {
            new User 
            { 
                Id = _nextId++, 
                FirstName = "Jean", 
                LastName = "Dupont", 
                Email = "jean.dupont@example.com",
                CreatedAt = DateTime.Now.AddDays(-30)
            },
            new User 
            { 
                Id = _nextId++, 
                FirstName = "Marie", 
                LastName = "Martin", 
                Email = "marie.martin@example.com",
                CreatedAt = DateTime.Now.AddDays(-20)
            },
            new User 
            { 
                Id = _nextId++, 
                FirstName = "Pierre", 
                LastName = "Durand", 
                Email = "pierre.durand@example.com",
                CreatedAt = DateTime.Now.AddDays(-10)
            }
        });
        
        _logger.LogInformation("Repository initialisé avec {Count} utilisateurs", _users.Count);
    }

    public Task<IEnumerable<User>> GetAllUsersAsync()
    {
        _logger.LogInformation("Récupération de tous les utilisateurs");
        return Task.FromResult<IEnumerable<User>>(_users.ToList());
    }

    public Task<User?> GetUserByIdAsync(int id)
    {
        _logger.LogInformation("Récupération de l'utilisateur avec l'ID: {Id}", id);
        var user = _users.FirstOrDefault(u => u.Id == id);
        return Task.FromResult(user);
    }

    public Task<User> AddUserAsync(User user)
    {
        user.Id = _nextId++;
        user.CreatedAt = DateTime.Now;
        _users.Add(user);
        
        _logger.LogInformation("Utilisateur ajouté: {FullName} (ID: {Id})", user.FullName, user.Id);
        return Task.FromResult(user);
    }

    public Task<bool> UpdateUserAsync(User user)
    {
        var existingUser = _users.FirstOrDefault(u => u.Id == user.Id);
        if (existingUser == null)
        {
            _logger.LogWarning("Tentative de mise à jour d'un utilisateur inexistant: {Id}", user.Id);
            return Task.FromResult(false);
        }

        existingUser.FirstName = user.FirstName;
        existingUser.LastName = user.LastName;
        existingUser.Email = user.Email;
        
        _logger.LogInformation("Utilisateur mis à jour: {FullName} (ID: {Id})", user.FullName, user.Id);
        return Task.FromResult(true);
    }

    public Task<bool> DeleteUserAsync(int id)
    {
        var user = _users.FirstOrDefault(u => u.Id == id);
        if (user == null)
        {
            _logger.LogWarning("Tentative de suppression d'un utilisateur inexistant: {Id}", id);
            return Task.FromResult(false);
        }

        _users.Remove(user);
        _logger.LogInformation("Utilisateur supprimé: {FullName} (ID: {Id})", user.FullName, id);
        return Task.FromResult(true);
    }
}
