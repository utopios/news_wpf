using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using WpfDIExample.Models;
using WpfDIExample.Services;

namespace WpfDIExample.ViewModels;

/// <summary>
/// ViewModel pour la liste des utilisateurs
/// </summary>
public class UserListViewModel : ViewModelBase
{
    private readonly IUserService _userService;
    private readonly ILogger<UserListViewModel> _logger;
    private bool _isLoading;
    private string _statusMessage = string.Empty;
    private User? _selectedUser;
    
    // Propriétés pour le formulaire d'ajout
    private string _newFirstName = string.Empty;
    private string _newLastName = string.Empty;
    private string _newEmail = string.Empty;

    public ObservableCollection<User> Users { get; } = new();

    public bool IsLoading
    {
        get => _isLoading;
        set => SetProperty(ref _isLoading, value);
    }

    public string StatusMessage
    {
        get => _statusMessage;
        set => SetProperty(ref _statusMessage, value);
    }

    public User? SelectedUser
    {
        get => _selectedUser;
        set => SetProperty(ref _selectedUser, value);
    }

    public string NewFirstName
    {
        get => _newFirstName;
        set => SetProperty(ref _newFirstName, value);
    }

    public string NewLastName
    {
        get => _newLastName;
        set => SetProperty(ref _newLastName, value);
    }

    public string NewEmail
    {
        get => _newEmail;
        set => SetProperty(ref _newEmail, value);
    }

    // Commandes
    public ICommand LoadUsersCommand { get; }
    public ICommand AddUserCommand { get; }
    public ICommand DeleteUserCommand { get; }
    public ICommand RefreshCommand { get; }

    public UserListViewModel(IUserService userService, ILogger<UserListViewModel> logger)
    {
        _userService = userService;
        _logger = logger;

        // Initialisation des commandes avec RelayCommand (implémentation simple)
        LoadUsersCommand = new RelayCommand(async () => await LoadUsersAsync());
        AddUserCommand = new RelayCommand(async () => await AddUserAsync(), CanAddUser);
        DeleteUserCommand = new RelayCommand(async () => await DeleteUserAsync(), () => SelectedUser != null);
        RefreshCommand = new RelayCommand(async () => await LoadUsersAsync());

        _logger.LogInformation("UserListViewModel initialisé");

        Application.Current.Dispatcher.InvokeAsync(async () => await LoadUsersAsync());
    }

    private async Task LoadUsersAsync()
    {
        try
        {
            IsLoading = true;
            StatusMessage = "Chargement des utilisateurs...";
            _logger.LogInformation("Chargement de la liste des utilisateurs");

            var users = await _userService.GetAllUsersAsync();
            
            Users.Clear();
            foreach (var user in users)
            {
                Users.Add(user);
            }

            StatusMessage = $"{Users.Count} utilisateur(s) chargé(s)";
            _logger.LogInformation("{Count} utilisateurs chargés avec succès", Users.Count);
        }
        catch (Exception ex)
        {
            StatusMessage = $"Erreur: {ex.Message}";
            _logger.LogError(ex, "Erreur lors du chargement des utilisateurs");
            MessageBox.Show($"Erreur lors du chargement: {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            IsLoading = false;
        }
    }

    private async Task AddUserAsync()
    {
        try
        {
            IsLoading = true;
            StatusMessage = "Ajout de l'utilisateur...";
            _logger.LogInformation("Tentative d'ajout d'un utilisateur: {FirstName} {LastName}", NewFirstName, NewLastName);

            var newUser = await _userService.CreateUserAsync(NewFirstName, NewLastName, NewEmail);
            Users.Add(newUser);

            // Réinitialisation du formulaire
            NewFirstName = string.Empty;
            NewLastName = string.Empty;
            NewEmail = string.Empty;

            StatusMessage = $"Utilisateur {newUser.FullName} ajouté avec succès";
            _logger.LogInformation("Utilisateur ajouté: {UserId}", newUser.Id);
            
            MessageBox.Show($"Utilisateur {newUser.FullName} ajouté avec succès!", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (ArgumentException ex)
        {
            StatusMessage = $"Validation échouée: {ex.Message}";
            _logger.LogWarning("Validation échouée lors de l'ajout: {Message}", ex.Message);
            MessageBox.Show(ex.Message, "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
        catch (Exception ex)
        {
            StatusMessage = $"Erreur: {ex.Message}";
            _logger.LogError(ex, "Erreur lors de l'ajout d'un utilisateur");
            MessageBox.Show($"Erreur: {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            IsLoading = false;
        }
    }

    private async Task DeleteUserAsync()
    {
        if (SelectedUser == null) return;

        var result = MessageBox.Show(
            $"Êtes-vous sûr de vouloir supprimer {SelectedUser.FullName}?",
            "Confirmation",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (result != MessageBoxResult.Yes) return;

        try
        {
            IsLoading = true;
            StatusMessage = "Suppression de l'utilisateur...";
            _logger.LogInformation("Tentative de suppression de l'utilisateur: {UserId}", SelectedUser.Id);

            var success = await _userService.DeleteUserAsync(SelectedUser.Id);
            
            if (success)
            {
                Users.Remove(SelectedUser);
                StatusMessage = "Utilisateur supprimé avec succès";
                _logger.LogInformation("Utilisateur supprimé: {UserId}", SelectedUser.Id);
            }
            else
            {
                StatusMessage = "Échec de la suppression";
                _logger.LogWarning("Échec de la suppression de l'utilisateur: {UserId}", SelectedUser.Id);
            }
        }
        catch (Exception ex)
        {
            StatusMessage = $"Erreur: {ex.Message}";
            _logger.LogError(ex, "Erreur lors de la suppression");
            MessageBox.Show($"Erreur: {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            IsLoading = false;
        }
    }

    private bool CanAddUser()
    {
        return !string.IsNullOrWhiteSpace(NewFirstName) &&
               !string.IsNullOrWhiteSpace(NewLastName) &&
               !string.IsNullOrWhiteSpace(NewEmail);
    }
}

/// <summary>
/// Implémentation simple de ICommand pour les commandes
/// </summary>
public class RelayCommand : ICommand
{
    private readonly Func<Task> _execute;
    private readonly Func<bool>? _canExecute;

    public event EventHandler? CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    public RelayCommand(Func<Task> execute, Func<bool>? canExecute = null)
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = canExecute;
    }

    public bool CanExecute(object? parameter) => _canExecute?.Invoke() ?? true;

    public async void Execute(object? parameter) => await _execute();
}
