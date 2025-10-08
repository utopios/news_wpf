using System.Windows.Input;
using Microsoft.Extensions.Logging;

public class AsyncRelayCommand : ICommand
{
    private readonly Func<Task> _execute;
    private readonly Func<bool> _canExecute;
    private readonly ILogger _logger;
    private readonly Action<Exception> _errorHandler;
    private bool _isExecuting;

    public AsyncRelayCommand(
        Func<Task> execute,
        Func<bool> canExecute,
        ILogger logger,
        Action<Exception> errorHandler = null)
    {
        _execute = execute;
        _canExecute = canExecute;
        _logger = logger;
        _errorHandler = errorHandler;
    }

    public event EventHandler CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    public bool CanExecute(object parameter)
    {
        return !_isExecuting && (_canExecute?.Invoke() ?? true);
    }

    public async void Execute(object parameter)
    {
        if (!CanExecute(parameter))
            return;

        _isExecuting = true;
        RaiseCanExecuteChanged();

        try
        {
            await _execute();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur lors de l'exécution de la commande");
            
            // Appeler le gestionnaire d'erreur personnalisé
            _errorHandler?.Invoke(ex);
        }
        finally
        {
            _isExecuting = false;
            RaiseCanExecuteChanged();
        }
    }

    public void RaiseCanExecuteChanged()
    {
        CommandManager.InvalidateRequerySuggested();
    }
}