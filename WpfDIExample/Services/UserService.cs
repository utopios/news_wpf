using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;
using WpfDIExample.Models;
using WpfDIExample.Repositories;

namespace WpfDIExample.Services;

/// <summary>
/// Service contenant la logique métier pour les utilisateurs
/// </summary>
public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<UserService> _logger;
    private readonly IConfiguration _configuration;
    public UserService(IUserRepository userRepository, ILogger<UserService> logger, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _logger = logger;
        _configuration = configuration;
        var apiKey = _configuration["Api:ApiKey"];
    }


    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        _logger.LogInformation("Service: Récupération de tous les utilisateurs");
        return await _userRepository.GetAllUsersAsync();
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        _logger.LogInformation("Service: Récupération de l'utilisateur {Id}", id);
        return await _userRepository.GetUserByIdAsync(id);
    }

    public async Task<User> CreateUserAsync(string firstName, string lastName, string email)
    {
        // Validation métier
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("Le prénom est requis", nameof(firstName));
        
        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Le nom est requis", nameof(lastName));
        
        if (!await ValidateEmailAsync(email))
            throw new ArgumentException("L'email n'est pas valide", nameof(email));

        var user = new User
        {
            FirstName = firstName.Trim(),
            LastName = lastName.Trim(),
            Email = email.Trim().ToLower()
        };

        _logger.LogInformation("Service: Création d'un nouvel utilisateur {FullName}", user.FullName);
        return await _userRepository.AddUserAsync(user);
    }

    public async Task<bool> UpdateUserAsync(User user)
    {
        if (user == null)
            throw new ArgumentNullException(nameof(user));

        if (!await ValidateEmailAsync(user.Email))
            throw new ArgumentException("L'email n'est pas valide");

        _logger.LogInformation("Service: Mise à jour de l'utilisateur {Id}", user.Id);
        return await _userRepository.UpdateUserAsync(user);
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        _logger.LogInformation("Service: Suppression de l'utilisateur {Id}", id);
        return await _userRepository.DeleteUserAsync(id);
    }

    public Task<bool> ValidateEmailAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return Task.FromResult(false);

        // Validation simple de l'email avec regex
        var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        var isValid = emailRegex.IsMatch(email);
        
        _logger.LogDebug("Validation de l'email {Email}: {IsValid}", email, isValid);
        return Task.FromResult(isValid);
    }
}
