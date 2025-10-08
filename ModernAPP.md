LegacyWpfApp/
├── LegacyWpfApp.csproj (old-style)
├── App.config
├── packages.config (ancien NuGet)
├── MainWindow.xaml
├── MainWindow.xaml.cs
├── ViewModels/
│   └── CustomerViewModel.cs
├── Services/
│   └── CustomerService.cs
└── Properties/
    ├── AssemblyInfo.cs
    └── Settings.settings

WpfModernApp/
├── Models/              # Entités métier
│   ├── Customer.cs
│   └── Order.cs
├── ViewModels/          # ViewModels MVVM
│   ├── MainViewModel.cs
│   └── CustomerViewModel.cs
├── Views/               # Vues XAML
│   ├── MainWindow.xaml
│   └── CustomerView.xaml
├── Services/            # Services métier
│   ├── ICustomerService.cs
│   └── CustomerService.cs
├── Infrastructure/      # Configuration, DI
│   ├── ServiceCollectionExtensions.cs
│   └── AppConfiguration.cs
├── Resources/           # Ressources (styles, dictionnaires)
│   └── Styles.xaml
├── App.xaml            # Application
├── App.xaml.cs
├── appsettings.json    # Configuration
└── GlobalUsings.cs     # Using globaux


MonApp.WPF/
├── appsettings.json                      # Configuration par défaut
├── appsettings.Development.json          # Configuration développement
├── appsettings.Staging.json              # Configuration staging
├── appsettings.Production.json           # Configuration production
├── Configuration/
│   ├── AppSettings.cs                    # Modèle de configuration
│   ├── DatabaseSettings.cs
│   └── ApiSettings.cs
├── Services/
│   └── ConfigurationService.cs
├── App.xaml.cs
└── MonApp.WPF.csproj


**appsettings.json** (configuration par défaut)

```json
{
  "Environment": "Development",
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "System": "Warning"
    }
  },
  "Application": {
    "Name": "Mon Application WPF",
    "Version": "1.0.0"
  },
  "Database": {
    "ConnectionString": "Server=localhost;Database=MyAppDb;Trusted_Connection=True;",
    "CommandTimeout": 30,
    "EnableSensitiveDataLogging": false
  },
  "Api": {
    "BaseUrl": "https://api.localhost:5001",
    "Timeout": 30,
    "RetryCount": 3
  },
  "Features": {
    "EnableNewUI": false,
    "EnableAnalytics": false,
    "EnableBetaFeatures": false
  },
  "Cache": {
    "ExpirationMinutes": 60,
    "MaxSize": 100
  }
}
```

**appsettings.Development.json**

```json
{
  "Environment": "Development",
  "Logging": {
    "LogLevel": {
      "Default": "Debug",
      "Microsoft": "Information"
    }
  },
  "Database": {
    "ConnectionString": "Server=localhost;Database=MyAppDb_Dev;Trusted_Connection=True;",
    "EnableSensitiveDataLogging": true
  },
  "Api": {
    "BaseUrl": "https://localhost:5001",
    "Timeout": 60
  },
  "Features": {
    "EnableNewUI": true,
    "EnableBetaFeatures": true
  }
}
```

**appsettings.Staging.json**

```json
{
  "Environment": "Staging",
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  },
  "Database": {
    "ConnectionString": "Server=staging-server;Database=MyAppDb_Staging;User Id=appuser;Password=REPLACED_BY_SECRET;",
    "EnableSensitiveDataLogging": false
  },
  "Api": {
    "BaseUrl": "https://api-staging.monapp.com",
    "Timeout": 30
  },
  "Features": {
    "EnableNewUI": true,
    "EnableAnalytics": true,
    "EnableBetaFeatures": false
  }
}
```

**appsettings.Production.json**

```json
{
  "Environment": "Production",
  "Logging": {
    "LogLevel": {
      "Default": "Warning",
      "Microsoft": "Error"
    }
  },
  "Database": {
    "ConnectionString": "REPLACED_BY_SECRET",
    "CommandTimeout": 60,
    "EnableSensitiveDataLogging": false
  },
  "Api": {
    "BaseUrl": "https://api.monapp.com",
    "Timeout": 30,
    "RetryCount": 5
  },
  "Features": {
    "EnableNewUI": true,
    "EnableAnalytics": true,
    "EnableBetaFeatures": false
  }
}
```


**Configuration/AppSettings.cs**

```csharp
namespace MonApp.WPF.Configuration;

public class AppSettings
{
    public string Environment { get; set; } = "Development";
    public LoggingSettings Logging { get; set; } = new();
    public ApplicationSettings Application { get; set; } = new();
    public DatabaseSettings Database { get; set; } = new();
    public ApiSettings Api { get; set; } = new();
    public FeaturesSettings Features { get; set; } = new();
    public CacheSettings Cache { get; set; } = new();
}

public class LoggingSettings
{
    public Dictionary<string, string> LogLevel { get; set; } = new();
}

public class ApplicationSettings
{
    public string Name { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;
}

public class DatabaseSettings
{
    public string ConnectionString { get; set; } = string.Empty;
    public int CommandTimeout { get; set; } = 30;
    public bool EnableSensitiveDataLogging { get; set; }
}

public class ApiSettings
{
    public string BaseUrl { get; set; } = string.Empty;
    public int Timeout { get; set; } = 30;
    public int RetryCount { get; set; } = 3;
    public string? ApiKey { get; set; }
}

public class FeaturesSettings
{
    public bool EnableNewUI { get; set; }
    public bool EnableAnalytics { get; set; }
    public bool EnableBetaFeatures { get; set; }
}

public class CacheSettings
{
    public int ExpirationMinutes { get; set; } = 60;
    public int MaxSize { get; set; } = 100;
}

### Enregistrement en CD

_host = Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration((context, config) =>
            {
                // Définir l'environnement
                context.HostingEnvironment.EnvironmentName = environment;

                // Construire la configuration dans l'ordre de priorité
                config
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    // 1. Configuration de base
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    // 2. Configuration spécifique à l'environnement
                    .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
                    // 3. User Secrets (uniquement en développement)
                    .AddUserSecrets<App>(optional: true, reloadOnChange: true)
                    // 4. Variables d'environnement (priorité la plus haute)
                    .AddEnvironmentVariables(prefix: "MONAPP_");
            })
            .ConfigureServices((context, services) =>
            {
                // Enregistrer la configuration complète
                services.Configure<AppSettings>(context.Configuration);

                // Enregistrer des sections spécifiques
                services.Configure<DatabaseSettings>(context.Configuration.GetSection("Database"));
                services.Configure<ApiSettings>(context.Configuration.GetSection("Api"));
                services.Configure<FeaturesSettings>(context.Configuration.GetSection("Features"));

                // Enregistrer IConfiguration pour accès direct
                services.AddSingleton(context.Configuration);

                // Enregistrer les services
                ConfigureServices(services, context.Configuration);

                // Enregistrer les fenêtres
                services.AddTransient<MainWindow>();
                services.AddTransient<MainViewModel>();
            })
            .ConfigureLogging((context, logging) =>
            {
                logging.ClearProviders();
                logging.AddConsole();
                logging.AddDebug();

                // Configurer le niveau de log selon l'environnement
                var logLevel = context.Configuration
                    .GetSection("Logging:LogLevel:Default")
                    .Get<string>();

                logging.SetMinimumLevel(logLevel switch
                {
                    "Debug" => LogLevel.Debug,
                    "Information" => LogLevel.Information,
                    "Warning" => LogLevel.Warning,
                    "Error" => LogLevel.Error,
                    _ => LogLevel.Information
                });
            })
            .Build();

```