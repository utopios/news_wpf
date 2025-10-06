namespace LibraryManager.Modern.Models;

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


public record Book(
    int Id,
    string Title,
    string Author,
    string ISBN,
    BookCategory Category,
    BookStatus Status,
    DateTime PublishedDate,
    int TotalCopies,
    int AvailableCopies)
{

    public string DisplayName => $"{Title} by {Author}";

    //  Switch expression au lieu de if/else
    public string StatusText => Status switch
    {
        BookStatus.Available => "Disponible",
        BookStatus.Borrowed => "Emprunté",
        BookStatus.Reserved => "Réservé",
        BookStatus.Maintenance => "En maintenance",
        _ => "Inconnu"
    };

    //  Switch expression pour les badges
    public string CategoryBadge => Category switch
    {
        BookCategory.Fiction => "📚 Fiction",
        BookCategory.NonFiction => "📖 Non-Fiction",
        BookCategory.Science => "🔬 Science",
        BookCategory.History => "📜 Histoire",
        BookCategory.Biography => "👤 Biographie",
        BookCategory.Children => "🧸 Jeunesse",
        _ => "❓ Autre"
    };

    //  Pattern matching simple
    public bool IsAvailable => AvailableCopies > 0 && Status == BookStatus.Available;

    //  Propriété calculée pour la couleur du statut
    public string StatusColor => Status switch
    {
        BookStatus.Available => "#66BB6A",
        BookStatus.Borrowed => "#EF5350",
        BookStatus.Reserved => "#FFA726",
        BookStatus.Maintenance => "#9E9E9E",
        _ => "#757575"
    };

    //  Propriété pour le taux de disponibilité
    public double AvailabilityRate => TotalCopies > 0
        ? (double)AvailableCopies / TotalCopies * 100
        : 0;
}
