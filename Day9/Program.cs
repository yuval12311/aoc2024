namespace Day9;

class Program
{
    static void Main(string[] args)
    {
        var t = new TimeOnly();
        
        Console.WriteLine(SolveFile("..\\..\\..\\\\input.txt"));

        Console.WriteLine((new TimeOnly() - t).TotalNanoseconds);
    }

    private static long SolveFile(string inputFile)
    {
        var str = File.ReadAllText(inputFile);
        List<(int id, int lenght)> originalBlocks = str.Select((c, i) => (i % 2 == 0 ? i / 2 : -1, c - '0')).ToList();
        List<(int id, int length)> newBlocks = new List<(int, int)>();
        int lastBloc = originalBlocks.Count - 1;
        int currentBloc = 0;
        int currentId = 0;
        while (currentBloc <= lastBloc)
        {
            if (originalBlocks[currentBloc].id != -1)
            {
                newBlocks.Add(originalBlocks[currentBloc]);
                currentBloc++;
            } else if (originalBlocks[lastBloc].id == -1)
            {
                lastBloc--;
            }
            else
            {
                var lastBlockLenght = originalBlocks[lastBloc].lenght;
                var currentBlockLength = originalBlocks[currentBloc].lenght;
                if (lastBlockLenght > currentBlockLength)
                {
                    newBlocks.Add((originalBlocks[lastBloc].id, currentBlockLength));
                    originalBlocks[lastBloc] = (originalBlocks[lastBloc].id, lastBlockLenght - currentBlockLength);
                    currentBloc++;
                }
                else if (lastBlockLenght < currentBlockLength)
                {
                    newBlocks.Add((originalBlocks[lastBloc].id, lastBlockLenght));
                    originalBlocks[currentBloc] = (originalBlocks[currentBloc].id, currentBlockLength - lastBlockLenght);
                    lastBloc--;
                }
                else
                {
                    newBlocks.Add((originalBlocks[lastBloc].id, currentBlockLength));
                    lastBloc--;
                    currentBloc++;
                }
            }
        }
        
        //Console.WriteLine(string.Join(", ", newBlocks));
        return newBlocks.Aggregate(new {Index=0L, Sum = 0L}, (acc, block) => 
            new {Index = acc.Index + block.length, Sum = acc.Sum + block.id * (acc.Index * block.length + block.length * (block.length - 1) / 2)}).Sum;
         
    }
}