using System.Text;

namespace Day15;

class Program
{
    static void Main(string[] args)
    {
        var t = new TimeOnly();

        //Console.WriteLine(SolveFile("..\\..\\..\\\\input.txt"));
        Console.WriteLine(
       SolveFile("..\\..\\..\\\\input.txt"));
        Console.WriteLine((new TimeOnly() - t).TotalNanoseconds);
    }

    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    public static Direction GetDirection(char c) => c switch
    {
        '^' => Direction.Up,
        'v' => Direction.Down,
        '<' => Direction.Left,
        '>' => Direction.Right,
        _ => throw new Exception("Invalid direction: " + c)
    };

    private static int SolveFile(string inputFile)
    {
        var txt = File.ReadAllText(inputFile);
        var splitted = txt.Split($"{Environment.NewLine}{Environment.NewLine}");
        var warehouse = splitted[0].Split(Environment.NewLine);
        var moves = string.Join("", splitted[1].Split(Environment.NewLine)).Select(GetDirection).ToList();
        (byte[,] map, int robotI, int robotJ) = GetMap(warehouse);
        foreach (var dir in moves)
        {
            MakeMove(map, dir, ref robotI, ref robotJ);
            /*PrintMap(map, robotI, robotJ);
            Thread.Sleep(20);*/
        }

        Console.WriteLine(map[robotI, robotJ]);
        return GetGPSCoordinateSum(map);
    }

    private static string PrintMap(byte[,] map, int robotI, int robotJ)
    {
        Console.SetCursorPosition(0, 0);
        var sb = new StringBuilder();
        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                if (i == robotI && j == robotJ)
                {
                    sb.Append('@');
                }
                else
                {
                    sb.Append(map[i, j] switch { 0 => '.', 1 => '#', 2 => 'O' });
                }
            }
            sb.AppendLine();
        }

        var printMap = sb.ToString();
        Console.WriteLine(printMap);
        return printMap;
    }

    private static int GetGPSCoordinateSum(byte[,] map)
    {
        var sum = 0;
        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                if (IsObstacle(map[i, j]))
                {
                    sum += i * 100 + j;
                }
            }
        }

        return sum;
    }
    

    private static bool IsObstacle(byte arg) => arg == 2;

    private static void MakeMove(byte[,] map, Direction dir, ref int robotI, ref int robotJ)
    {
        int nextFreeI = robotI;
        int nextFreeJ = robotJ;
        do
        {
            MoveInDir(dir, ref nextFreeI, ref nextFreeJ);
        } while (map[nextFreeI, nextFreeJ] == 2);

        if (map[nextFreeI, nextFreeJ] == 1)
        {
            return;
        }

        map[robotI, robotJ] = 0;
        MoveInDir(dir, ref robotI, ref robotJ);
        if (robotI != nextFreeI || robotJ != nextFreeJ)
        {
            map[nextFreeI, nextFreeJ] = 2;
        }
    }

    private static void MoveInDir(Direction dir, ref int i, ref int j)
    {
        switch (dir)
        {
            case Direction.Up:
                i -= 1;
                break;
            case Direction.Down:
                i += 1;
                break;
            case Direction.Left:
                j -= 1;
                break;
            case Direction.Right:
                j += 1;
                break;
        }
    }

    private static (byte[,] map, int robotI, int robotJ) GetMap(string[] warehouse)
    {
        byte[,] map = new byte[warehouse.Length, warehouse[0].Length];
        int robotI = 0;
        int robotJ = 0;
        for (int i = 0; i < warehouse.Length; i++)
        {
            for (int j = 0; j < warehouse[0].Length; j++)
            {
                map[i, j] = warehouse[i][j] switch { '.' => 0, '#' => 1, 'O' => 2, '@' => 0 };
                if (warehouse[i][j] == '@')
                {
                    robotI = i;
                    robotJ = j;
                }
            }
        }

        return (map, robotI, robotJ);
    }
}