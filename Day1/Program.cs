// See https://aka.ms/new-console-template for more information
using System.Linq;
class Program
{
    public static void Main(string[] args)
    {
        
        Console.WriteLine(SolveFile("C:\\Users\\yuval\\RiderProjects\\AdventOfCode2024\\Day1\\input.txt"));
    }

    private static int SolveFile(string inputFile)
    {
        var lines = System.IO.File.ReadAllLines(inputFile);
        List<int> left = new();
        List<int> right = new();
        foreach (var line in lines)
        {
            var values = line.Split("   ");
            left.Add(int.Parse(values[0]));
            right.Add(int.Parse(values[1]));
        }
        return left.Order().Zip(right.Order(), (x, y) => Math.Abs(x - y)).Sum();
    }
}