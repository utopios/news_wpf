namespace LibraryManager.Modern.Models;

public enum LoanStatus
{
    Active,
    Returned,
    Overdue,
    Lost
}

//  Record avec init-only properties
public record Loan(
    int Id,
    int BookId,
    string BookTitle,
    int MemberId,
    string MemberName,
    DateTime BorrowDate,
    DateTime DueDate,
    DateTime? ReturnDate,
    LoanStatus Status,
    decimal Penalty)
{
    //  Propriété calculée pour les jours de retard
    public int DaysOverdue
    {
        get
        {
            if (Status is not (LoanStatus.Overdue or LoanStatus.Active))
                return 0;

            var compareDate = ReturnDate ?? DateTime.Now;
            return compareDate > DueDate ? (compareDate - DueDate).Days : 0;
        }
    }

    //  Calcul de pénalité avec pattern matching
    public decimal CalculatedPenalty => DaysOverdue switch
    {
        <= 0 => 0m,
        <= 7 => DaysOverdue * 0.50m,
        <= 14 => (7 * 0.50m) + ((DaysOverdue - 7) * 0.75m),
        _ => (7 * 0.50m) + (7 * 0.75m) + ((DaysOverdue - 14) * 1.00m)
    };

    //  Switch expression pour le texte du statut
    public string StatusText => Status switch
    {
        LoanStatus.Active => "En cours",
        LoanStatus.Returned => "Retourné",
        LoanStatus.Overdue => "En retard",
        LoanStatus.Lost => "Perdu",
        _ => "Inconnu"
    };

    //  Couleur du statut
    public string StatusColor => Status switch
    {
        LoanStatus.Active => "#42A5F5",
        LoanStatus.Returned => "#66BB6A",
        LoanStatus.Overdue => "#EF5350",
        LoanStatus.Lost => "#9E9E9E",
        _ => "#757575"
    };

    //  Icône du statut
    public string StatusIcon => Status switch
    {
        LoanStatus.Active => "📖",
        LoanStatus.Returned => "",
        LoanStatus.Overdue => "⚠️",
        LoanStatus.Lost => "❌",
        _ => "❓"
    };

    //  Nombre de jours d'emprunt
    public int BorrowDurationDays => ((ReturnDate ?? DateTime.Now) - BorrowDate).Days;

    //  Est en retard?
    public bool IsOverdue => Status == LoanStatus.Overdue ||
                            (Status == LoanStatus.Active && DateTime.Now > DueDate);
}