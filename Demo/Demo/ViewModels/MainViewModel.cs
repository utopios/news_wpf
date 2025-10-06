using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Demo.Models;
using Demo.Tools;

namespace Demo.ViewModels;


public partial class MainViewModel: ObservableObject
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

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(FullName))]
    public string _firstname = string.Empty;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(FullName))]
    public string _lastname = string.Empty;

    public string FullName => $"{Firstname}  {Lastname}";
  
    

    [RelayCommand(CanExecute = nameof(canSave))]
    private void Save()
    {

    }

    private bool canSave() => true;

}

public record Customer(int Id, string Name);

