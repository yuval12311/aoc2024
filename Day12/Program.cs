namespace Day12;

class Program
{
    static void Main(string[] args)
    {
        var t = new TimeOnly();
        
        Console.WriteLine(SolveFile("..\\..\\..\\\\test.txt"));
        //Console.WriteLine(SolveFile2("..\\..\\..\\\\input.txt"));

        Console.WriteLine((new TimeOnly() - t).TotalNanoseconds);
    }

    private static bool SolveFile(string testTxt)
    {
        throw new NotImplementedException();
    }
}