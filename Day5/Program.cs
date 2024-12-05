using System.Diagnostics;
using System.Xml;

class Program
{
    public static void Main()
    {
        Console.WriteLine(SolveFile("..\\..\\..\\\\input.txt"));
    }

    private static int SolveFile(string inputFile)
    {
        DAG dag = new DAG();
        string[] lines = System.IO.File.ReadAllLines(inputFile);
        int i = 0;
        while (i < lines.Length && lines[i] != "")
        {
            dag.AddEdge(lines[i].Split("|"));
            ++i;
        }

        i++;
        int count = 0;
        for (; i < lines.Length; ++i)
        {
            var nodes = lines[i].Split(",");
            if (!dag.VerifyOrder(nodes))
            {
                count += int.Parse(dag.CreateSubset(nodes).TopologicalSort()[nodes.Length / 2]);
            }
        }
        return count;
    }
}

class DAG
{
    private class Node
    { 
        public bool IsStart => From.Count == 0;
        public string Name;
        public HashSet<Node> To = new();
        public HashSet<Node> From = new();
    }

    private Dictionary<string, Node> nodes = new();
    private Dictionary<(string, string), int> edges = new();
    public void AddEdge(params string[] edge)
    {
        if (!nodes.ContainsKey(edge[0]))
        {
            nodes.Add(edge[0], new Node() {Name = edge[0]});
        }
        if (!nodes.ContainsKey(edge[1]))
        {
            nodes.Add(edge[1], new Node() { Name = edge[1] });
        }
        nodes[edge[0]].To.Add(nodes[edge[1]]);
        nodes[edge[1]].From.Add(nodes[edge[0]]);
        edges[(edge[0], edge[1])] = 1;
    }

    public DAG CreateSubset(string[] subsetNodes)
    {
        HashSet<string> nodesSet = new HashSet<string>(subsetNodes);
        DAG dag = new DAG();
        foreach (var edge in edges.Keys)
        {
            if (nodesSet.Contains(edge.Item1) && nodesSet.Contains(edge.Item2))
            {
                dag.AddEdge(edge.Item1, edge.Item2);    
            }
        }
        return dag;
    }

    public int this[string from, string to]
    {
        get => edges.GetValueOrDefault((from, to), -edges.GetValueOrDefault((to, from), 0));
    }

    public bool VerifyOrder(string[] split)
    {
        for (int i = 0; i < split.Length; ++i)
        {
            for (int j = i + 1; j < split.Length; ++j)
            {
                if (this[split[i], split[j]] == -1)
                {
                    return false;
                }
            }
        }
        return true;
    }

    public List<string> TopologicalSort()
    {
        HashSet<Node> startes = nodes.Values.Where(node => node.IsStart).ToHashSet();
        List<Node> res = [];
        while (startes.Count > 0)
        {
            Node current = startes.First();
            startes.Remove(current);
            res.Add(current);
            foreach (var node in current.To)
            {
                node.From.Remove(current);
                edges.Remove((current.Name, node.Name));
                if (node.IsStart)
                {
                    startes.Add(node);
                }
            }
        }
        return res.Select(n => n.Name).ToList<string>();
    }


}