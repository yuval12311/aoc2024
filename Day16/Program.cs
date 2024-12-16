using System.Security.Cryptography;

namespace Day16;

class Program
{
    static void Main(string[] args)
    {
        var t = new TimeOnly();

        //Console.WriteLine(SolveFile("..\\..\\..\\\\input.txt"));
        Console.WriteLine(SolveFile("..\\..\\..\\\\test.txt"));
        Console.WriteLine((new TimeOnly() - t).TotalNanoseconds);
    }


    private static int SolveFile(string inputFile)
    {\
        var lines = File.ReadAllLines(inputFile);
        (Dictionary<PointAndDir, int> costs, Dictionary<PointAndDir, PointAndDir> parents) = ParseInput(lines);

        Dictionary<PointAndDir, int> Graph(PointAndDir pointAndDir)
        {
            var dictionary = new Dictionary<PointAndDir, int> { [pointAndDir with { Dir = (pointAndDir.Dir - 1) % 4 }] = 1000, [pointAndDir with { Dir = (pointAndDir.Dir + 1) % 4 }] = 1000, };
            var nextI = pointAndDir.I + MoveInIDir(pointAndDir.Dir);
            var nextJ = pointAndDir.J + MoveInJDir(pointAndDir.Dir);
            if (lines[nextI][nextJ] == '.')
            {
                dictionary[pointAndDir with { I = nextI, J = nextJ }] = 0;
            }

            return dictionary;
        }

        DijkstraAlgorithm.FindShortestPath(Graph, costs, parents);
        return Enumerable.Range(0, 4).Select(dir => costs[new PointAndDir(1, lines.Length - 2, dir)]).Min();
    }
    
    public static int Test<T>(List<T?> list) => 0;

    public static int MoveInIDir(int dir) => dir switch { 1 => -1, 3 => 1, _ => 0 };
    public static int MoveInJDir(int dir) => dir switch { 0 => 1, 2 => -1, _ => 0 };

    
    private static (Dictionary<PointAndDir, int> costs, Dictionary<PointAndDir, PointAndDir> parents) ParseInput(string[] lines)
    {
        var costs = new Dictionary<PointAndDir, int>();
        var parents = new Dictionary<PointAndDir, PointAndDir?>();
        for (int i = 0; i < lines.Length; i++)
        {
            for (int j = 0; j < lines[0].Length; j++)
            {
                if (lines[i][j] == 'S')
                {
                    parents.Add(new PointAndDir(i, j, 1), new PointAndDir(i, j, 0));
                    parents.Add(new PointAndDir(i, j, 3), new PointAndDir(i, j, 0));
                    costs.Add(new PointAndDir(i, j, 1), 1000);
                    costs.Add(new PointAndDir(i, j, 3), 1000);
                    continue;
                }
                for (int dir = 0; dir < 4; dir++)
                {
                    if (lines[i][j] == '.' && lines[i][j-1] == 'S' && dir == 0)
                    {
                        parents.Add(new PointAndDir(i, j, 0), new PointAndDir(i, j-1, 0));
                        costs.Add(new PointAndDir(i, j, 0), 1);
                    }
                    else if (lines[i][j] == '.' || lines[i][j] == 'E')
                    {
                        parents.Add(new PointAndDir(i, j, dir), null);
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
            Dictionary<T, T?> parents) 
        {
            // Store the visited nodes
            var visited = new HashSet<T>();

            var lowestCostUnvisitedNode = FindLowestCostUnvisitedNode(costs, visited);
            while (lowestCostUnvisitedNode != null)
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
                        parents[neighbor.Key] = lowestCostUnvisitedNode;
                    }
                }

                visited.Add(lowestCostUnvisitedNode);
                lowestCostUnvisitedNode = FindLowestCostUnvisitedNode(costs, visited);
            }
        }

        private static T? FindLowestCostUnvisitedNode<T>(Dictionary<T, int> costs, HashSet<T> visited)
        {
            return costs
                .Where(node => !visited.Contains(node.Key))
                .OrderBy(node => node.Value)
                // When .Where above returns empty collection, this will give default KeyValuePair: { Key = null, Value = 0 }
                .FirstOrDefault()
                .Key;
        }
    }
}