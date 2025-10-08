using System.Windows.Controls;
using WpfDIExample.ViewModels;

namespace WpfDIExample.Views;

/// <summary>
/// Logique d'interaction pour UserListView.xaml
/// </summary>
public partial class UserListView : UserControl
{
    public UserListView(UserListViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
