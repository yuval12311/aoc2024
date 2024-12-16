using System.Security.Cryptography;
using System.Text;

namespace Day16;

class Program
{
    static void Main(string[] args)
    {
        var t = new TimeOnly();

        //Console.WriteLine(SolveFile("..\\..\\..\\\\input.txt"));
        Console.WriteLine(SolveFile("..\\..\\..\\\\input.txt"));
        Console.WriteLine((new TimeOnly() - t).TotalNanoseconds);
    }


    private static int SolveFile(string inputFile)
    {
        var lines = File.ReadAllLines(inputFile);
        (Dictionary<PointAndDir, int> costs, Dictionary<PointAndDir, List<PointAndDir>> parents) = ParseInput(lines);

        Dictionary<PointAndDir, int> Graph(PointAndDir pointAndDir)
        {
            var dictionary = new Dictionary<PointAndDir, int> { [pointAndDir with { Dir = (pointAndDir.Dir - 1 + 4) % 4 }] = 1000, [pointAndDir with { Dir = (pointAndDir.Dir + 1 + 4) % 4 }] = 1000, };
            var nextI = pointAndDir.I + MoveInIDir(pointAndDir.Dir);
            var nextJ = pointAndDir.J + MoveInJDir(pointAndDir.Dir);
            if (lines[nextI][nextJ] == '.' || lines[nextI][nextJ] == 'E')
            {
                dictionary[pointAndDir with { I = nextI, J = nextJ }] = 1;
            }

            return dictionary;
        }

        DijkstraAlgorithm.FindShortestPath(Graph, costs, parents);
        HashSet<(int i, int j)> onBestTrails =  GetBestTrails(parents, 
            Enumerable.Range(0, 4)
                .Select(dir => new PointAndDir { I= 1, J = lines[0].Length -2, Dir = dir })
                .MinBy(pointAndDir => costs[pointAndDir])
            );
        //PrintTrails(lines, onBestTrails);
        Console.WriteLine(onBestTrails.Count);
        return Enumerable.Range(0, 4).Select(dir => costs[new PointAndDir(1, lines.Length - 2, dir)]).Min();
    }

    private static void PrintTrails(string[] lines, HashSet<(int i, int j)> onBestTrails)
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < lines.Length; i++)
        {
            for (int j = 0; j < lines[0].Length; j++)
            {
                if (onBestTrails.Contains((i, j)))
                {
                    sb.Append('O');
                }
                else
                {
                    sb.Append(lines[i][j]);
                }
            }
            sb.AppendLine();
        }

        Console.WriteLine(sb.ToString());
    }

    private static HashSet<(int i, int j)> GetBestTrails(Dictionary<PointAndDir,List<PointAndDir>> parents, PointAndDir startingPoint)
    {
        HashSet<(int i, int j)> path = [];
        Queue<PointAndDir> queue = new();
        
        queue.Enqueue(startingPoint);
        while (queue.Count > 0)
        {
            var pointAndDir = queue.Dequeue();
            path.Add((pointAndDir.I, pointAndDir.J));
            foreach (var parent in parents[pointAndDir])
            {
                queue.Enqueue(parent);
            }
        }
        return path;
    }


    public static int MoveInIDir(int dir) => dir switch { 1 => -1, 3 => 1, _ => 0 };
    public static int MoveInJDir(int dir) => dir switch { 0 => 1, 2 => -1, _ => 0 };

    
    private static (Dictionary<PointAndDir, int> costs, Dictionary<PointAndDir, List<PointAndDir>> parents) ParseInput(string[] lines)
    {
        var costs = new Dictionary<PointAndDir, int>();
        var parents = new Dictionary<PointAndDir, List<PointAndDir>>();
        for (int i = 0; i < lines.Length; i++)
        {
            for (int j = 0; j < lines[0].Length; j++)
            {
                if (lines[i][j] == 'S')
                {
                    parents.Add(new PointAndDir(i, j, 1), [new PointAndDir(i, j, 0)]);
                    parents.Add(new PointAndDir(i, j, 0), []);
                    parents.Add(new PointAndDir(i, j, 3), [new PointAndDir(i, j, 0)]);
                    costs.Add(new PointAndDir(i, j, 1), 1000);
                    costs.Add(new PointAndDir(i, j, 0), 0);
                    costs.Add(new PointAndDir(i, j, 3), 1000);
                    costs.Add(new PointAndDir(i, j, 2), int.MaxValue);
                    continue;
                }
                for (int dir = 0; dir < 4; dir++)
                {
                    if (lines[i][j] == '.' && lines[i][j-1] == 'S' && dir == 0)
                    {
                        parents.Add(new PointAndDir(i, j, 0), [new PointAndDir(i, j-1, 0)]);
                        costs.Add(new PointAndDir(i, j, 0), 1);
                    }
                    else if (lines[i][j] == '.' || lines[i][j] == 'E')
                    {
                        costs.Add(new PointAndDir(i, j, dir), int.MaxValue);
                    }
                }
            }
        }
        return (costs, parents);
    }

    public record struct PointAndDir(int I, int J, int Dir);
    
    public static class DijkstraAlgorithm
    {
        public static void FindShortestPath<T>(Func<T, Dictionary<T, int>> graph,
            Dictionary<T, int> costs,
            Dictionary<T, List<T>> parents) 
        {
            // Store the visited nodes
            var visited = new HashSet<T>();

            var isDone = FindLowestCostUnvisitedNode(costs, visited, out var lowestCostUnvisitedNode);
            while (!isDone)
            {
                var cost = costs[lowestCostUnvisitedNode];
                var neighbors = graph(lowestCostUnvisitedNode);

                foreach (var neighbor in neighbors)
                {
                    // New cost to get to the neighbor of lowestCostUnvisitedNode
                    var newCost = cost + neighbor.Value;
                    if (newCost < costs[neighbor.Key])
                    {
                        costs[neighbor.Key] = newCost;
                        parents[neighbor.Key] = [lowestCostUnvisitedNode];
                    }
                    else if (newCost == costs[neighbor.Key])
                    {
                        parents[neighbor.Key].Add(lowestCostUnvisitedNode);
                    }
                }

                visited.Add(lowestCostUnvisitedNode);
                isDone = FindLowestCostUnvisitedNode(costs, visited, out lowestCostUnvisitedNode);
                if (visited.Count % 100 == 0)
                {
                    Console.SetCursorPosition(0, 0);
                    Console.WriteLine(visited.Count + "/" +costs.Count );
                }
            }
        }

        private static bool FindLowestCostUnvisitedNode<T>(Dictionary<T, int> costs, HashSet<T> visited, out T lowestCostUnvisitedNode)
        {
            var unvisitedNode = costs
                .Where(node => !visited.Contains(node.Key))
                .OrderBy(node => node.Value)
                .ToList();
            lowestCostUnvisitedNode = unvisitedNode.FirstOrDefault().Key;
            return unvisitedNode.Count == 0;
        }
    }
}