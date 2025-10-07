namespace LibraryManager.Modern.Models;

public enum MembershipType
{
    Basic,
    Premium,
    Student,
    Senior
}

//  Record avec toutes les propriétés
public record Member(
    int Id,
    string FirstName,
    string LastName,
    string Email,
    string Phone,
    MembershipType MembershipType,
    DateTime RegistrationDate,
    bool IsActive)
{
    //  Propriété calculée simple
    public string FullName => $"{FirstName} {LastName}";

    //  Switch expression pour le badge
    public string MembershipBadge => MembershipType switch
    {
        MembershipType.Premium => "⭐ Premium",
        MembershipType.Student => "🎓 Étudiant",
        MembershipType.Senior => "👴 Senior",
        MembershipType.Basic => "👤 Basic",
        _ => "❓ Inconnu"
    };

    //  Switch expression pour la limite d'emprunt
    public int MaxBorrowLimit => MembershipType switch
    {
        MembershipType.Premium => 10,
        MembershipType.Student => 5,
        MembershipType.Senior => 5,
        MembershipType.Basic => 3,
        _ => 3
    };

    //  Switch expression pour la durée d'emprunt
    public int BorrowDurationDays => MembershipType switch
    {
        MembershipType.Premium => 30,
        MembershipType.Student => 21,
        _ => 14
    };

    //  Propriétés calculées supplémentaires
    public int MembershipDurationDays => (DateTime.Now - RegistrationDate).Days;

    public string MembershipDuration => MembershipDurationDays switch
    {
        < 30 => "Nouveau",
        < 365 => "Récent",
        _ => "Fidèle"
    };

    public string StatusBadge => IsActive ? " Actif" : "❌ Inactif";
}