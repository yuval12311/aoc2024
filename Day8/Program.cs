// See https://aka.ms/new-console-template for more information

namespace Day8;

class Program
{
    static void Main(string[] args)
    {
        var t = new TimeOnly();

        Console.WriteLine(SolveFile("..\\..\\..\\\\input.txt"));

        Console.WriteLine((new TimeOnly() - t).TotalNanoseconds);
    }

    private static int SolveFile(string inputFile)
    {
        return 0;
    }
}