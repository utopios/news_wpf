using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Demo.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Demo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IServiceProvider _serviceProvider;
        IConfiguration _configuration;
        readonly ILogger<MainWindow> _logger;
        public MainWindow(IServiceProvider serviceProvider, IConfiguration configuration, ILogger<MainWindow> logger)
        {
            InitializeComponent();
            DataContext = new MainViewModel();
            _serviceProvider = serviceProvider;
            _configuration = configuration;
            var applicationName = _configuration["AppSettings:ApplicationName"];
            _logger = logger;
            
        }
    }
}