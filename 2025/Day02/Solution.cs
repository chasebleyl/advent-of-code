namespace AdventOfCode.Y2025.Day02;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Numerics;

[ProblemName("Gift Shop")]
class Solution : Solver {

    public object PartOne(string input) {
        ProductIdRange[] ranges = GetArrayInput(input);
        long accumulatedSum = 0;
        foreach (ProductIdRange range in ranges) {
            for (long i = range.ProductIdMinimumLong; i <= range.ProductIdMaximumLong; i++) {
                if (substringRepeatedTwice(i.ToString())) accumulatedSum += i;
            }
        }
        return accumulatedSum;
    }

    private bool substringRepeatedTwice(string input) {
        if (string.IsNullOrEmpty(input)) return false;
        if (input.Length % 2 != 0) return false;

        return input[..(input.Length / 2)] == input[(input.Length / 2)..];
    }

    public object PartTwo(string input) {
        ProductIdRange[] ranges = GetArrayInput(input);
        long accumulatedSum = 0;
        foreach (ProductIdRange range in ranges) {
            for (long i = range.ProductIdMinimumLong; i <= range.ProductIdMaximumLong; i++) {
                if (substringRepeatedAtLeastTwice(i.ToString())) {
                    accumulatedSum += i;
                }
            }
        }
        return accumulatedSum;
    }

    private bool substringRepeatedAtLeastTwice(string input) {
        if (string.IsNullOrEmpty(input)) return false;

        for (int i = 1; i <= input.Length / 2; i++) {
            if (input.Length % i != 0) continue;
            string sub = input[..i];
            if (string.Concat(Enumerable.Repeat(sub, input.Length / i)) == input)
                return true;
        }

        return false;
    }

    private ProductIdRange[] GetArrayInput(string input) => input
        .Split(",", StringSplitOptions.RemoveEmptyEntries)
        .Select(line => new ProductIdRange(line.Trim()))
        .ToArray();
}

public class ProductIdRange 
{
    public string ProductIdMinimum;
    public long ProductIdMinimumLong;
    public string ProductIdMaximum;
    public long ProductIdMaximumLong;

    public ProductIdRange(string input) 
    {
        string[] parts = input.Split('-');
        ProductIdMinimum = parts[0];
        ProductIdMinimumLong = long.Parse(ProductIdMinimum);
        ProductIdMaximum = parts[1];
        ProductIdMaximumLong = long.Parse(ProductIdMaximum);
    }
}
