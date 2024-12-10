
namespace Day7;

class Program
{
    static void Main(string[] args)
    {
        var t = DateTime.Now;

        Console.WriteLine(SolveFile("..\\..\\..\\\\input.txt"));

        Console.WriteLine((DateTime.Now - t));
    }

    private static long SolveFile(string inputTxt)
    {
        return File
            .ReadLines(inputTxt
            )
            .Select(line =>
            {
                var split = line.Split(": ");
                return (long.Parse(split[0]), split[1].Split(' '));
            })
            .Where(pair => SolveEquation(
                pair.Item2.Select(int.Parse).ToArray(),
                pair.Item1))
            .Sum(pair =>pair.Item1);
    }
    

    public static bool SolveEquation(Span<int> input, long result)
    {
        return SolveEquationRec(input, result);
    }
    
    private static bool SolveEquationRec(Span<int> input, long result)
    {
        if (input.Length == 1)
        {
            return input[0] == result;
        }
        if (result % input[^1] == 0 && SolveEquationRec(input[..^1], result / input[^1]))
        {
            return true;
        }

        if (EndsWith(result, input[^1]) && SolveEquationRec(input[..^1], RemoveDigits(result, input[^1])))
        {
            return true;
        }

        return SolveEquationRec(input[..^1], result - input[^1]);
    }

    private static long RemoveDigits(long result , int i)
    {
        while (i > 0)
        {
            i /= 10;
            result /= 10;
        }
        return result;
    }

    private static bool EndsWith(long num1, int num2)
    {
        while (num1 > 0 && num2 > 0)
        {
            if (num2 % 10 != (num1 % 10) ) return false;
            num2 /= 10;
            num1 /= 10;
        }
        return true;
    }
}