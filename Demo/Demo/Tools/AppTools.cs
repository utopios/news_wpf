
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Tools;

public class AppTools
{
    public static string DescribeList(int[] numbers) => numbers switch
    {
        [] => "Liste vide",
        [var x] => $"Un seul élément : {x}",
        [var x, var y] => $"Deux éléments : {x} et {y}",
        [1, 2, ..] => "commence par 1 et 2",
        [.., 9, 10] => "Termine par 9,10",
        [var first, .., var last] => $"Premier ; {first}, Last : {last}",
        
        _ => "Autre"
    };

    public record Person(string Name, int Age, string City);

    public static string GetDiscount(Person person) => person switch
    {
        { Age: < 18 } => "Tarif enfant",
        { Age: >= 65 } => "Tarif senior",
        { City: "Paris" } => "Tarif parisien",
        { Age: >= 18 and < 65 } => "Tarif normal",
        _ => "Tarif par défaut"
    };

    public record Point(int X, int Y);

    public static string Describe(Point point) => point switch
    {
        (0, 0) => "Origine",
        (var x, 0) => $"Sur l'axe X à {x}",
        (0, var y) => $"Sur l'axe Y à {y}",
        (var x, var y) when x == y => "Sur la diagonale",
        (var x, var y) => $"Point ({x}, {y})"
    };

    public static string ProcessShape(object shape) => shape switch
    {
        Circle { Radius: > 0 } c => $"Cercle de rayon {c.Radius}",
        Rectangle { Width: var w, Height: var h } => $"Rectangle {w}x{h}",
        Triangle t when t.IsEquilateral() => "Triangle équilatéral",
        null => "Forme nulle",
        _ => "Forme inconnue"
    };
}

internal class Triangle
{
    internal bool IsEquilateral()
    {
        throw new NotImplementedException();
    }
}

internal class Circle
{
    public int Radius { get; internal set; }
}