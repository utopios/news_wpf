using System.ComponentModel;
using System.Runtime.CompilerServices;

public abstract class BaseViewModel : INotifyPropertyChanged
{
    protected readonly ILogger _logger;

    private string _errorMessage;
    private bool _hasError;
    private bool _isBusy;

    protected BaseViewModel(ILogger logger)
    {
        _logger = logger;
    }

    // Propriétés d'état
    public string ErrorMessage
    {
        get => _errorMessage;
        set => SetProperty(ref _errorMessage, value);
    }

    public bool HasError
    {
        get => _hasError;
        set => SetProperty(ref _hasError, value);
    }

    public bool IsBusy
    {
        get => _isBusy;
        set => SetProperty(ref _isBusy, value);
    }

    // Méthode utilitaire pour exécuter des opérations avec gestion d'erreurs
    protected async Task ExecuterAvecGestionErreurs(
        Func<Task> operation, 
        string nomOperation)
    {
        try
        {
            IsBusy = true;
            HasError = false;
            ErrorMessage = null;

            _logger.LogInformation("Début de l'opération: {Operation}", nomOperation);
            
            await operation();
            
            _logger.LogInformation("Opération réussie: {Operation}", nomOperation);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur lors de {Operation}: {Message}", 
                nomOperation, ex.Message);
            
            HasError = true;
            ErrorMessage = ObtenirMessageUtilisateur(ex);
        }
        finally
        {
            IsBusy = false;
        }
    }

    protected string ObtenirMessageUtilisateur(Exception ex)
    {
        return ex switch
        {
            ValidationException => ex.Message,
            UnauthorizedAccessException => "Vous n'avez pas les droits nécessaires.",
            HttpRequestException => "Erreur de connexion au serveur.",
            _ => "Une erreur inattendue s'est produite."
        };
    }

    // INotifyPropertyChanged
    public event PropertyChangedEventHandler PropertyChanged;

    protected bool SetProperty<T>(
        ref T field, 
        T value, 
        [CallerMemberName] string propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
            return false;

        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}