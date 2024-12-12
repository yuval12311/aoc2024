using System.Runtime.CompilerServices;
using System.Xml.Schema;

namespace Day12;

class Program
{
    static void Main(string[] args)
    {
        var t = new TimeOnly();

        Console.WriteLine(SolveFile2("..\\..\\..\\\\input.txt"));
        //Console.WriteLine(SolveFile2("..\\..\\..\\\\input.txt"));
        
        Console.WriteLine((new TimeOnly() - t).TotalNanoseconds);
    }

    private class Region()
    {
        public int Perimiter;
        public int Count;
        public char Letter { get; set; }
    }
    class DisjointSetUBR
    {
        int[] parent;
        int[] rank; // height of tree

        public DisjointSetUBR(int size)
        {
            parent = new int[size + 1];
            rank = new int[size + 1];
        }

        public void MakeSet(int i)
        {
            parent[i] = i;
        }

        public int Find(int i)
        {
            while (i!=parent[i]) // If i is not root of tree we set i to his parent until we reach root (parent of all parents)
            {
                i = parent[i]; 
            }
            return i;
        }

        // Path compression, O(log*n). For practical values of n, log* n <= 5
        public int FindPath(int i)
        {
            if (i!=parent[i])
            {
                parent[i] = FindPath(parent[i]);
            }
            return parent[i];
        }

        public int Union(int i, int j)
        {
            int i_id = FindPath(i); // Find the root of first tree (set) and store it in i_id
            int j_id = FindPath(j); // // Find the root of second tree (set) and store it in j_id

            if (i_id == j_id) // If roots are equal (they have same parents) than they are in same tree (set)
            {
                return -1;
            }

            if (rank[i_id] > rank[j_id]) // If height of first tree is larger than second tree
            {
                parent[j_id] = i_id; // We hang second tree under first, parent of second tree is same as first tree
            }
            else
            {
                parent[i_id] = j_id; // We hang first tree under second, parent of first tree is same as second tree
                if (rank[i_id] == rank[j_id]) // If heights are same
                {
                    rank[j_id]++; // We hang first tree under second, that means height of tree is incremented by one
                }
            }
            return FindPath(i_id);
        }
    }

    private static long SolveFile(string inputFile)
    {
       
        Dictionary<int, Region> regions = new();
        var lines = File.ReadAllLines(inputFile);
        DisjointSetUBR regionSets = new(lines.Length * lines[0].Length);
        StrongBox<Region>[,] plotRegions = new StrongBox<Region>[lines.Length, lines[0].Length];
        for (int i = 0; i < lines.Length; i++)
        {
            for (int j = 0; j < lines[i].Length; j++)
            {
                regionSets.MakeSet(i*lines.Length + j);
                regions.Add(i*lines.Length + j, new Region() {Perimiter = 4, Count = 1, Letter = lines[i][j]});
                if (i > 0 && lines[i][j] == lines[i - 1][j])
                {
                    var rep1 = regionSets.FindPath(i * lines.Length + j);
                    var rep2 = regionSets.FindPath((i -1) * lines.Length + j );
                    int styingRep;
                    if ((styingRep = regionSets.Union(i * lines.Length + j, (i -1) * lines.Length + j)) > -1)
                    {
                        int otherRep = styingRep == rep2 ? rep1 : rep2;
                        regions[styingRep].Count += regions[otherRep].Count;
                        regions[styingRep].Perimiter += regions[otherRep].Perimiter - 2;
                        regions.Remove(otherRep);

                    }
                }

                if (j > 0 && lines[i][j] == lines[i][j - 1])
                {
                    var rep1 = regionSets.FindPath(i * lines.Length + j);
                    var rep2 = regionSets.FindPath(i * lines.Length + j - 1 );
                    int styingRep;
                    if ((styingRep = regionSets.Union(i * lines.Length + j, i * lines.Length + j - 1)) > -1)
                    {
                        int otherRep = styingRep == rep2 ? rep1 : rep2;
                        regions[styingRep].Count += regions[otherRep].Count;
                        regions[styingRep].Perimiter += regions[otherRep].Perimiter - 2;
                        regions.Remove(otherRep);
                    }
                    else
                    {
                        regions[rep1].Perimiter -= 2;
                    };
                }
                
            }
        }

        Console.WriteLine(string.Join("\n", regions.Values.Select(reg => new {reg.Count, reg.Perimiter, reg.Letter})));
        return regions.Values.Sum(region => (long)region.Perimiter * region.Count);
    }
    
    private static long SolveFile2(string inputFile)
    {
       
        Dictionary<int, Region> regions = new();
        var lines = File.ReadAllLines(inputFile);
        DisjointSetUBR regionSets = new(lines.Length * lines[0].Length);
        for (int i = 0; i < lines.Length; i++)
        {
            for (int j = 0; j < lines[i].Length; j++)
            {
                bool top = i > 0 && lines[i][j] == lines[i - 1][j];
                bool left = j > 0 && lines[i][j] == lines[i][j - 1];
                bool topLeft = i > 0 && j > 0 && lines[i][j] == lines[i - 1][j - 1];
                bool topRight =i > 0 &&  j < lines.Length - 1 && lines[i][j] == lines[i - 1][j + 1];
                regionSets.MakeSet(i*lines.Length + j);
                regions.Add(i * lines.Length + j, new Region() { Perimiter = 4, Count = 1, Letter = lines[i][j] });
                if (top) JoinUp(i, j, regionSets, lines ,regions);
                if (left) JoinLeft(i, j, regionSets, lines ,regions);
                if (!top && !left)
                {
                    continue;
                }
                Region current = regions[regionSets.FindPath(i * lines.Length + j)];
                if (top && left && !topLeft)
                {
                    current.Perimiter += topRight ? -4 : -6;
                }
                else if (top && left && topLeft)
                {
                    current.Perimiter += -4 + (topRight ? 0 : -2);
                }
                else if (!left)
                {
                    if (!top || !topLeft || !topRight)
                        current.Perimiter += top ^ topLeft ^ topRight ? -4 : -2;
                    
                } else if (!top)
                {
                    current.Perimiter += topLeft ? -2 : -4;
                }
            }
        }

        Console.WriteLine(string.Join("\n", regions.Values.Select(reg => new {reg.Count, reg.Perimiter, reg.Letter})));
        return regions.Values.Sum(region => (long)region.Perimiter * region.Count);
    }

    private static void JoinLeft(int i, int j, DisjointSetUBR regionSets, string[] lines, Dictionary<int, Region> regions)
    {
        Join(i *lines[0].Length + j , i *lines[0].Length + j - 1, regionSets, regions);
    }
    private static void JoinUp(int i, int j, DisjointSetUBR regionSets, string[] lines, Dictionary<int, Region> regions)
    {
        Join(i *lines[0].Length + j , (i - 1) *lines[0].Length + j, regionSets, regions);
    }

    private static void Join(int i, int j, DisjointSetUBR regionSets, Dictionary<int, Region> regions)
    {
        var rep1 = regionSets.FindPath(i );
        var rep2 = regionSets.FindPath(j);
        int styingRep;
        if ((styingRep = regionSets.Union(i, j)) > -1 )
        {
            int otherRep = styingRep == rep2 ? rep1 : rep2;
            regions[styingRep].Count += regions[otherRep].Count;
            regions[styingRep].Perimiter += regions[otherRep].Perimiter;
            regions.Remove(otherRep);
        }
    }
}