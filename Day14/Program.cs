using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

namespace Day14;

class Program
{
    static void Main(string[] args)
    {
        var t = new TimeOnly();

        //Console.WriteLine(SolveFile("..\\..\\..\\\\input.txt"));
        SolveFile2("..\\..\\..\\\\input.txt");
        Console.WriteLine((new TimeOnly() - t).TotalNanoseconds);
    }

    private static void SolveFile2(string inputFile)
    {
        var robots = File.ReadAllLines(inputFile)
            .Select(ParseLine)
            .ToList();
        ConsoleKey key;
        int moves = 82;

        robots.ForEach(robot => robot.Move(82));
        PrintSpace(robots);
        Console.WriteLine(moves);
        while ((key = Console.ReadKey(true).Key) != ConsoleKey.Escape)
        {
            if (key == ConsoleKey.RightArrow)
            {
                robots.ForEach(robot => robot.Move(101));
                moves += 101;
            }
            else if (key == ConsoleKey.LeftArrow)
            {
                robots.ForEach(robot => robot.Move(-101));
                moves -= 101;
            }
            PrintSpace(robots);
            Console.WriteLine(moves);
        }
    }

    private static void PrintSpace(List<Robot> robots)
    {
        Console.SetCursorPosition(0, 0);
        int[,] space = new int[Robot.SPACE_Y, Robot.SPACE_X];
        foreach (var robot in robots)
        {
            space[robot._py, robot._px]++;
        }
        StringBuilder sb = new StringBuilder();
        for (int y = 0; y < Robot.SPACE_Y; y++)
        {
            /*if (y == Robot.SPACE_Y / 2)
            {
                Console.WriteLine();
                continue;
            }*/
            for (int x = 0; x < Robot.SPACE_X; x++)
            {
                /*if (x == Robot.SPACE_X / 2)
                {
                    Console.Write(' ');

                }
                else*/ if (space[y, x] == 0)
                {
                    sb.Append('.');
                }
                else
                {
                    sb.Append(space[y, x]);
                }
            }

            sb.AppendLine();
        }
        Console.WriteLine(sb);
    }

    private static int SolveFile(string inputFile)
    {
        var robotsAfter = File.ReadAllLines(inputFile)
            .Select(ParseLine)
            .Select(robot => robot.Move(100))
            .ToList();
        int[,] space = new int[Robot.SPACE_Y, Robot.SPACE_X];
        foreach (var place in robotsAfter)
        {
            space[place.y, place.x]++;
        }

        for (int y = 0; y < Robot.SPACE_Y; y++)
        {
            /*if (y == Robot.SPACE_Y / 2)
            {
                Console.WriteLine();
                continue;
            }*/
            for (int x = 0; x < Robot.SPACE_X; x++)
            {
                /*if (x == Robot.SPACE_X / 2)
                {
                    Console.Write(' ');
                    
                }
                else*/ if (space[y, x] == 0)
                {
                    Console.Write('.');
                }
                else
                {
                    Console.Write(space[y, x]);
                }
            }

            Console.WriteLine();
        }

        var quadrantCount = robotsAfter
            .GroupBy(Quadrant)
            .Where(g => g.Key >= 0)
            .Select(g => g.Count())
            .ToList();
        Console.WriteLine(string.Join(", ", quadrantCount));
        return quadrantCount.Count < 4 ?  0 : quadrantCount
            .Aggregate(1, (acc, cur) => acc * cur);
    }

    private static int Quadrant((int x, int y) arg)
    {
        if (arg.x == Robot.SPACE_X / 2 || arg.y == Robot.SPACE_Y / 2)
        {
            return -1;
        }

        return (arg.x < Robot.SPACE_X / 2 , arg.y < Robot.SPACE_Y / 2) switch
        {
            (true, true) => 0,
            (true, false) => 1,
            (false, true) => 2,
            (false, false) => 3,
        };
    }

    private static Robot ParseLine(string arg)
    {
        Regex re = new Regex("p=([-]?\\d+),([-]?\\d+) v=([-]?\\d+),([-]?\\d+)");
        Match m = re.Match(arg);
        return new Robot(int.Parse(m.Groups[1].Value), int.Parse(m.Groups[2].Value),
            int.Parse(m.Groups[3].Value), int.Parse(m.Groups[4].Value));
    }

    private class Robot(int px, int py, int vx, int vy)
    {
        public int _px = px;
        public int _py = py;
        public static int SPACE_X = 101;
        public static int SPACE_Y = 103;

        public (int x, int y) Move(int steps)
        {
            _px = Mod(_px + steps * vx, SPACE_X);
            _py = Mod(_py + steps * vy, SPACE_Y);
            return ( _px, _py);
        }
    }

    static int Mod(int x, int m)
    {
        int r = x % m;
        return r < 0 ? r + m : r;
    }
}
