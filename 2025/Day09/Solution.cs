namespace AdventOfCode.Y2025.Day09;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Numerics;

[ProblemName("Movie Theater")]
class Solution : Solver {

    public object PartOne(string input) {
        var tileCoords = input.Split("\n")
            .Select(line => line.Split(','))
            .Select(pair => (int.Parse(pair[0]), int.Parse(pair[1])))
            .OrderBy(pair => pair.Item1)
            .ToArray();

        long maxSizeRectangle = 0;
        for (var i = 0; i < (tileCoords.Length / 2); i++) {
            for (var j = tileCoords.Length - 1; j > (tileCoords.Length / 2); j--) {
                // Console.WriteLine($"Comparing ({tileCoords[i].Item1},{tileCoords[i].Item2}) with ({tileCoords[j].Item1},{tileCoords[j].Item2})");
                long currentSize = Math.Abs((long)(tileCoords[j].Item1 - tileCoords[i].Item1 + 1) * (tileCoords[j].Item2 - tileCoords[i].Item2 + 1));
                if (currentSize > maxSizeRectangle) {
                    maxSizeRectangle = currentSize;
                }
            }
        }
        return maxSizeRectangle;
    }

    public object PartTwo(string input) {
        var tileCoords = input.Split("\n")
            .Select(line => line.Split(','))
            .Select(pair => (int.Parse(pair[0]), int.Parse(pair[1])))
            .OrderBy(pair => pair.Item1)
            .ToArray();
        var compressor = new Compressor2D(tileCoords);
        var compressedCoords = tileCoords.Select(p => compressor.GetCompressed(p.Item1, p.Item2)).ToArray();

        return 0;
    }
}

class Compressor2D {
    public Dictionary<int, int> XMap { get; private set; }
    public Dictionary<int, int> YMap { get; private set; }

    public Compressor2D(IEnumerable<(int x, int y)> points) {
        var pointList = points.ToList();

        XMap = Compress(pointList.Select(p => p.x));
        YMap = Compress(pointList.Select(p => p.y));
    }

    private Dictionary<int, int> Compress(IEnumerable<int> values) {
        return values.Distinct()
            .OrderBy(v => v)
            .Select((v, i) => (v, i))
            .ToDictionary(x => x.v, x => x.i);
    }

    public (int x, int y) GetCompressed(int x, int y) => (XMap[x], YMap[y]);
}
