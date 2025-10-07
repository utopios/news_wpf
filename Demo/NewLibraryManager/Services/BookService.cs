namespace LibraryManager.Modern.Services;

public interface IBookService
{
    Task<List<Book>> GetAllAsync();
    Task<Book?> GetByIdAsync(int id);
    Task<List<Book>> SearchAsync(string searchText);
    Task<List<Book>> GetByCategoryAsync(BookCategory category);
    Task<List<Book>> GetAvailableAsync();
    Task<Book> AddAsync(Book book);
    Task UpdateAsync(Book book);
    Task DeleteAsync(int id);
    Task BorrowAsync(int bookId);
    Task ReturnAsync(int bookId);
}

//  Primary constructor
public class BookService(List<Book>? initialBooks = null) : IBookService
{
    private readonly List<Book> _books = initialBooks ?? InitializeSampleData();

    //  Collection expression pour l'initialisation
    private static List<Book> InitializeSampleData() => [
        new(1, "Le Seigneur des Anneaux", "J.R.R. Tolkien", "978-2-07-061332-8",
            BookCategory.Fiction, BookStatus.Available, new(1954, 7, 29), 5, 3),

        new(2, "1984", "George Orwell", "978-0-452-28423-4",
            BookCategory.Fiction, BookStatus.Borrowed, new(1949, 6, 8), 3, 0),

        new(3, "Une brève histoire du temps", "Stephen Hawking", "978-2-08-081238-4",
            BookCategory.Science, BookStatus.Available, new(1988, 4, 1), 2, 2),

        new(4, "Sapiens", "Yuval Noah Harari", "978-2-226-25701-7",
            BookCategory.History, BookStatus.Available, new(2011, 1, 1), 4, 4),

        new(5, "Harry Potter à l'école des sorciers", "J.K. Rowling", "978-2-07-054127-3",
            BookCategory.Children, BookStatus.Reserved, new(1997, 6, 26), 8, 0)
    ];

    //  Collection expression avec spread
    public Task<List<Book>> GetAllAsync() => Task.FromResult<List<Book>>([.. _books]);

    //  LINQ moderne avec FirstOrDefault
    public Task<Book?> GetByIdAsync(int id) =>
        Task.FromResult(_books.FirstOrDefault(b => b.Id == id));

    //  Pattern matching + LINQ + collection expression
    public Task<List<Book>> SearchAsync(string searchText) =>
        Task.FromResult<List<Book>>(searchText switch
        {
            null or { Length: 0 } => [.. _books],
            var search => [.._books.Where(b =>
                b.Title.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                b.Author.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                b.ISBN.Contains(search))]
        });

    //  LINQ + collection expression
    public Task<List<Book>> GetByCategoryAsync(BookCategory category) =>
        Task.FromResult<List<Book>>([.. _books.Where(b => b.Category == category)]);

    //  LINQ avec IsAvailable
    public Task<List<Book>> GetAvailableAsync() =>
        Task.FromResult<List<Book>>([.. _books.Where(b => b.IsAvailable)]);

    //  LINQ pour trouver le max ID
    public Task<Book> AddAsync(Book book)
    {
        var maxId = _books.Any() ? _books.Max(b => b.Id) : 0;
        var newBook = book with { Id = maxId + 1 };
        _books.Add(newBook);
        return Task.FromResult(newBook);
    }

    //  LINQ FindIndex + collection expression
    public Task UpdateAsync(Book book)
    {
        var index = _books.FindIndex(b => b.Id == book.Id);
        if (index >= 0)
            _books[index] = book;
        return Task.CompletedTask;
    }

    //  LINQ RemoveAll
    public Task DeleteAsync(int id)
    {
        _books.RemoveAll(b => b.Id == id);
        return Task.CompletedTask;
    }

    //  Pattern matching pour la logique
    public Task BorrowAsync(int bookId)
    {
        var book = _books.FirstOrDefault(b => b.Id == bookId);
        if (book is null or { IsAvailable: false })
            return Task.CompletedTask;

        var updatedBook = book with
        {
            AvailableCopies = book.AvailableCopies - 1,
            Status = book.AvailableCopies - 1 == 0 ? BookStatus.Borrowed : book.Status
        };

        var index = _books.FindIndex(b => b.Id == bookId);
        if (index >= 0)
            _books[index] = updatedBook;

        return Task.CompletedTask;
    }

    //  Pattern matching + with expression
    public Task ReturnAsync(int bookId)
    {
        var book = _books.FirstOrDefault(b => b.Id == bookId);
        if (book is null)
            return Task.CompletedTask;

        var updatedBook = book with
        {
            AvailableCopies = book.AvailableCopies + 1,
            Status = BookStatus.Available
        };

        var index = _books.FindIndex(b => b.Id == bookId);
        if (index >= 0)
            _books[index] = updatedBook;

        return Task.CompletedTask;
    }
}