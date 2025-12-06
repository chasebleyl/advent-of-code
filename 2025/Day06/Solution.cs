namespace AdventOfCode.Y2025.Day06;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Numerics;

[ProblemName("Trash Compactor")]
class Solution : Solver {

    public object PartOne(string input) {
        var problemGrid = input.Split("\n")
            .Select(row => row
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .ToList())
            .ToList();

        long grandTotal = 0;
        for (var column = 0; column < problemGrid[0].Count; column++) {
            var operation = problemGrid[^1][column];
            var currentCalcValue = long.Parse(problemGrid[0][column]);
            for (var row = 1; row < problemGrid.Count - 1; row++) {
                if (operation == "+") currentCalcValue += long.Parse(problemGrid[row][column]);
                else currentCalcValue *= long.Parse(problemGrid[row][column]);
            }

            grandTotal += currentCalcValue;
        }

        return grandTotal;
    }

    public object PartTwo(string input) {
        long grandTotal = 0;
        var inputRows = input.Split("\n");

        var currentNums = new List<long>();
        for (var column = inputRows[0].Length - 1; column >= 0; column--) {
            var currentNum = "";
            for(var row = 0; row < inputRows.Length - 1; row++) {
                currentNum += inputRows[row][column];
            }
            if (currentNum.Trim() != "") currentNums.Add(long.Parse(currentNum));
            if (inputRows[^1][column] == '+') {
                grandTotal += currentNums.Sum();
                currentNums = new List<long>();
            }
            else if (inputRows[^1][column] == '*') {
                grandTotal += currentNums.Aggregate(1L, (seed, value) => seed * value);
                currentNums = new List<long>();
            }
        }

        return grandTotal;
    }
}
