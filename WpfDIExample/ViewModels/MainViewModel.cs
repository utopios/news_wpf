using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Windows.Controls;
using System.Windows.Input;
using WpfDIExample.Services;
using WpfDIExample.Views;

namespace WpfDIExample.ViewModels;

/// <summary>
/// ViewModel principal de l'application
/// </summary>
public class MainViewModel : ViewModelBase
{
    private readonly INavigationService _navigationService;
    private readonly IConfiguration _configuration;
    private readonly ILogger<MainViewModel> _logger;
    private UserControl? _currentView;
    private readonly IApplicationInfoService _appInfo;


    public string ApplicationTitle { get; }

    public UserControl? CurrentView
    {
        get => _currentView;
        set => SetProperty(ref _currentView, value);
    }

    // Commandes de navigation
    public ICommand NavigateToUserListCommand { get; }

    public MainViewModel(
        INavigationService navigationService,
        IConfiguration configuration,
        ILogger<MainViewModel> logger,
        IApplicationInfoService appInfo)
    {
        _navigationService = navigationService;
        _configuration = configuration;
        _logger = logger;

        // Récupération du titre depuis la configuration
        ApplicationTitle = _configuration["AppSettings:ApplicationName"] ?? "WPF DI Example";

        // Initialisation des commandes
        NavigateToUserListCommand = new RelayCommand(async () =>
        {
            Application.Current.Dispatcher.InvokeAsync(() =>
            {
                _navigationService.NavigateTo<UserListView>();
            });
        });

        // Abonnement aux changements de vue
        _navigationService.CurrentViewChanged += OnCurrentViewChanged;

        _logger.LogInformation("MainViewModel initialisé avec le titre: {Title}", ApplicationTitle);

        // Navigation initiale

        _appInfo = appInfo;
        _navigationService.NavigateTo<UserListView>();
        
    }

    private void OnCurrentViewChanged(object? sender, UserControl view)
    {
        CurrentView = view;
        _logger.LogInformation("Vue changée vers: {ViewType}", view.GetType().Name);
    }
}
