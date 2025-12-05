namespace AdventOfCode.Y2025.Day05;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Numerics;
using System.Dynamic;

[ProblemName("Cafeteria")]
class Solution : Solver {

    public object PartOne(string input) {
        string ingredientRangesInput = input.Split("\n\n")[0];
        HashSet<(long, long)> freshIngredientRanges = GetFreshIngredientRanges(ingredientRangesInput);
        string ingredientIdsInput = input.Split("\n\n")[1];
        HashSet<long> ingredientIds = GetIngredientIds(ingredientIdsInput);

        int freshCount = 0;
        foreach (var id in ingredientIds) {
            foreach (var range in freshIngredientRanges) {
                if (id >= range.Item1 && id <= range.Item2) {
                    freshCount++;
                    break;
                }
            }
        }

        return freshCount;
    }
    
    private HashSet<(long, long)> GetFreshIngredientRanges(string input) {
        HashSet<(long, long)> freshIngredientRanges = new();
        
        foreach (var line in input.Split("\n")) {
            if (line.Contains("-")) {
                var parts = line.Split("-");
                var start = long.Parse(parts[0]);
                var end = long.Parse(parts[1]);
                freshIngredientRanges.Add((start, end));
            }    
        }

        return freshIngredientRanges;
    }

    private HashSet<long> GetIngredientIds(string input) {
        HashSet<long> ingredientIds = new();

        foreach (var line in input.Split("\n")) {
            if (long.TryParse(line, out long id)) {
                ingredientIds.Add(id);
            }
        }

        return ingredientIds;
    }

    public object PartTwo(string input) {
        string ingredientRangesInput = input.Split("\n\n")[0];
        List<(long, long)> freshIngredientRanges = GetFreshIngredientRangesList(ingredientRangesInput);
        List<(long, long)> dedupedIngredientRanges = DeDupeRanges(freshIngredientRanges);
        return CountUniqueIngredientIds(dedupedIngredientRanges);
    }
    
    public object AltPartTwo(string input) {
        return input.Split("\n\n")[0]
            .Split("\n")
            .Where(line => line.Contains("-"))
            .Select(line => line.Split("-"))
            .Select(parts => (Start: long.Parse(parts[0]), End: long.Parse(parts[1])))
            .OrderBy(range => range.Start)
            .Aggregate(
                new List<(long Start, long End)>(),
                (merged, range) => {
                    if (merged.Count == 0 || range.Start > merged[^1].End + 1) merged.Add(range);
                    else merged[^1] = (merged[^1].Start, Math.Max(merged[^1].End, range.End));
                    return merged;
                })
            .Sum(range => range.End - range.Start + 1);
    }

    private List<(long, long)> GetFreshIngredientRangesList(string input) {
        List<(long, long)> freshIngredientRanges = new();
        
        foreach (var line in input.Split("\n")) {
            if (line.Contains("-")) {
                var parts = line.Split("-");
                var start = long.Parse(parts[0]);
                var end = long.Parse(parts[1]);
                freshIngredientRanges.Add((start, end));
            }    
        }

        return freshIngredientRanges;
    }

    private List<(long, long)> DeDupeRanges(List<(long, long)> inputRanges) {
        for (var target = inputRanges.Count - 1; target > 0; target--) {
            for (var deDuped = 0; deDuped < target; deDuped++) {
                if (inputRanges[target].Item1 <= inputRanges[deDuped].Item1 && inputRanges[target].Item2 >= inputRanges[deDuped].Item1 && inputRanges[target].Item2 <= inputRanges[deDuped].Item2) {
                    // targetRange lower bound should replace deDupedRange's lower bound
                    inputRanges[deDuped] = (inputRanges[target].Item1, inputRanges[deDuped].Item2);
                    inputRanges.RemoveAt(target);
                    break;
                } else if (inputRanges[target].Item1 >= inputRanges[deDuped].Item1 && inputRanges[target].Item1 <= inputRanges[deDuped].Item2 && inputRanges[target].Item2 <= inputRanges[deDuped].Item2) {
                    // targetRange is entirely within existing range; skip update
                    inputRanges.RemoveAt(target);
                    break;
                } else if (inputRanges[target].Item1 >= inputRanges[deDuped].Item1 && inputRanges[target].Item1 <= inputRanges[deDuped].Item2 && inputRanges[target].Item2 >= inputRanges[deDuped].Item2) {
                    // targetRange upper bound should replace deDupedRange's upper bound
                    inputRanges[deDuped] = (inputRanges[deDuped].Item1, inputRanges[target].Item2);
                    inputRanges.RemoveAt(target);
                    break;
                } else if (inputRanges[target].Item1 <= inputRanges[deDuped].Item1 && inputRanges[target].Item2 >= inputRanges[deDuped].Item2) {
                    // targetRange should replace deDupedRange's lower AND upper bounds
                    inputRanges[deDuped] = (inputRanges[target].Item1, inputRanges[target].Item2);
                    inputRanges.RemoveAt(target);
                    break;
                }
            }
        }
        return inputRanges;
    }

    private long CountUniqueIngredientIds(List<(long, long)> ranges) {
        long count = 0;

        foreach (var range in ranges) {
            count += range.Item2 - range.Item1 + 1; // inclusive; add 1 to compensate
        }

        return count;
    }
}
