using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.Models;
using Demo.Tools;

namespace Demo.ViewModels;
public class MainViewModel
{
    public MainViewModel() {
        //var customer1 = new Customer(1, "ihab");
        //customer1 = customer1 with { Name = "Abadi ihab"};
        ////Person p = new Person("ihab", "abadi");
        //Person p = new Person("toto");
        //Person p = new Person { Name = "Ihab" };
        //Person p2 = new Person { Phone = "00000" };
        Console.WriteLine(AppTools.DescribeList(new[] { 1 }));

        //List<string> list = new List<string>() { "toto", "tata"};
        List<string> list = ["toto", "tata", "titi"];
        List<string> list2 = ["arare", "zerezrrez"];
        List<string> combined = [.. list, .. list2, "zzrezrezr"];
    }
}

public record Customer(int Id, string Name);

