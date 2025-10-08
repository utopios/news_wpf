using System.Windows.Controls;

namespace WpfDIExample.Services;

/// <summary>
/// Service pour gérer la navigation entre les vues
/// </summary>
public interface INavigationService
{
    void NavigateTo<TView>() where TView : UserControl;
    void NavigateTo(Type viewType);
    UserControl? CurrentView { get; }
    event EventHandler<UserControl>? CurrentViewChanged;
}
