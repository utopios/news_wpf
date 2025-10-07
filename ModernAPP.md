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