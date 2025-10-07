namespace LibraryManager.Modern.Services;

public interface IMemberService
{
    Task<List<Member>> GetAllAsync();
    Task<Member?> GetByIdAsync(int id);
    Task<List<Member>> GetActiveAsync();
    Task<Member> AddAsync(Member member);
    Task UpdateAsync(Member member);
    Task DeactivateAsync(int id);
}

//  Primary constructor + collection expression
public class MemberService(List<Member>? initialMembers = null) : IMemberService
{
    private readonly List<Member> _members = initialMembers ?? InitializeSampleData();

    //  Collection expression pour l'initialisation
    private static List<Member> InitializeSampleData() => [
        new(1, "Jean", "Dupont", "jean.dupont@email.com", "0612345678",
            MembershipType.Premium, DateTime.Now.AddMonths(-12), true),

        new(2, "Marie", "Martin", "marie.martin@email.com", "0623456789",
            MembershipType.Student, DateTime.Now.AddMonths(-6), true),

        new(3, "Pierre", "Bernard", "pierre.bernard@email.com", "0634567890",
            MembershipType.Basic, DateTime.Now.AddMonths(-3), true),

        new(4, "Sophie", "Petit", "sophie.petit@email.com", "0645678901",
            MembershipType.Senior, DateTime.Now.AddYears(-2), false)
    ];

    //  Collection expression avec spread
    public Task<List<Member>> GetAllAsync() => Task.FromResult<List<Member>>([.. _members]);

    //  LINQ FirstOrDefault
    public Task<Member?> GetByIdAsync(int id) =>
        Task.FromResult(_members.FirstOrDefault(m => m.Id == id));

    //  LINQ Where + collection expression
    public Task<List<Member>> GetActiveAsync() =>
        Task.FromResult<List<Member>>([.. _members.Where(m => m.IsActive)]);

    //  LINQ Max + collection expression
    public Task<Member> AddAsync(Member member)
    {
        var maxId = _members.Any() ? _members.Max(m => m.Id) : 0;
        var newMember = member with { Id = maxId + 1, RegistrationDate = DateTime.Now };
        _members.Add(newMember);
        return Task.FromResult(newMember);
    }

    //  LINQ FindIndex
    public Task UpdateAsync(Member member)
    {
        var index = _members.FindIndex(m => m.Id == member.Id);
        if (index >= 0)
            _members[index] = member;
        return Task.CompletedTask;
    }

    //  Pattern matching + with expression
    public Task DeactivateAsync(int id)
    {
        var member = _members.FirstOrDefault(m => m.Id == id);
        if (member is not null)
        {
            var index = _members.FindIndex(m => m.Id == id);
            if (index >= 0)
                _members[index] = member with { IsActive = false };
        }
        return Task.CompletedTask;
    }
}