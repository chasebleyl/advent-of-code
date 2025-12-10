namespace AdventOfCode.Y2025.Day08;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Numerics;

public class Pos {
    public double X { get; set; }
    public double Y { get; set; }
    public double Z { get; set; }

    public double this[int axis] => axis switch {
        0 => X,
        1 => Y, 
        2 => Z,
        _ => throw new IndexOutOfRangeException()
    };

    public double DistanceSquaredTo(Pos other) => Math.Sqrt(
            Math.Pow(X - other.X, 2) + 
            Math.Pow(Y - other.Y, 2) + 
            Math.Pow(Z - other.Z, 2)
        );
}

public class UnionFind<T> where T : notnull {
    private readonly Dictionary<T, T> parent = new();
    private readonly Dictionary<T, int> rank = new();

    public int Count { get; private set; }
    public void Add(T item) {
        if (parent.ContainsKey(item)) return;
        parent[item] = item;
        rank[item] = 0;
        Count++;
    }

    public T Find(T x) {
        if (!parent.ContainsKey(x)) Add(x);

        if (!parent[x].Equals(x)) parent[x] = Find(parent[x]);

        return parent[x];
    }

    public bool Union(T x, T y) {
        T rootX = Find(x), rootY = Find(y);

        if (rootX.Equals(rootY)) return false;

        if (rank[rootX] < rank[rootY]) parent[rootX] = rootY;
        else if (rank[rootX] > rank[rootY]) parent[rootY] = rootX;
        else {
            parent[rootY] = rootX;
            rank[rootX]++;
        }

        Count--;
        return true;
    }

    public bool Connected(T x, T y) => Find(x).Equals(Find(y));

    public List<int> GetGroupSizesSorted() =>
        parent.Keys
            .GroupBy(Find)
            .Select(g => g.Count())
            .OrderByDescending(x => x)
            .ToList();

    public int GetGroupsCount() =>
        parent.Keys.GroupBy(Find).Count();
}

[ProblemName("Playground")]
class Solution : Solver {

    public object PartOne(string input) {
        var allPositions= input.Split('\n')
            .Select(line => line.Split(','))
            .Select(p => new Pos { X = int.Parse(p[0]), Y = int.Parse(p[1]), Z = int.Parse(p[2]) })
            .ToList();

        var nClosestPairs = FindNClosestPairs(allPositions, 1000);

        var uf = new UnionFind<Pos>();
        foreach (var pair in nClosestPairs) {
            uf.Union(pair.Item1, pair.Item2);
        }
        return uf.GetGroupSizesSorted().Take(3).Aggregate(1, (acc, x) => acc * x);
    }

    public List<(Pos, Pos)> FindNClosestPairs(List<Pos> points, int n)
    {
        var pairs = new SortedList<double, (Pos, Pos)>();
        
        for (int i = 0; i < points.Count; i++)
        {
            for (int j = i + 1; j < points.Count; j++)
            {
                double dist = points[i].DistanceSquaredTo(points[j]);
                
                // Handle duplicate distances by adding tiny offset
                while (pairs.ContainsKey(dist))
                    dist += double.Epsilon;
                    
                pairs.Add(dist, (points[i], points[j]));
                
                // Keep only top N to save memory
                if (pairs.Count > n)
                    pairs.RemoveAt(pairs.Count - 1);
            }
        }
        
        return pairs.Values.Take(n).ToList();
    }

    public object PartTwo(string input) {
        var allPositions= input.Split('\n')
            .Select(line => line.Split(','))
            .Select(p => new Pos { X = int.Parse(p[0]), Y = int.Parse(p[1]), Z = int.Parse(p[2]) })
            .ToList();
        
        var uf = new UnionFind<Pos>();
        foreach (var position in allPositions) {
            uf.Union(position, position);
        }

        var sortedClosestPairs = FindAllClosestPairs(allPositions);

        var currentPair = 0;
        while (uf.GetGroupsCount() > 1 && currentPair < sortedClosestPairs.Count) {
            uf.Union(sortedClosestPairs[currentPair].Item1, sortedClosestPairs[currentPair].Item2);
            currentPair += 1;
        }

        var (x, y) = sortedClosestPairs[currentPair - 1];

        return x.X * y.X;
    }

    
    public List<(Pos, Pos)> FindAllClosestPairs(List<Pos> points)
    {
        var pairs = new SortedList<double, (Pos, Pos)>();
        
        for (int i = 0; i < points.Count; i++)
        {
            for (int j = i + 1; j < points.Count; j++)
            {
                double dist = points[i].DistanceSquaredTo(points[j]);
                
                // Handle duplicate distances by adding tiny offset
                while (pairs.ContainsKey(dist))
                    dist += double.Epsilon;
                    
                pairs.Add(dist, (points[i], points[j]));
            }
        }
        
        return pairs.Values.ToList();
    }
}
