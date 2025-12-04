namespace AdventOfCode.Y2025.Day03;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Numerics;

[ProblemName("Lobby")]
class Solution : Solver {
    private const int PartTwoBatteryCount = 12;

    public object PartOne(string input) {
        PowerBank[] powerBanks = GetArrayInput(input);
        int accumulatedJoltage = 0;
        foreach(PowerBank powerBank in powerBanks) {
            accumulatedJoltage += CalculateMaxJoltageFromPowerBank(powerBank);
        }
        return accumulatedJoltage;
    }

    private int CalculateMaxJoltageFromPowerBank(PowerBank powerBank) {
        char[] batteries = powerBank.batteriesCharArray;
        char activatedBatteryOne = batteries[0];
        char activatedBatteryTwo = batteries[1];
        for (int i = 2; i < batteries.Length; i++) {
            int currentJoltage = DigitsToInt(activatedBatteryOne, activatedBatteryTwo);
            int calc1 = DigitsToInt(activatedBatteryOne, batteries[i]);
            int calc2 = DigitsToInt(activatedBatteryTwo, batteries[i]);
            if (calc1 > currentJoltage && calc1 >= calc2) {
                activatedBatteryTwo = batteries[i];
            }
            else if (calc2 > currentJoltage && calc2 > calc1) {
                activatedBatteryOne = activatedBatteryTwo;
                activatedBatteryTwo = batteries[i];
            }
        }
        return DigitsToInt(activatedBatteryOne, activatedBatteryTwo);
    }

    private int DigitsToInt(char c1, char c2) {
        return int.Parse($"{c1}{c2}");
    }

    public object PartTwo(string input) {
        PowerBank[] powerBanks = GetArrayInput(input);
        long accumulatedJoltage = 0;
        foreach(PowerBank powerBank in powerBanks) {
            accumulatedJoltage += CalculateMaxBatteryVoltage(powerBank);
        }
        return accumulatedJoltage;
    }

    private long CalculateMaxBatteryVoltage(PowerBank powerBank) {
        int[] batteries = powerBank.batteriesIntArray;
        int[] allocatedBatteries = new int[PartTwoBatteryCount];
        int searchStart = 0;

        for (int i = 0; i < PartTwoBatteryCount; i++) {
            int remainingToAllocate = PartTwoBatteryCount - i;
            int searchEnd = batteries.Length - remainingToAllocate;
            int maxIndex = FindMaxIndexInRange(batteries, searchStart, searchEnd);
            allocatedBatteries[i] = batteries[maxIndex];
            searchStart = maxIndex + 1;
        }

        return DigitsToLong(allocatedBatteries);
    }

    private int FindMaxIndexInRange(int[] nums, int startIndex, int endIndex) {
        int maxVal = nums[startIndex];
        int maxIndex = startIndex;
        for (int i = startIndex + 1; i <= endIndex; i++) {
            if (nums[i] > maxVal) {
                maxVal = nums[i];
                maxIndex = i;
            }
        }
        return maxIndex;
    }

    private long DigitsToLong(int[] digits) {
        StringBuilder sb = new StringBuilder();
        foreach (int digit in digits) {
            sb.Append(digit);
        }
        return long.Parse(sb.ToString());
    }
    
    private PowerBank[] GetArrayInput(string input) => input
        .Split("\n", StringSplitOptions.RemoveEmptyEntries)
        .Select(line => new PowerBank(line.Trim()))
        .ToArray();
}

public class PowerBank {
    public int[] batteriesIntArray;
    public char[] batteriesCharArray;

    public PowerBank(string input) {
        batteriesIntArray = input.Select(c => c - '0').ToArray();
        batteriesCharArray = input.ToCharArray();
    }
}
