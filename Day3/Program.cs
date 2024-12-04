// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using System.Text.RegularExpressions;

class Program
{
    public static void Main()
    {
        Console.WriteLine(SolveFile("..\\..\\..\\\\input.txt"));
        Console.WriteLine(SolveFile2("..\\..\\..\\\\input.txt"));
    }

    private static int SolveFile2(string inputFile)
    {
        bool isDo = true;
        var re = new Regex(@"do\(\)|don't\(\)|mul\((\d{1,3}),(\d{1,3})\)");
        return re.Matches(File.ReadAllText(inputFile))
            .Select(match =>
                {
                    switch (match.Groups[0].Value)
                    {
                        case "do()":
                            isDo = true;
                            break;
                        case "don't()":
                            isDo = false;
                            break;
                        default:
                            return isDo ? int.Parse(match.Groups[1].Value) * int.Parse(match.Groups[2].Value) : 0;
                    }
                    return 0;
                }
            )
            .Sum();
    }

    private static int SolveFile(string inputFile)
    {
        var re = new Regex(@"mul\((\d{1,3}),(\d{1,3})\)");
        return re.Matches(File.ReadAllText(inputFile))
            .Select(match => int.Parse(match.Groups[1].Value) * int.Parse(match.Groups[2].Value))
            .Sum();
    }
}