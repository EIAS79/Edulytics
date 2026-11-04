namespace Edulytics.Core.Mathematics.Practice;

/// <summary>
/// Exact bounded vertex-geometry helper for Grade 7 U01:L01 ("What are Scaled
/// Copies?"). It does not replace the open polygon drawing/justification,
/// which remains TEACHER_REVIEW_REQUIRED. NOT a selected Practice route.
/// </summary>
public static class UsCcssG7Unit1Lesson1ScaledCopyMath
{
    public readonly record struct Vertex(decimal X, decimal Y);
    public readonly record struct CopyResult(bool IsScaledCopy, string Code);

    private const decimal MaxCoordinate = 1_000_000m;
    private const int MaxVertices = 64;

    /// <summary>
    /// Source-style correspondence may start at another vertex and traverse
    /// either direction. Exact pairwise squared distances ensure ALL sides,
    /// diagonals and angles scale uniformly (including rotated/translated
    /// copies), rather than accepting a horizontal-only stretch.
    /// The factor is supplied by the source task; no guess from one edge.
    /// Coordinates are decimal, with NO floating-point tolerance or rounding.
    /// </summary>
    public static CopyResult Check(
        IReadOnlyList<Vertex>? original,
        IReadOnlyList<Vertex>? candidate,
        decimal factor)
    {
        if (factor <= 0m || factor > MaxCoordinate)
            return new(false, "INVALID_FACTOR");
        if (original is null || candidate is null ||
            original.Count < 3 || original.Count > MaxVertices ||
            candidate.Count != original.Count)
            return new(false, "INVALID_VERTEX_COUNT");
        if (!IsSimplePolygon(original) || !IsSimplePolygon(candidate))
            return new(false, "INVALID_OR_DEGENERATE_POLYGON");

        int n = original.Count;
        decimal squaredFactor = factor * factor;
        for (int reversed = 0; reversed <= 1; reversed++)
        {
            for (int offset = 0; offset < n; offset++)
            {
                bool matches = true;
                for (int i = 0; i < n && matches; i++)
                {
                    int mappedI = MapIndex(i, offset, n, reversed != 0);
                    for (int j = i + 1; j < n; j++)
                    {
                        int mappedJ = MapIndex(j, offset, n, reversed != 0);
                        if (SquaredDistance(candidate[mappedI], candidate[mappedJ]) !=
                            squaredFactor * SquaredDistance(original[i], original[j]))
                        {
                            matches = false;
                            break;
                        }
                    }
                }
                if (matches)
                    return new(true, "UNIFORM_SCALED_COPY");
            }
        }
        return new(false, "NON_UNIFORM_OR_WRONG_CORRESPONDENCE");
    }

    private static int MapIndex(int index, int offset, int count, bool reversed) =>
        reversed ? (offset - index + count) % count : (offset + index) % count;

    private static decimal SquaredDistance(Vertex a, Vertex b)
    {
        decimal dx = a.X - b.X;
        decimal dy = a.Y - b.Y;
        return dx * dx + dy * dy;
    }

    private static bool IsSimplePolygon(IReadOnlyList<Vertex> points)
    {
        int n = points.Count;
        for (int i = 0; i < n; i++)
        {
            if (decimal.Abs(points[i].X) > MaxCoordinate ||
                decimal.Abs(points[i].Y) > MaxCoordinate)
                return false;
            for (int j = i + 1; j < n; j++)
                if (points[i] == points[j])
                    return false;
        }

        decimal twiceArea = 0m;
        for (int i = 0; i < n; i++)
        {
            Vertex a = points[i], b = points[(i + 1) % n];
            twiceArea += a.X * b.Y - b.X * a.Y;
        }
        if (twiceArea == 0m)
            return false;

        // A self-intersecting path is not a valid source polygon, even if its
        // vertex distances happen to be scale-equivalent.
        for (int i = 0; i < n; i++)
        {
            Vertex a = points[i], b = points[(i + 1) % n];
            for (int j = i + 1; j < n; j++)
            {
                if (j == i + 1 || (i == 0 && j == n - 1))
                    continue; // incident edges share their intended vertex
                Vertex c = points[j], d = points[(j + 1) % n];
                if (SegmentsIntersect(a, b, c, d))
                    return false;
            }
        }
        return true;
    }

    private static decimal Cross(Vertex a, Vertex b, Vertex c) =>
        (b.X - a.X) * (c.Y - a.Y) -
        (b.Y - a.Y) * (c.X - a.X);

    private static bool OnSegment(Vertex a, Vertex b, Vertex p) =>
        p.X >= decimal.Min(a.X, b.X) && p.X <= decimal.Max(a.X, b.X) &&
        p.Y >= decimal.Min(a.Y, b.Y) && p.Y <= decimal.Max(a.Y, b.Y);

    private static bool SegmentsIntersect(Vertex a, Vertex b, Vertex c, Vertex d)
    {
        decimal abC = Cross(a, b, c), abD = Cross(a, b, d);
        decimal cdA = Cross(c, d, a), cdB = Cross(c, d, b);
        if (abC == 0m && OnSegment(a, b, c)) return true;
        if (abD == 0m && OnSegment(a, b, d)) return true;
        if (cdA == 0m && OnSegment(c, d, a)) return true;
        if (cdB == 0m && OnSegment(c, d, b)) return true;
        return (abC > 0m) != (abD > 0m) &&
               (cdA > 0m) != (cdB > 0m);
    }
}
