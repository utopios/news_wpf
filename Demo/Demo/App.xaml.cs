using System.Configuration;
using System.Data;
using System.IO;
using System.Windows;
using System.Windows.Navigation;
using Demo.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
namespace Demo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    /// 
    public partial class App : Application
    {
        private IHost? host;
        protected async override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            host = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.SetBasePath(Directory.GetCurrentDirectory());
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                    config.AddEnvironmentVariables();
                })
                .UseSerilog((context, services, configuration) => {
                    configuration.MinimumLevel.Debug()
                    .Enrich.FromLogContext()
                    .WriteTo.File(
                        path: Path.Combine("ap-.log"),
                        rollingInterval: RollingInterval.Day,
                            retainedFileCountLimit: 30,
                            outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] [{SourceContext}] {Message:lj}{NewLine}{Exception}"
                        );
                })
                .ConfigureServices((context, services) =>
            {
                //services.AddLogging(configure =>
                //{
                //    configure.AddConsole();
                //    configure.AddDebug();
                //    configure.SetMinimumLevel(LogLevel.Error);
                    
                //});
                

                
                services.AddSingleton<IConfiguration>(context.Configuration);
                
                //services.AddScoped<IRepository, ServiceRepository>();

                //services.AddSingleton<INavigationService, NavigationService>();

                services.AddTransient<MainWindow>();

            
            }).Build();

            DispatcherUnhandledException += App_DispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;

            await host.StartAsync();

            var mainWindow = host.Services.GetRequiredService<MainWindow>();
            mainWindow.Show();

            
        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {

        }

        private void TaskScheduler_UnobservedTaskException(
        object sender, 
        UnobservedTaskExceptionEventArgs e)
        {

        }
    }
}

