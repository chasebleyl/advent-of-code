namespace AdventOfCode.Y2025.Day01;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Numerics;

[ProblemName("Secret Entrance")]
class Solution : Solver {

    public object PartOne(string input) {
        Rotation[] rotations = GetArrayInput(input);
        int currentPosition = 50;
        int occurrencesOfPositionZero = 0;

        foreach (var rotation in rotations)
        {
            currentPosition = GetRotatedDialPosition(rotation, currentPosition);
            if (currentPosition == 0) occurrencesOfPositionZero++;
        }

        return occurrencesOfPositionZero;
    }

    public object PartTwo(string input) {
        Rotation[] rotations = GetArrayInput(input);
        int currentPosition = 50;
        int occurrencesTraversingPositionZero = 0;

        foreach (var rotation in rotations)
        {
            occurrencesTraversingPositionZero = occurrencesTraversingPositionZero + GetPositionZeroHits(rotation, currentPosition);
            currentPosition = GetRotatedDialPosition(rotation, currentPosition);
        }

        return occurrencesTraversingPositionZero;
    }

    private int GetRotatedDialPosition(Rotation rotation, int currentPosition)
    {
        int delta = rotation.Direction == Direction.Left ? -rotation.Clicks : rotation.Clicks;
        int newPosition = ((currentPosition + delta) % 100 + 100) % 100;
        return newPosition;
    }

    private int GetPositionZeroHits(Rotation rotation, int currentPosition)
    {
        if (rotation.Direction == Direction.Right) return (currentPosition + rotation.Clicks) / 100;
        else if (currentPosition == 0) return rotation.Clicks / 100; // Only count full loops back to 0
        else return (100 - currentPosition + rotation.Clicks) / 100; 
    }

    private Rotation[] GetArrayInput(string input) => input
        .Split("\n", StringSplitOptions.RemoveEmptyEntries)
        .Select(line => new Rotation(line.Trim()))
        .ToArray();
}

public enum Direction
{
    Left,
    Right
}

public class Rotation
{
    public int Clicks { get; set; }
    public Direction Direction { get; set; }

    public Rotation(string input)
    {
        Direction = input[0] switch
        {
            'L' => Direction.Left,
            'R' => Direction.Right,
            _ => throw new ArgumentException($"Invalid direction character: {input[0]}")
        };

        Clicks = int.Parse(input[1..]);
    }
}
