using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
                .ConfigureServices((context, services) =>
            {
                
                //services.AddScoped<IRepository, ServiceRepository>();

                //services.AddSingleton<INavigationService, NavigationService>();

                services.AddTransient<MainWindow>();

            
            }).Build();

            await host.StartAsync();

            var mainWindow = host.Services.GetService<MainWindow>();
            mainWindow.Show();
        }
    }

}
