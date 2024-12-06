using System.Diagnostics;
using System.Dynamic;
using System.Numerics;

namespace Day6;

class Program
{
    static void Main(string[] args)
    {
        for (int i = 0; i < 4; ++i)
        {
            Console.WriteLine($"{i}: {GetI(i)}, {GetJ(i)}");
        }
        Console.WriteLine(SolveFile("..\\..\\..\\\\input.txt"));
    }
    
    
    
    private static int SolveFile(string inputFile)
    {
        Dictionary<(int i, int j), HashSet<int>> visited = new();
        HashSet<(int i, int j)> possibleObst = new();
        (int[,] grid, (int i, int j) currentPosition, int dir)  = ParseFile(inputFile);
        while (IsInBounds(currentPosition, grid))
        {
            if (visited.ContainsKey(currentPosition))
            {
                visited[currentPosition] = new();
            }
            visited[currentPosition].Add(dir);
            (int i, int j) nextPos = (currentPosition.i + GetI(dir), currentPosition.j + GetJ(dir));
            if (IsInBounds(nextPos, grid) && grid[nextPos.i, nextPos.j] == 1)
            {
                dir = (dir + 1) % 4;
            }
            else if (IsInBounds(nextPos, grid) && grid[nextPos.i, nextPos.j] == 0)
            {
                currentPosition = nextPos;
            }
            //PrintGrid(grid, currentPosition, dir);
            //Thread.Sleep(100);
        }

        return visited.Count;
    }

    private static void PrintGrid(int[,] grid, (int i, int j) currentPosition, int dir)
    {
        Console.SetCursorPosition(0, 0);
        for (int i = 0; i < grid.GetLength(0); ++i)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                if (currentPosition.i == i && currentPosition.j == j)
                {
                    Console.Write(dir switch {0 => '^', 1 => '>', 2 => 'v', 3 => '<', _ => ' '});
                }
                else
                {
                    Console.Write(grid[i, j] == 1 ? '#' : '.');
                }
            }
            Console.WriteLine();
        }
    }

    private static int GetI(int dir) => (1 - dir % 2) * (dir/2 == 1 ? 1 : -1);
    private static int GetJ(int dir) => (dir % 2) * (dir/2 == 0 ? 1 : -1);
    private static bool IsInBounds((int i, int j) p, int[,] grid) 
        => 0 <= p.i && p.i < grid.GetLength(0) && 0 <= p.j && p.j < grid.GetLength(1);

    private static (int[,] grid, (int i, int j) currentPosition, int dir) ParseFile(string inputFile)
    {
        var dir = '^';
        (int i, int j) currentPosition = (0, 0);
        var lines = File.ReadAllLines(inputFile);
        int[,] grid = new int[lines.Length, lines[0].Length];
        for (int i = 0; i < lines.Length; i++)
        {
            for (int j = 0; j < lines[i].Length; j++)
            {
                switch (lines[i][j])
                {
                    case '.':
                        grid[i, j] = 0;
                        break;
                    case '#':
                        grid[i, j] = 1;
                        break;
                    case '^' or 'v' or '>' or '<':
                        dir = lines[i][j];
                        currentPosition = (i, j);
                        grid[i, j] = 0;
                        break;
                }
            }
        }
        return (grid, currentPosition, ToIndex(dir));
    }
    
    public static int ToIndex(char dir) => dir switch{ '^' => 0, '>' => 1, 'v' => 2, '<' => 3, _ => 4 };
}