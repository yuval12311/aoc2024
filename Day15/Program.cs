namespace Day15;

class Program
{
    static void Main(string[] args)
    {
        var t = new TimeOnly();

        //Console.WriteLine(SolveFile("..\\..\\..\\\\input.txt"));
        SolveFile("..\\..\\..\\\\input.txt");
        Console.WriteLine((new TimeOnly() - t).TotalNanoseconds);
    }

    private static int SolveFile(string inputFile)
    {
        throw new NotImplementedException();
    }
}