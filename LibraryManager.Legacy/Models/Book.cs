using System;

namespace LibraryManager.Models
{
    public enum BookStatus
    {
        Available,
        Borrowed,
        Reserved,
        Maintenance
    }

    public enum BookCategory
    {
        Fiction,
        NonFiction,
        Science,
        History,
        Biography,
        Children
    }

    public class Book
    {
        private int _id;
        private string _title;
        private string _author;
        private string _isbn;
        private BookCategory _category;
        private BookStatus _status;
        private DateTime _publishedDate;
        private int _totalCopies;
        private int _availableCopies;

        public Book(int id, string title, string author, string isbn, 
                    BookCategory category, BookStatus status, 
                    DateTime publishedDate, int totalCopies, int availableCopies)
        {
            _id = id;
            _title = title;
            _author = author;
            _isbn = isbn;
            _category = category;
            _status = status;
            _publishedDate = publishedDate;
            _totalCopies = totalCopies;
            _availableCopies = availableCopies;
        }

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        public string Author
        {
            get { return _author; }
            set { _author = value; }
        }

        public string ISBN
        {
            get { return _isbn; }
            set { _isbn = value; }
        }

        public BookCategory Category
        {
            get { return _category; }
            set { _category = value; }
        }

        public BookStatus Status
        {
            get { return _status; }
            set { _status = value; }
        }

        public DateTime PublishedDate
        {
            get { return _publishedDate; }
            set { _publishedDate = value; }
        }

        public int TotalCopies
        {
            get { return _totalCopies; }
            set { _totalCopies = value; }
        }

        public int AvailableCopies
        {
            get { return _availableCopies; }
            set { _availableCopies = value; }
        }

        public string GetDisplayName()
        {
            return _title + " by " + _author;
        }

        public string GetStatusText()
        {
            if (_status == BookStatus.Available)
                return "Disponible";
            else if (_status == BookStatus.Borrowed)
                return "Emprunté";
            else if (_status == BookStatus.Reserved)
                return "Réservé";
            else if (_status == BookStatus.Maintenance)
                return "En maintenance";
            else
                return "Inconnu";
        }

        public string GetCategoryBadge()
        {
            switch (_category)
            {
                case BookCategory.Fiction:
                    return "[FICTION] Fiction";
                case BookCategory.NonFiction:
                    return "[NON-FICTION] Non-Fiction";
                case BookCategory.Science:
                    return "[SCIENCE] Science";
                case BookCategory.History:
                    return "[HISTOIRE] Histoire";
                case BookCategory.Biography:
                    return "[BIO] Biographie";
                case BookCategory.Children:
                    return "[JEUNESSE] Jeunesse";
                default:
                    return "[AUTRE] Autre";
            }
        }

        public bool IsAvailable()
        {
            return _availableCopies > 0 && _status == BookStatus.Available;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Book other = (Book)obj;
            return _id == other._id &&
                   _title == other._title &&
                   _author == other._author &&
                   _isbn == other._isbn;
        }

        public override int GetHashCode()
        {
            return _id.GetHashCode() ^ _title.GetHashCode() ^ _isbn.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("Book {{ Id = {0}, Title = {1}, Author = {2}, ISBN = {3} }}", 
                                _id, _title, _author, _isbn);
        }
    }
}
