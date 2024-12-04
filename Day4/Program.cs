using System.Reflection.Metadata;

namespace Day4;

class Program
{
    public static void Main()
    {
        Console.WriteLine(SolveFile("..\\..\\..\\\\input.txt"));
        Console.WriteLine(SolveFile2("..\\..\\..\\\\input.txt"));
    }

    private static int SolveFile2(string inputFile)
    {
        char[][] input = File.ReadAllLines(inputFile).Select(s => s.ToCharArray()).ToArray();
        return (from i in Enumerable.Range(0, input.Length)
            from j in Enumerable.Range(0, input[0].Length)
            where IsMas(input, i, j)
            select 1).Count();
    }

    private static bool IsMas(char[][] input, int i, int j)
    {
        if (input[i][j] != 'A' || i < 1 || j < 1 || i > input.Length - 2 || j > input.Length - 2)
        {
            return false;
        }

        return ((input[i - 1][j - 1] == 'M' && input[i + 1][j + 1] == 'S')
                || (input[i - 1][j - 1] == 'S' && input[i + 1][j + 1] == 'M'))
               && ((input[i + 1][j - 1] == 'M' && input[i - 1][j + 1] == 'S')
                   || (input[i + 1][j - 1] == 'S' && input[i - 1][j + 1] == 'M'));
    }

    private static int SolveFile(string inputFile)
    {
        char[][] input = File.ReadAllLines(inputFile).Select(s => s.ToCharArray()).ToArray();
        var count = 0;
        for (int i = 0; i < input.Length; i++)
        {
            for (int j = 0; j < input[i].Length; j++)
            {
                count += CountStringFromHere(input, i, j, "XMAS");
            }
        }

        return count;
    }

    private static int CountStringFromHere(char[][] input, int i, int j, string str)
    {
        return Enumerable.Range(-1, 3).SelectMany(di => Enumerable.Range(-1, 3).Select(dj => (di, dj)))
            .Where(pair => pair.di != 0 || pair.dj != 0)
            .Select(pair => FindStringInDir(input, i, j, str, pair.di, pair.dj))
            .Sum();
    }

    private static int FindStringInDir(char[][] input, int i, int j, string str, int diri, int dirj)
    {
        foreach (var c in str)
        {
            if (0 <= i && i < input.Length && 0 <= j && j < input[i].Length && c == input[i][j])
            {
                i += diri;
                j += dirj;
            }
            else
            {
                return 0;
            }
        }

        return 1;
    }
}