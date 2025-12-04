namespace AdventOfCode.Y2025.Day04;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Numerics;

[ProblemName("Printing Department")]
class Solution : Solver {

    public object PartOne(string input) {
        Grid grid = GetArrayInput(input);
        int accessiblePaperCount = 0;
        foreach(GridNode node in grid.nodes) {
            if (node.isPaper) {
                if (CountAdjacentPaper(grid, node) < 4) {
                    accessiblePaperCount++;
                }
            }
        }
        return accessiblePaperCount;
    }

    private int CountAdjacentPaper(Grid grid, GridNode node) {
        int adjacentPaperCount = 0;
        bool isColumnZero = node.column == 0;
        bool isColumnLast = node.column == grid.columnCount - 1;
        bool isRowZero = node.row == 0;
        bool isRowLast = node.row == grid.rowCount - 1;

        // Calculate paper above
        if (!isRowZero) {
            if (!isColumnZero && grid.nodes[node.row - 1, node.column - 1].isPaper) adjacentPaperCount++;
            if (grid.nodes[node.row - 1, node.column].isPaper) adjacentPaperCount++;
            if (!isColumnLast && grid.nodes[node.row - 1, node.column + 1].isPaper) adjacentPaperCount++;
        }

        // Calculate paper below
        if (!isRowLast) {
            if (!isColumnZero && grid.nodes[node.row + 1, node.column - 1].isPaper) adjacentPaperCount++;
            if (grid.nodes[node.row + 1, node.column].isPaper) adjacentPaperCount++;
            if (!isColumnLast && grid.nodes[node.row + 1, node.column + 1].isPaper) adjacentPaperCount++;
        }

        // Calculate paper directly left & right
        if (!isColumnZero && grid.nodes[node.row, node.column - 1].isPaper) adjacentPaperCount++;
        if (!isColumnLast && grid.nodes[node.row, node.column + 1].isPaper) adjacentPaperCount++;

        return adjacentPaperCount;
    }

    public object PartTwo(string input) {
        Grid grid = GetArrayInput(input);
        int iterationCount = 0;
        int totalAccessiblePaperCount = 0;
        int currentAccessiblePaperCount;
        do {
            iterationCount++;
            currentAccessiblePaperCount = 0;
            foreach(GridNode node in grid.nodes) {
                if (node.isPaper) {
                    if (CountAdjacentPaper(grid, node) < 4) {
                        totalAccessiblePaperCount++;
                        currentAccessiblePaperCount++;
                        node.RemovePaper();
                    }
                }
            }
        } while (currentAccessiblePaperCount != 0 && iterationCount <= 1000);
        return totalAccessiblePaperCount;
    }

    private Grid GetArrayInput(string input) {
        string[] lines = input.Split("\n");
        return new Grid(lines);
    }
}

public class Grid {
    public GridNode[,] nodes;
    public int rowCount;
    public int columnCount;

    public Grid(string[] input) {
        rowCount = input.Length;
        columnCount = input[0].Length;
        nodes = new GridNode[rowCount, columnCount];
        for (int row = 0; row < rowCount; row++) {
            for (int column = 0; column < columnCount; column++) {
                nodes[row, column] = new GridNode(input[row][column], row, column);
            }
        }
    }
}

public class GridNode {
    private const char PaperSymbol = '@';
    private const char EmptySymbol = '.';
    public char symbol;
    public int row;
    public int column;
    public bool isPaper => symbol == PaperSymbol;
    public void RemovePaper() {
        if (isPaper) {
            symbol = EmptySymbol;
        }
    }

    public GridNode(char symbol, int row, int column) {
        this.symbol = symbol;
        this.row = row;
        this.column = column;
    }
}
