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
        // Parse coordinates - KEEP ORIGINAL ORDER (defines polygon vertices)
        var tileCoords = input.Split("\n")
            .Select(line => line.Split(','))
            .Select(pair => (x: int.Parse(pair[0]), y: int.Parse(pair[1])))
            .ToArray();

        var (xToExpanded, yToExpanded, gridWidth, gridHeight) = CompressCoordinates(tileCoords);

        // Mark boundary cells (polygon edges)
        var boundary = new bool[gridWidth, gridHeight];

        for (int i = 0; i < tileCoords.Length; i++) {
            var p1 = tileCoords[i];
            var p2 = tileCoords[(i + 1) % tileCoords.Length]; // wraps to connect last->first

            int ex1 = xToExpanded[p1.x], ey1 = yToExpanded[p1.y];
            int ex2 = xToExpanded[p2.x], ey2 = yToExpanded[p2.y];

            // Draw axis-aligned line from p1 to p2
            int minEx = Math.Min(ex1, ex2), maxEx = Math.Max(ex1, ex2);
            int minEy = Math.Min(ey1, ey2), maxEy = Math.Max(ey1, ey2);

            for (int ex = minEx; ex <= maxEx; ex++) {
                for (int ey = minEy; ey <= maxEy; ey++) {
                    boundary[ex, ey] = true;
                }
            }
        }

        var exterior = FloodFillExterior(boundary, gridWidth, gridHeight);

        // Find largest valid rectangle (corners at red tiles, no exterior cells inside)
        long maxArea = 0;

        for (int i = 0; i < tileCoords.Length; i++) {
            for (int j = i + 1; j < tileCoords.Length; j++) {
                var p1 = tileCoords[i];
                var p2 = tileCoords[j];

                int minEx = Math.Min(xToExpanded[p1.x], xToExpanded[p2.x]);
                int maxEx = Math.Max(xToExpanded[p1.x], xToExpanded[p2.x]);
                int minEy = Math.Min(yToExpanded[p1.y], yToExpanded[p2.y]);
                int maxEy = Math.Max(yToExpanded[p1.y], yToExpanded[p2.y]);

                if (IsRectangleValid(exterior, minEx, maxEx, minEy, maxEy)) {
                    long width = Math.Abs((long)p2.x - p1.x) + 1;
                    long height = Math.Abs((long)p2.y - p1.y) + 1;
                    maxArea = Math.Max(maxArea, width * height);
                }
            }
        }

        return maxArea;
    }

    private (Dictionary<int, int> xMap, Dictionary<int, int> yMap, int width, int height) CompressCoordinates(
        (int x, int y)[] points) {
        // Get unique sorted coordinates
        var xCoords = points.Select(p => p.x).Distinct().OrderBy(x => x).ToArray();
        var yCoords = points.Select(p => p.y).Distinct().OrderBy(y => y).ToArray();

        // Map original coords to expanded grid positions
        // Original coord i maps to position 2*i+1 (odd positions)
        // Even positions represent "between" regions
        var xToExpanded = xCoords.Select((x, i) => (x, idx: 2 * i + 1)).ToDictionary(p => p.x, p => p.idx);
        var yToExpanded = yCoords.Select((y, i) => (y, idx: 2 * i + 1)).ToDictionary(p => p.y, p => p.idx);

        int gridWidth = 2 * xCoords.Length + 1;
        int gridHeight = 2 * yCoords.Length + 1;

        return (xToExpanded, yToExpanded, gridWidth, gridHeight);
    }

    private bool[,] FloodFillExterior(bool[,] boundary, int width, int height) {
        var exterior = new bool[width, height];
        var queue = new Queue<(int x, int y)>();
        queue.Enqueue((0, 0));
        exterior[0, 0] = true;

        int[] dx = {1, -1, 0, 0};
        int[] dy = {0, 0, 1, -1};

        while (queue.Count > 0) {
            var (x, y) = queue.Dequeue();
            for (int d = 0; d < 4; d++) {
                int nx = x + dx[d], ny = y + dy[d];
                if (nx >= 0 && nx < width && ny >= 0 && ny < height &&
                    !exterior[nx, ny] && !boundary[nx, ny]) {
                    exterior[nx, ny] = true;
                    queue.Enqueue((nx, ny));
                }
            }
        }

        return exterior;
    }

    private bool IsRectangleValid(bool[,] exterior, int minX, int maxX, int minY, int maxY) {
        for (int x = minX; x <= maxX; x++) {
            for (int y = minY; y <= maxY; y++) {
                if (exterior[x, y]) {
                    return false;
                }
            }
        }
        return true;
    }
}
