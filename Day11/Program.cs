﻿namespace Day11;

class Program
{
    static void Main(string[] args)
    {
        var t = DateTime.Now;

        Console.WriteLine(SolveFile("..\\..\\..\\\\input.txt"));

        Console.WriteLine((DateTime.Now - t).ToString());
    }

    private static long SolveFile(string inputFile)
    {
        return File.ReadAllText(inputFile)
            .Split(' ')
            .Select(int.Parse)
            .Select(arg => CountStones(arg))
            .Sum();
    }

    private static long CountStones(int arg)
    {
        Dictionary<(long, int), long> memo = new Dictionary<(long, int), long>();
        return CountStones(arg, 75, memo);
    }

    private static long CountStones(long stone, int blinks, Dictionary<(long, int), long> memo)
    {
         
        if (blinks == 0)
        {
            return 1;
        }

        if (memo.ContainsKey((stone, blinks)))
        {
            return memo[(stone, blinks)];
        }

        long res;
        if (stone == 0)
        {
            res = CountStones(1, blinks - 1, memo);
        }
        else if (SplitNum(stone, out long left, out long right))
        {
            res = CountStones(left, blinks - 1, memo) + CountStones(right, blinks - 1, memo);
        }
        else
        {
            res = CountStones(stone * 2024, blinks - 1, memo);
        }

        memo[(stone, blinks)] = res;
        return res;

    }

    private static bool SplitNum(long stone, out long left, out long right)
    {
        left = stone;
        right = 0;
        var numDigits = CountDigits(stone);
        if (numDigits % 2 != 0)
        {
            return false;
        }

        long currentRightDigit = 1;
        for (int i = 0; i < numDigits/2; i++)
        {
            right += (left % 10) * currentRightDigit;
            left /= 10;
            currentRightDigit *= 10;
        }

        return true;
    }

    private static int CountDigits(long number)
    {
        int length = 1;
        
        if (number >= 10000000000000000) {
            length += 16;
            number /= 10000000000000000;
        }
        if (number >= 100000000) {
            length += 8;
            number /= 100000000;
        }
        if (number >= 10000) {
            length += 4;
            number /= 10000;
        }
        if (number >= 100) {
            length += 2;
            number /= 100;
        }
        if (number >= 10) {
            length += 1;
        }
        return length;
    }
}