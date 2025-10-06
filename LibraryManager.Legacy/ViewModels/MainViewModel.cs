using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using LibraryManager.Commands;
using LibraryManager.Services;

namespace LibraryManager.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private IBookService _bookService;
        private IMemberService _memberService;
        private string _title;
        private object _currentViewModel;

        public MainViewModel()
        {
            _bookService = new BookService();
            _memberService = new MemberService();
            _title = "Gestion de Bibliothèque";
            
            ShowBooksCommand = new RelayCommand(ExecuteShowBooks);
            ShowMembersCommand = new RelayCommand(ExecuteShowMembers);
            
            // Charger la vue des livres par défaut
            ExecuteShowBooks(null);
        }

        public string Title
        {
            get { return _title; }
            set
            {
                if (_title != value)
                {
                    _title = value;
                    OnPropertyChanged();
                }
            }
        }

        public object CurrentViewModel
        {
            get { return _currentViewModel; }
            set
            {
                if (_currentViewModel != value)
                {
                    _currentViewModel = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand ShowBooksCommand { get; private set; }
        public ICommand ShowMembersCommand { get; private set; }

        private void ExecuteShowBooks(object parameter)
        {
            CurrentViewModel = new BookListViewModel(_bookService);
        }

        private void ExecuteShowMembers(object parameter)
        {
            // Implementation future
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
