using System.Text;

namespace Day15;

class Program
{
    static void Main(string[] args)
    {
        var t = new TimeOnly();

        //Console.WriteLine(SolveFile("..\\..\\..\\\\input.txt"));
        Console.WriteLine(
       SolveFile2("..\\..\\..\\\\input.txt"));
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
    private static int SolveFile2(string inputFile)
    {
        var txt = File.ReadAllText(inputFile);
        var splitted = txt.Split($"{Environment.NewLine}{Environment.NewLine}");
        var warehouse = splitted[0].Split(Environment.NewLine);
        var moves = string.Join("", splitted[1].Split(Environment.NewLine)).Select(GetDirection).ToList();
        (byte[,] map, int robotI, int robotJ) = GetMap2(warehouse);
        foreach (var dir in moves)
        {
            (robotI, robotJ) = MakeMove2(map, dir, ref robotI, ref robotJ);
            PrintMap2(map);
            Console.WriteLine(dir);
            Thread.Sleep(2);
            //Console.ReadKey();
        }

        Console.WriteLine(map[robotI, robotJ]);
        return GetGPSCoordinateSum(map);
    }

    private static String PrintMap2(byte[,] map)
    {
        Console.SetCursorPosition(0, 0);
        var sb = new StringBuilder();
        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                sb.Append((char)map[i, j]);
            }
            sb.AppendLine();
        }

        var printMap = sb.ToString();
        Console.WriteLine(printMap);
        return printMap;
    }

    private static (int i , int j) MakeMove2(byte[,] map, Direction dir, ref int robotI, ref int robotJ)
    {
        if (IsMoveFree(map, dir, robotI, robotJ))
        {
            MakeMoveRec(map, dir, robotI, robotJ);
            return MoveInDir(dir, robotI, robotJ);
        };
        return (robotI, robotJ);
    }

    private static void MakeMoveRec(byte[,] map, Direction dir,  int i,  int j)
    {
        if (map[i, j] == '.') return;
        var next = MoveInDir(dir, i, j);
        
        if (dir == Direction.Left || dir == Direction.Right)
        {
            
            MakeMoveRec(map, dir,  next.i,  next.j);
        }
        else if (map[next.i, next.j] != '.') 
        {
            
            var secondPart = map[next.i, next.j] == '[' ? 1 : -1;
            MakeMoveRec(map, dir, next.i, next.j); 
            MakeMoveRec(map, dir, next.i , next.j + secondPart);
        }
        map[next.i, next.j] = map[i, j] ;
        map[i, j] = (byte)'.';
    }

    private static bool IsMoveFree(byte[,] map, Direction dir, int i,  int j)
    {
        if (map[i, j] == '#') return false;
        if (map[i, j] == '.') return true;
        var next = MoveInDir(dir, i, j);
        if (map[i, j] == '@') return IsMoveFree(map, dir, next.i, next.j);
        if (dir == Direction.Left || dir == Direction.Right)
        {
            return IsMoveFree(map, dir, next.i, next.j);
        }
        else
        {
            var nextSecond = map[i, j] == '[' ? MoveInDir(dir, i, j + 1) : MoveInDir(dir, i, j - 1);
            return IsMoveFree(map, dir, next.i, next.j) && IsMoveFree(map, dir, nextSecond.i, nextSecond.j);
        }
    }

    private static (byte[,] map, int robotI, int robotJ) GetMap2(string[] warehouse)
    {
        byte[,] map = new byte[warehouse.Length,warehouse[0].Length * 2];
        int robotI = 0;
        int robotJ = 0;
        for (int i = 0; i < warehouse.Length; i++)
        {
            for (int j = 0; j < warehouse[0].Length; j++)
            {
                if (warehouse[i][j] == '@')
                {
                    robotI = i;
                    robotJ = 2 * j;
                    map[robotI, robotJ] = (byte)'@';
                    map[robotI, robotJ + 1] = (byte)'.';
                }
                else
                {
                    map[i, 2 * j] = warehouse[i][j] == 'O' ? (byte) '[' : (byte) warehouse[i][j];
                    map[i, 2 * j + 1] = warehouse[i][j] == 'O' ? (byte) ']' : (byte) warehouse[i][j];

                }
            }
        }
        return (map, robotI, robotJ);
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
    

    private static bool IsObstacle(byte arg) => arg == 2 || arg == '[';

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
    private static (int i, int j) MoveInDir(Direction dir, int i, int j)
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

        return (i, j);
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