// See https://aka.ms/new-console-template for more information

using System.Numerics;

namespace Day8;

class Program
{
    static void Main(string[] args)
    {
        var t = new TimeOnly();
        
        Console.WriteLine(SolveFile("..\\..\\..\\\\input.txt"));

        Console.WriteLine((new TimeOnly() - t).TotalNanoseconds);
    }

    private record struct Vector2<T> (T x, T y)
        where T : INumber<T>
    {
        public static Vector2<T> operator +(Vector2<T> a, Vector2<T> b) => new(a.x + b.x, a.y + b.y);
        public static Vector2<T> operator -(Vector2<T> a, Vector2<T> b) => new(a.x - b.x, a.y - b.y);

    }

    private static int SolveFile(string inputFile)
    {
        var text = File.ReadAllLines(inputFile);
        int maxI = text.Length - 1;
        int maxJ = text[0].Length - 1;

        return text
            .SelectMany((line, i) => line.Select((c, j) => new { Char = c, Point = new Vector2<int>(i, j) }))
            .Where(t => t.Char != '.')
            .GroupBy(t => t.Char)
            .SelectMany(g =>
                g.SelectMany((p, i) => g.Skip(i + 1),
                        (p1, p2) => (p1.Point, p2.Point))
                    .SelectMany(pair => new List<Vector2<int>>
                        { pair.Item1 + pair.Item1 - pair.Item2, pair.Item2 + pair.Item2 - pair.Item1 })
                    .Where(p => IsInBounds(p, maxI, maxJ))
            )
            .Distinct()
            .Count();
    }

    private static bool IsInBounds(Vector2<int> vector, int maxI, int maxJ)
    {
        return 0 <= vector.x && vector.x <= maxI && 0 <= vector.y && vector.y <= maxJ;
    }
}