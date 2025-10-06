using System.Windows;
using LibraryManager.ViewModels;

namespace LibraryManager
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }
    }
}
