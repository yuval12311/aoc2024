using System.Net.Sockets;
using System.Text.RegularExpressions;

namespace Day14;

class Program
{
    static void Main(string[] args)
    {
        var t = new TimeOnly();

        Console.WriteLine(SolveFile("..\\..\\..\\\\input.txt"));

        Console.WriteLine((new TimeOnly() - t).TotalNanoseconds);
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

    private record struct Robot(int px, int py, int vx, int vy)
    {
        public static int SPACE_X = 101;
        public static int SPACE_Y = 103;

        public (int x, int y) Move(int steps)
        {
            px = Mod(px + steps * vx, SPACE_X);
            py = Mod(py + steps * vy, SPACE_Y);
            return (px, py);
        }
    }

    static int Mod(int x, int m)
    {
        int r = x % m;
        return r < 0 ? r + m : r;
    }
}
