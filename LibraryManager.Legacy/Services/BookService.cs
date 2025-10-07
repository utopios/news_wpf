using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LibraryManager.Models;

namespace LibraryManager.Services
{
    public interface IBookService
    {
        List<Book> GetAllBooks();
        Book GetBookById(int id);
        List<Book> SearchBooks(string searchText);
        List<Book> GetBooksByCategory(BookCategory category);
        List<Book> GetAvailableBooks();
        bool AddBook(Book book);
        bool UpdateBook(Book book);
        bool DeleteBook(int id);
        bool BorrowBook(int bookId);
        bool ReturnBook(int bookId);
    }

    public class BookService : IBookService
    {
        private List<Book> _books;

        public BookService()
        {
            _books = new List<Book>();
            InitializeSampleData();

           
            
        }

        private void InitializeSampleData()
        {
            _books.Add(new (1, "Le Seigneur des Anneaux", "J.R.R. Tolkien", "978-2-07-061332-8",
                BookCategory.Fiction, BookStatus.Available, new DateTime(1954, 7, 29), 5, 3));
            
            _books.Add(new Book(2, "1984", "George Orwell", "978-0-452-28423-4",
                BookCategory.Fiction, BookStatus.Borrowed, new DateTime(1949, 6, 8), 3, 0));
            
            _books.Add(new Book(3, "Une brève histoire du temps", "Stephen Hawking", "978-2-08-081238-4",
                BookCategory.Science, BookStatus.Available, new DateTime(1988, 4, 1), 2, 2));
            
            _books.Add(new Book(4, "Sapiens", "Yuval Noah Harari", "978-2-226-25701-7",
                BookCategory.History, BookStatus.Available, new DateTime(2011, 1, 1), 4, 4));
            
            _books.Add(new Book(5, "Harry Potter à l'école des sorciers", "J.K. Rowling", "978-2-07-054127-3",
                BookCategory.Children, BookStatus.Reserved, new DateTime(1997, 6, 26), 8, 0));
        }

        //Refactor en async
        public List<Book> GetAllBooks()
        {
            return new List<Book>(_books);
        }

        public Task<List<Book>> GetAllBooksAsync()
        {

            var task = Task.Run(() =>
            {
                Thread.CurrentThread.Priority = ThreadPriority.Highest;
                var books = new List<Book>();
                
                return books;
            });

            return Task.FromResult<List<Book>>([.._books]);
        }

        public Book GetBookById(int id)
        {
            foreach (var book in _books)
            {
                if (book.Id == id)
                    return book;
            }
            return null;
        }

        public List<Book> SearchBooks(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
                return GetAllBooks();

            List<Book> results = new List<Book>();
            string searchLower = searchText.ToLower();

            foreach (var book in _books)
            {
                if (book.Title.ToLower().Contains(searchLower) ||
                    book.Author.ToLower().Contains(searchLower) ||
                    book.ISBN.Contains(searchText))
                {
                    results.Add(book);
                }
            }

            return results;
        }

        public List<Book> GetBooksByCategory(BookCategory category)
        {
            List<Book> results = new List<Book>();
            foreach (var book in _books)
            {
                if (book.Category == category)
                    results.Add(book);
            }
            return results;
        }

        public List<Book> GetAvailableBooks()
        {
            List<Book> results = new List<Book>();
            foreach (var book in _books)
            {
                if (book.IsAvailable())
                    results.Add(book);
            }
            return results;
        }

        public bool AddBook(Book book)
        {
            if (book == null)
                return false;

            int maxId = 0;
            foreach (var b in _books)
            {
                if (b.Id > maxId)
                    maxId = b.Id;
            }
            book.Id = maxId + 1;

            _books.Add(book);
            return true;
        }

        public bool UpdateBook(Book book)
        {
            if (book == null)
                return false;

            for (int i = 0; i < _books.Count; i++)
            {
                if (_books[i].Id == book.Id)
                {
                    _books[i] = book;
                    return true;
                }
            }
            return false;
        }

        public bool DeleteBook(int id)
        {
            for (int i = 0; i < _books.Count; i++)
            {
                if (_books[i].Id == id)
                {
                    _books.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }

        public bool BorrowBook(int bookId)
        {
            Book book = GetBookById(bookId);
            if (book == null || !book.IsAvailable())
                return false;

            book.AvailableCopies = book.AvailableCopies - 1;
            if (book.AvailableCopies == 0)
                book.Status = BookStatus.Borrowed;

            return true;
        }

        public bool ReturnBook(int bookId)
        {
            Book book = GetBookById(bookId);
            if (book == null)
                return false;

            book.AvailableCopies = book.AvailableCopies + 1;
            if (book.AvailableCopies > 0)
                book.Status = BookStatus.Available;

            return true;
        }
    }
}
