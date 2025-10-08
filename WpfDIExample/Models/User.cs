namespace WpfDIExample.Models;

/// <summary>
/// Modèle représentant un utilisateur
/// </summary>
public class User
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    
    public string FullName => $"{FirstName} {LastName}";
}
