# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Overview

Advent of Code solutions repository:
- **2020**: Python solutions
- **2025**: C# solutions using [encse/adventofcode-template](https://github.com/encse/adventofcode-template)

Most of the repo is template files supporting the 2025 C# implementation. The template provides automated input downloading, solution scaffolding, answer submission, and regression testing.

## Daily Workflow

```bash
# 1. Setup new day (creates folder, downloads input, generates Solution.cs template)
dotnet run update today          # During Dec 1-25
dotnet run update 2025/1         # Specific day

# 2. Edit {year}/Day{NN}/Solution.cs - implement PartOne, return your answer

# 3. Test locally
dotnet run today

# 4. Submit answer (runs solution, validates against .refout, then uploads)
dotnet run upload today

# 5. On success, auto-updates to fetch Part Two - implement PartTwo, repeat
```

Requires `SESSION` environment variable from adventofcode.com cookie.

## Other Commands

```bash
dotnet run 2025/1                # Run specific day
dotnet run 2025                  # Run entire year
dotnet run all                   # Run everything
dotnet run calendars             # Show ASCII advent calendars
dotnet run loc                   # Show lines of code chart
```

## Solution Structure

Each day creates `{year}/Day{NN}/` containing:
- `Solution.cs` - Your solution (namespace: `AdventOfCode.Y{year}.Day{NN}`)
- `input.in` - Puzzle input (encrypted via git-crypt)
- `input.refout` - Expected answers for regression testing (auto-updated on correct submissions)
- `README.md` - Problem description excerpt

**Solver Interface** (`Lib/Runner.cs:18`):
```csharp
interface Solver {
    object PartOne(string input);
    object PartTwo(string input);  // Optional, defaults to null
}
```

Use `[ProblemName("Title")]` attribute on solution class. Return answer as any object (ToString() is called).

**Generated template includes**: System, System.Linq, System.Collections.Generic, System.Collections.Immutable, System.Text.RegularExpressions, System.Text, System.Numerics

## Key Details

- **Input normalization**: `\r` stripped, trailing newline removed (handled by framework)
- **OCR utility** (`Lib/Ocr.cs`): For puzzles rendering letters in ASCII art, call `.Ocr()` on the string to decode
- **Regression testing**: Runner compares output against `.refout`, shows ✓/X with timing
- **Upload flow**: Runs solution first, only uploads if existing tests pass, auto-updates on success

## Tech Stack

.NET 10, C# 14, AngleSharp (HTML parsing), git-crypt (input encryption)
