

namespace Demo.Models;

public class Person(string firtname, string lastname)
{
    //public List<string> Addresses { get; set; }
    //public Person()
    //{

    //}
    public string Name { get; init; } = firtname + " " + lastname;
}

//public class Person
//{
//    public string Firstname { get; init; }

//    public Person(string firstName)
//    {
//        //Firstname = firstName;
//    }
//}

public class Person
{
    public required string Name { get; init; }
    public string? Phone { get; init; }
}
