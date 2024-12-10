using System.Security.Claims;

namespace Day10;

class Program
{
    static void Main(string[] args)
    {
        var t = DateTime.Now;

        Console.WriteLine(SolveFile("..\\..\\..\\\\input.txt"));

        Console.WriteLine((DateTime.Now - t));
    }

    private static int SolveFile(string inputFile)
    {
        string[] lines = File.ReadAllLines(inputFile);
        int[,] map = new int[lines.Length, lines[0].Length];
        var trailHeads = new List<(int i, int j)>();
        for (int i = 0; i < lines.Length; i++)
        {
            for (int j = 0; j < lines[i].Length; j++)
            {
                map[i, j] = lines[i][j] - '0';
                if (map[i, j] == 0)
                {
                    trailHeads.Add((i, j));
                }
            }
        }
        return trailHeads.Select(coord => Score(map, coord)).Sum();
    }

    private static int Score(int[,] map, (int i, int j) trailHead)
    {
        HashSet<(int i, int j)> peaks = new HashSet<(int i, int j)>();
        AddPeaksRec(map, trailHead, peaks);
        return peaks.Count;
    }

    private static void AddPeaksRec(int[,] map, (int i, int j) coord, HashSet<(int i, int j)> peaks)
    {
        if (!InBounds(map, coord)) return;
        int height = map[coord.i, coord.j];
        if (height == 9)
        {
            peaks.Add((coord.i, coord.j));
            return;
        }

        if (InBounds(map, (coord.i+1, coord.j)) && map[coord.i + 1, coord.j] == height + 1)
        {
            AddPeaksRec(map, (coord.i + 1, coord.j), peaks);
        }
        if (InBounds(map, (coord.i-1, coord.j)) &&map[coord.i - 1, coord.j] == height + 1)
        {
            AddPeaksRec(map, (coord.i - 1, coord.j), peaks);
        }
        if (InBounds(map, (coord.i, coord.j + 1)) &&map[coord.i, coord.j + 1] == height + 1)
        {
            AddPeaksRec(map, (coord.i , coord.j + 1), peaks);
        }
        if (InBounds(map, (coord.i, coord.j -1)) && map[coord.i, coord.j - 1] == height + 1)
        {
            AddPeaksRec(map, (coord.i, coord.j - 1), peaks);
        }
    }

    private static bool InBounds(int[,] map, (int i, int j) coord)
    {
        return 0 <= coord.i && coord.i < map.GetLength(0) && 0 <= coord.j && coord.j < map.GetLength(1);
    }
}