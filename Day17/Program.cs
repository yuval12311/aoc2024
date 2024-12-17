using System.Diagnostics;

namespace Day17;

class Program
{
    static void Main(string[] args)
    {
        var t = new TimeOnly();

        //Console.WriteLine(SolveFile("..\\..\\..\\\\input.txt"));
        Console.WriteLine(SolveFile("..\\..\\..\\\\input.txt"));
        Console.WriteLine((new TimeOnly() - t).TotalNanoseconds);
    }

    private static string SolveFile(string inputFile)
    {
        var txt = File.ReadAllText(inputFile);
        var split = txt.Split(Environment.NewLine + Environment.NewLine);
        Computer computer = Computer.FromString(split[0]);
        List<int> result = computer.Run(GetProgram(split[1]));
        return string.Join(',', result);
    }

    public class Computer(int a, int b, int c)
    {
        private int A = a;
        private int B = b;
        private int C = c;

        public static Computer FromString(string program)
        {
            var split = program.Split(Environment.NewLine);
            
            return new Computer(GetRegister(split[0]), GetRegister(split[1]), GetRegister(split[2]));
        }

        private static int GetRegister(string line)
        {
            return int.Parse(line.Substring(12, line.Length - 12));
        }

        public List<int> Run(List<int> program)
        {
            List<int> result = new List<int>();
            int instructionPointer = 0;
            while (instructionPointer < program.Count)
            {
                var operand = program[instructionPointer + 1];
                switch (program[instructionPointer])
                {
                    case 0:
                        A /= TwoExponent(operand);
                        instructionPointer += 2;
                        break;
                    case 1:
                        B ^= operand;
                        instructionPointer += 2;
                        break;
                    case 2:
                        B = Combo(operand) % 8;
                        instructionPointer += 2;
                        break;
                    case 3:
                        if (A != 0)
                        {
                            instructionPointer = operand;
                        }
                        else
                        {
                            instructionPointer += 2;
                        }
                        break;
                    case 4:
                        B ^= C;
                        instructionPointer += 2;
                        break;
                    case 5:
                        result.Add(Combo(operand) % 8);
                        instructionPointer += 2;
                        break;
                    case 6:
                        B = A / TwoExponent(operand);
                        instructionPointer += 2;
                        break;
                    case 7:
                        C = A / TwoExponent(operand);
                        instructionPointer += 2;
                        break;
                }
            }
            return result;
        }

        private int TwoExponent(int operand)
        {
            if (Combo(operand) > 0)
            {
                return 2 << (Combo(operand) - 1);
            }
            else
            {
                return 1;
            }
        }

        private int Combo(int operand) =>
            operand switch
            {
                4 => A,
                5 => B,
                6 => C,
                _ => operand
            };
    }

    private static List<int> GetProgram(string str)
    {
        var startIndex = str.IndexOf(' ');  
        return str.Substring(startIndex, str.Length - startIndex)
            .Split(',')
            .Select(int.Parse)
            .ToList();
    }
}