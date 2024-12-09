// See https://aka.ms/new-console-template for more information

using System.Numerics;
using System.Text;

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
        public static Vector2<T> operator *(T a, Vector2<T> b) => new(a *  b.x, a * b.y);

    }

    private static int SolveFile(string inputFile)
    {
        var text = File.ReadAllLines(inputFile);
        int maxI = text.Length - 1;
        int maxJ = text[0].Length - 1;
        var vecs = text
            .SelectMany((line, i) => 
                line.Select((c, j) => 
                    new { Char = c, Point = new Vector2<int>(i, j) }))
            .Where(t => t.Char != '.')
            .GroupBy(t => t.Char)
            .SelectMany(g =>
                g.SelectMany((p, i) => g,
                        (p1, p2) => (p1.Point, p2.Point))
                    .Where(pair => pair.Item1 != pair.Item2)
                    .SelectMany(pair => Enumerable.Range(0, Math.Max(maxI, maxJ))
                        .Select(i => pair.Item1 + i * (pair.Item1 - pair.Item2))
                        //.Take(1)
                        .TakeWhile(p => IsInBounds(p, maxI, maxJ))
                    )
            )
            .Distinct()
            .ToList();

        PrintAnti(text, vecs);

        return vecs
            .Count();
    }

    private static void PrintAnti(string[] text, List<Vector2<int>> vecs)
    {
        var chars = text.Select(s => s.ToCharArray()).ToArray();
        foreach (var vec in vecs)
        {
            chars[vec.x][vec.y] = '#';
        }

        foreach (var str in chars)
        {
            Console.WriteLine(str);
        }
        
    }

    private static bool IsInBounds(Vector2<int> vector, int maxI, int maxJ)
    {
        return 0 <= vector.x && vector.x <= maxI && 0 <= vector.y && vector.y <= maxJ;
    }
}