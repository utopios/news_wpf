using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using LibraryManager.Commands;
using LibraryManager.Models;
using LibraryManager.Services;

namespace LibraryManager.ViewModels
{
    public class BookListViewModel : INotifyPropertyChanged
    {
        private IBookService _bookService;
        private ObservableCollection<Book> _books;
        private Book _selectedBook;
        private string _searchText;
        private bool _isLoading;
        private BookCategory? _selectedCategory;

        public BookListViewModel(IBookService bookService)
        {
            _bookService = bookService;
            _books = new ObservableCollection<Book>();
            _searchText = string.Empty;
            
            LoadBooksCommand = new RelayCommand(ExecuteLoadBooks);
            SearchCommand = new RelayCommand(ExecuteSearch);
            DeleteBookCommand = new RelayCommand(ExecuteDeleteBook, CanDeleteBook);
            BorrowBookCommand = new RelayCommand(ExecuteBorrowBook, CanBorrowBook);
            
            LoadBooks();
        }

        public ObservableCollection<Book> Books
        {
            get { return _books; }
            set
            {
                if (_books != value)
                {
                    _books = value;
                    OnPropertyChanged();
                }
            }
        }

        public Book SelectedBook
        {
            get { return _selectedBook; }
            set
            {
                if (_selectedBook != value)
                {
                    _selectedBook = value;
                    OnPropertyChanged();
                    RaiseCanExecuteChanged();
                }
            }
        }

        public string SearchText
        {
            get { return _searchText; }
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                if (_isLoading != value)
                {
                    _isLoading = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand LoadBooksCommand { get; private set; }
        public ICommand SearchCommand { get; private set; }
        public ICommand DeleteBookCommand { get; private set; }
        public ICommand BorrowBookCommand { get; private set; }

        private void LoadBooks()
        {
            IsLoading = true;
            
            Books.Clear();
            var books = _bookService.GetAllBooks();
            foreach (var book in books)
            {
                Books.Add(book);
            }
            
            IsLoading = false;
        }

        private void ExecuteLoadBooks(object parameter)
        {
            LoadBooks();
        }

        private void ExecuteSearch(object parameter)
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                LoadBooks();
                return;
            }

            IsLoading = true;
            
            Books.Clear();
            var results = _bookService.SearchBooks(SearchText);
            foreach (var book in results)
            {
                Books.Add(book);
            }
            
            IsLoading = false;
        }

        private void ExecuteDeleteBook(object parameter)
        {
            if (SelectedBook == null)
                return;

            bool success = _bookService.DeleteBook(SelectedBook.Id);
            if (success)
            {
                Books.Remove(SelectedBook);
                SelectedBook = null;
            }
        }

        private bool CanDeleteBook(object parameter)
        {
            return SelectedBook != null;
        }

        private void ExecuteBorrowBook(object parameter)
        {
            if (SelectedBook == null)
                return;

            bool success = _bookService.BorrowBook(SelectedBook.Id);
            if (success)
            {
                LoadBooks();
            }
        }

        private bool CanBorrowBook(object parameter)
        {
            return SelectedBook != null && SelectedBook.IsAvailable();
        }

        private void RaiseCanExecuteChanged()
        {
            (DeleteBookCommand as RelayCommand)?.RaiseCanExecuteChanged();
            (BorrowBookCommand as RelayCommand)?.RaiseCanExecuteChanged();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
