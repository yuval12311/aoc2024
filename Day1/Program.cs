// See https://aka.ms/new-console-template for more information
using System.Linq;
class Program
{
    public static void Main(string[] args)
    {
        
        Console.WriteLine(SolveFile("..\\..\\..\\input.txt"));
        Console.WriteLine(SolveFile2("..\\..\\..\\input.txt"));
    }

    private static int SolveFile(string inputFile)
    {
        var (left, right) = GetNumbers(inputFile);
        return left.Order().Zip(right.Order(), (x, y) => Math.Abs(x - y)).Sum();
    }

    private static (List<int> left, List<int> right) GetNumbers(string inputFile)
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

        return (left, right);
    }

    private static int SolveFile2(string inputFile)
    {
        var (left, right) = GetNumbers(inputFile);
        Histogram<int> rightHistorgram = new();
        foreach (var num in right)
        {
            rightHistorgram.IncrementCount(num);
        }

        return left.Select(x => x * rightHistorgram.GetCount(x)).Sum();
    }
}

public class Histogram<TVal> : Dictionary<TVal, int>
{
    public void IncrementCount(TVal binToIncrement)
    {
        if (ContainsKey(binToIncrement))
        {
            this[binToIncrement]++;
        }
        else
        {
            Add(binToIncrement, 1);
        }
    }

    public int GetCount(TVal binToGet)
    {
        return ContainsKey(binToGet) ? this[binToGet] : 0;
    }
}