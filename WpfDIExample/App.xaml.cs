using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Windows;
using WpfDIExample.Repositories;
using WpfDIExample.Services;
using WpfDIExample.ViewModels;
using WpfDIExample.Views;

namespace WpfDIExample;

/// <summary>
/// Classe principale de l'application avec configuration de l'injection de dépendances
/// </summary>
public partial class App : Application
{
    private IHost? _host;

    /// <summary>
    /// Point d'entrée de l'application - Configuration du Host et de l'injection de dépendances
    /// </summary>
    /// 
    
    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // Construction du Host avec tous les services
        _host = Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration((context, config) =>
            {
                // Configuration depuis appsettings.json
                config.SetBasePath(Directory.GetCurrentDirectory());
                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                config.AddEnvironmentVariables();
            })
            .ConfigureServices((context, services) =>
            {
                // ===== CONFIGURATION DE L'INJECTION DE DÉPENDANCES =====

                // 1. Enregistrement de la configuration
                services.AddSingleton<IConfiguration>(context.Configuration);

                services.AddSingleton<IApplicationInfoService, ApplicationInfoService>();

                // 2. Configuration du logging
                services.AddLogging(builder =>
                {
                    builder.AddConsole();
                    builder.AddDebug();
                    builder.SetMinimumLevel(LogLevel.Information);
                });

                // 3. Enregistrement des repositories (Scoped - une instance par scope/requête)
                services.AddScoped<IUserRepository, UserRepository>();

                // 4. Enregistrement des services métier (Scoped)
                services.AddWithLogging<IUserService, UserService>();

                // 5. Enregistrement du service de navigation (Singleton - une seule instance pour toute l'app)
                services.AddSingleton<INavigationService, NavigationService>();

                // 6. Enregistrement des ViewModels (Transient - nouvelle instance à chaque demande)
                services.AddTransient<MainViewModel>();
                services.AddTransient<UserListViewModel>();

                // 7. Enregistrement des Views (Transient)
                services.AddTransient<MainWindow>();
                services.AddTransient<UserListView>();

                // Note: Vous pouvez aussi enregistrer des services avec des durées de vie différentes:
                // - Singleton: Une seule instance pour toute la durée de vie de l'application
                // - Scoped: Une instance par scope (utile pour les opérations avec état)
                // - Transient: Une nouvelle instance à chaque fois qu'elle est demandée
            })
            .Build();

        // Démarrage du host
        await _host.StartAsync();

        // Résolution et affichage de la fenêtre principale
        var mainWindow = _host.Services.GetRequiredService<MainWindow>();
        mainWindow.Show();

        // Log du démarrage réussi
        var logger = _host.Services.GetRequiredService<ILogger<App>>();
        logger.LogInformation("Application démarrée avec succès");
    }

    /// <summary>
    /// Nettoyage lors de la fermeture de l'application
    /// </summary>
    protected override async void OnExit(ExitEventArgs e)
    {
        if (_host != null)
        {
            var logger = _host.Services.GetRequiredService<ILogger<App>>();
            logger.LogInformation("Arrêt de l'application");

            await _host.StopAsync();
            _host.Dispose();
        }

        base.OnExit(e);
    }
}
