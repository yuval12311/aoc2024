// See https://aka.ms/new-console-template for more information

namespace Day2;

class Program
{
    public static void Main(string[] args)
    {
        
        Console.WriteLine(SolveFile("C:\\Users\\yuval\\RiderProjects\\AdventOfCode2024\\Day2\\input.txt", IsSafeLevel));
        Console.WriteLine(SolveFile("C:\\Users\\yuval\\RiderProjects\\AdventOfCode2024\\Day2\\input.txt", IsSafeLevel2));
    }

    private static int SolveFile(string inputPath, Func<List<int>, bool> condition)
    {
        return File.ReadAllLines(inputPath)
            .Select(l => l.Split(" ").Select(int.Parse).ToList())
            .Where(condition)
            .Count();
    }

    private static bool IsSafeLevel(List<int> level)
    {
        int isIncreasing = level[0] < level[1] ? 1 : -1;
        for (int i = 0; i < level.Count - 1; i++)
        {
             var diff = isIncreasing * (level[i + 1] - level[i]);
             if (diff is <= 0 or > 3)
             {
                 //Console.WriteLine($"level [{string.Join(", ", level)}] bad: {level[i]}, {level[i + 1]}");
                 return false;
             }
        }
        return true;
    }
    
    private static bool IsSafeLevel2(List<int> level)
    {
        return IsSafeLevel(level) || Enumerable.Range(0, level.Count).Any(i =>
        {
            var copy = level.ToList();
            copy.RemoveAt(i);
            return IsSafeLevel(copy);
        });
    }

    

    private static bool DiffBad(int[] level, int isIncreasing, int low, int high)
    {
        var diff = isIncreasing * (level[high] - level[low]);
        return diff is <= 0 or > 3;
    }
}