namespace AdventOfCode.Y2025.Day07;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Numerics;
using System.Globalization;
using AngleSharp.Common;

record Pos(int Row, int Col);

[ProblemName("Laboratories")]
class Solution : Solver {

    public object PartOne(string input) {
        string[] inputLines = input.Split('\n');
        bool[] manifoldBeamActivated = new bool[inputLines[0].Length];
        int startingIndex = inputLines[0].IndexOf('S');
        manifoldBeamActivated[startingIndex] = true;
        int beamSplitCount = 0;
        for (var i = 1; i < inputLines.Length; i++) {
            for (var j = 1; j < inputLines[i].Length - 1; j++) {
                if (inputLines[i][j] == '^' && manifoldBeamActivated[j] == true) {
                    manifoldBeamActivated[j-1] = true;
                    manifoldBeamActivated[j] = false;
                    manifoldBeamActivated[j+1] = true;
                    beamSplitCount += 1;
                }
            }
        }
        return beamSplitCount;
    }

    public object PartTwo(string input) {
        string[] inputLines = input.Split('\n');
        int startingIndex = inputLines[2].IndexOf('^');
        var cache = new Dictionary<Pos, long>();
        return CountBeamPaths(inputLines, new Pos(2, startingIndex), cache);
    }

    private long CountBeamPaths(string[] inputGrid, Pos current, Dictionary<Pos, long> cache) {
        if (cache.TryGetValue(current, out long cached)) {
            return cached;
        }

        var leftSplitter = FindNextSplitter(inputGrid, current.Row, current.Col - 1);
        var rightSplitter = FindNextSplitter(inputGrid, current.Row, current.Col + 1);

        long leftPaths = leftSplitter != null ? CountBeamPaths(inputGrid, leftSplitter, cache) : 1;
        long rightPaths = rightSplitter != null ? CountBeamPaths(inputGrid, rightSplitter, cache) : 1;

        long pathCount = leftPaths + rightPaths;
        cache[current] = pathCount;
        return pathCount;
    }

    private Pos? FindNextSplitter(string[] inputGrid, int startRow, int col) {
        for (var row = startRow + 1; row < inputGrid.Length; row++) {
            if (inputGrid[row][col] == '^') {
                return new Pos(row, col);
            }
        }
        return null;
    }
}
