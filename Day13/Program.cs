using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.X86;
using System.Text.RegularExpressions;
using Rationals;

namespace Day13;

class Program
{
    static void Main(string[] args)
    {
        var t = new TimeOnly();

        Console.WriteLine(SolveFile("..\\..\\..\\\\input.txt"));
        //Console.WriteLine(SolveFile2("..\\..\\..\\\\input.txt"));

        Console.WriteLine((new TimeOnly() - t).TotalNanoseconds);
    }

    private static long SolveFile(string inputFile)
    {
        return GetMatrices(inputFile)
            .Select(pair =>
            {
                if (pair.matrix.Determinant.IsZero)
                {
                    if (pair.vector.IsCoplanar(pair.matrix.Left))
                    {
                        var leftMult = pair.vector.X / pair.matrix.A;
                        var rightMult = pair.vector.X / pair.matrix.B;
                        if ((!IsInt(rightMult) || 3 * leftMult < rightMult) && IsInt(leftMult))
                        {
                            return new RationalVector(leftMult, 0);
                        }
                        else if (IsInt(rightMult))
                        {
                            return new RationalVector(0, rightMult);
                        }
                        return new RationalVector(0, 0);
                    }
                }
                return pair.matrix.Inverse() * pair.vector;
            })
            .Where(vec => IsInt(vec.X) && IsInt(vec.Y))
            .Select(vec => 3 * (long)vec.X + (long)vec.Y)
            .Sum();
        
    }

    private static bool IsInt(Rational arg)
    {
        
        return arg.FractionPart == 0;
    }

    private record RationalVector(Rational X, Rational Y)
    {
        public bool IsCoplanar(RationalVector other) => new RationalMatrix(X , other.X, Y , other.Y).Determinant == 0;
    };

    private readonly record struct RationalMatrix(Rational A, Rational B, Rational C, Rational D)
    {
        public static RationalVector operator *(RationalMatrix M, RationalVector V) =>
            new RationalVector(
                M.A * V.X + M.B * V.Y,
                M.C * V.X + M.D * V.Y
            );

        public static RationalMatrix operator *(RationalMatrix M, RationalMatrix M2) =>
            new RationalMatrix(
                M.A * M2.A + M.B * M2.C, M.A * M2.B + M.B * M2.D,
                M.C * M2.A + M.D * M2.C, M.C * M2.B + M.D * M2.D
            );

        public static RationalMatrix operator *(int a, RationalMatrix M) =>
            new RationalMatrix(
                a * M.A, a * M.B,
                a * M.C, a * M.D
            );

        public static RationalMatrix operator /(RationalMatrix M, Rational a) =>
            new RationalMatrix(
                M.A / a, M.B / a,
                M.C / a, M.D / a
            );

        public Rational Determinant => A * D - B * C;

        public RationalMatrix Inverse() => new RationalMatrix(
            D, -B,
            -C, A
        ) / Determinant;
        
        public RationalVector Left => new RationalVector(A, C);
        public RationalVector Right => new RationalVector(B, D);
    }

    private static IEnumerable<(RationalMatrix matrix, RationalVector vector)> GetMatrices(string inputFile)
    {
        var txt = File.ReadAllText(inputFile);
        Regex regex =
            new Regex(@"Button A: X\+(\d+), Y\+(\d+)\r\nButton B: X\+(\d+), Y\+(\d+)\r\nPrize: X=(\d+), Y=(\d+)");
        return regex.Matches(txt)
            .Select(match =>
                match.Groups.OfType<Group>().Skip(1).Select(group => long.Parse(group.Value)).ToList())
            .Select(intValue =>
            {
                Console.WriteLine("got values: " + string.Join(", ", intValue));
                return (
                    new RationalMatrix(
                        intValue[0], intValue[2],
                        intValue[1], intValue[3]
                    ),
                    new RationalVector(10000000000000 + intValue[4],10000000000000 + intValue[5]));
            });
    }
}