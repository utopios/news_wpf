using System;

namespace LibraryManager.Models
{
    public enum LoanStatus
    {
        Active,
        Returned,
        Overdue,
        Lost
    }

    public class Loan
    {
        private int _id;
        private int _bookId;
        private string _bookTitle;
        private int _memberId;
        private string _memberName;
        private DateTime _borrowDate;
        private DateTime _dueDate;
        private DateTime? _returnDate;
        private LoanStatus _status;
        private decimal _penalty;

        public Loan(int id, int bookId, string bookTitle, int memberId, 
                   string memberName, DateTime borrowDate, DateTime dueDate, 
                   DateTime? returnDate, LoanStatus status, decimal penalty)
        {
            _id = id;
            _bookId = bookId;
            _bookTitle = bookTitle;
            _memberId = memberId;
            _memberName = memberName;
            _borrowDate = borrowDate;
            _dueDate = dueDate;
            _returnDate = returnDate;
            _status = status;
            _penalty = penalty;
        }

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public int BookId
        {
            get { return _bookId; }
            set { _bookId = value; }
        }

        public string BookTitle
        {
            get { return _bookTitle; }
            set { _bookTitle = value; }
        }

        public int MemberId
        {
            get { return _memberId; }
            set { _memberId = value; }
        }

        public string MemberName
        {
            get { return _memberName; }
            set { _memberName = value; }
        }

        public DateTime BorrowDate
        {
            get { return _borrowDate; }
            set { _borrowDate = value; }
        }

        public DateTime DueDate
        {
            get { return _dueDate; }
            set { _dueDate = value; }
        }

        public DateTime? ReturnDate
        {
            get { return _returnDate; }
            set { _returnDate = value; }
        }

        public LoanStatus Status
        {
            get { return _status; }
            set { _status = value; }
        }

        public decimal Penalty
        {
            get { return _penalty; }
            set { _penalty = value; }
        }

        public int GetDaysOverdue()
        {
            if (_status != LoanStatus.Overdue && _status != LoanStatus.Active)
                return 0;

            //DateTime compareDate;
            //if (_returnDate.HasValue)
            //    compareDate = _returnDate.Value;
            //else
            //    compareDate = DateTime.Now;

            //if (compareDate > _dueDate)
            //{
            //    TimeSpan difference = compareDate - _dueDate;
            //    return difference.Days;
            //}

            //return 0;
            var compareDate = ReturnDate ?? DateTime.Now;
            return compareDate > DueDate ? (compareDate - DueDate).Days : 0;
        }

        public decimal CalculatePenalty()
        {
            int daysOverdue = GetDaysOverdue();
            if (daysOverdue <= 0)
                return 0m;

            decimal dailyPenalty = 0.50m;
            return daysOverdue * dailyPenalty;
        }

        public string GetStatusText()
        {
            if (_status == LoanStatus.Active)
                return "En cours";
            else if (_status == LoanStatus.Returned)
                return "Retourné";
            else if (_status == LoanStatus.Overdue)
                return "En retard";
            else if (_status == LoanStatus.Lost)
                return "Perdu";
            else
                return "Inconnu";
        }
    }
}
