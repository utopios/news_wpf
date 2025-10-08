using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Windows.Controls;

namespace WpfDIExample.Services;

/// <summary>
/// Implémentation du service de navigation
/// </summary>
public class NavigationService : INavigationService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<NavigationService> _logger;
    private UserControl? _currentView;

    public UserControl? CurrentView
    {
        get => _currentView;
        private set
        {
            _currentView = value;
            CurrentViewChanged?.Invoke(this, _currentView!);
        }
    }

    public event EventHandler<UserControl>? CurrentViewChanged;

    public NavigationService(IServiceProvider serviceProvider, ILogger<NavigationService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public void NavigateTo<TView>() where TView : UserControl
    {
        NavigateTo(typeof(TView));
    }

    public void NavigateTo(Type viewType)
    {
        if (!typeof(UserControl).IsAssignableFrom(viewType))
        {
            _logger.LogError("Le type {ViewType} n'est pas un UserControl", viewType.Name);
            throw new ArgumentException($"Le type {viewType.Name} doit hériter de UserControl");
        }

        _logger.LogInformation("Navigation vers {ViewType}", viewType.Name);

        // Résoudre la vue depuis le conteneur DI
        var view = _serviceProvider.GetRequiredService(viewType) as UserControl;
        CurrentView = view;
    }
}
