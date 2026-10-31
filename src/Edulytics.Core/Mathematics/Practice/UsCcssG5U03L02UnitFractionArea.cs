namespace Edulytics.Core.Mathematics.Practice;

/// <summary>
/// Source-bounded digital original-whole grid for IM 2021 Grade 5 U03:L02.
/// Does NOT score freehand drawings, Priya's explanation, Which One Doesn't
/// Belong, or the publisher's separate shaded-picture task.
/// </summary>
public static class UsCcssG5U03L02UnitFractionArea
{
    public enum Direction { Horizontal, Vertical }

    public readonly record struct Cell(int X, int Y, int Width, int Height, bool Shaded);

    public sealed record Grid(
        int WholeWidth, int WholeHeight, int FirstDenominator,
        int SecondDenominator, Direction FirstCut, Direction SecondCut,
        IReadOnlyList<Cell> Cells)
    {
        public int NumeratorOfOriginalWhole => 1;
        public int DenominatorOfOriginalWhole => checked(FirstDenominator * SecondDenominator);
        public bool TeacherReviewRequired => true;
    }

    /// <summary>
    /// first=3,second=4 represents 1/4 of 1/3; reversed 4,3
    /// represents 1/3 of 1/4. Priya analogues are 2,5 and 5,2.
    /// All shading is measured relative to the same ORIGINAL WHOLE.
    /// </summary>
    public static bool TryCreate(
        int width, int height, int first, int second,
        Direction firstCut, Direction secondCut, out Grid? grid)
    {
        grid = null;
        if (width <= 0 || height <= 0 || !ApprovedPair(first, second) ||
            !Enum.IsDefined(firstCut) || !Enum.IsDefined(secondCut))
            return false;

        int columns = (firstCut == Direction.Vertical ? first : 1) *
                      (secondCut == Direction.Vertical ? second : 1);
        int rows = (firstCut == Direction.Horizontal ? first : 1) *
                   (secondCut == Direction.Horizontal ? second : 1);
        if (width % columns != 0 || height % rows != 0)
            return false;

        int cellWidth = width / columns;
        int cellHeight = height / rows;
        var cells = new List<Cell>(checked(columns * rows));
        for (int y = 0; y < rows; y++)
            for (int x = 0; x < columns; x++)
                cells.Add(new Cell(x * cellWidth, y * cellHeight,
                    cellWidth, cellHeight, x == 0 && y == 0));

        grid = new Grid(width, height, first, second,
            firstCut, secondCut, cells.AsReadOnly());
        return true;
    }

    public static bool IsValidDigitalGrid(Grid? reference, IReadOnlyList<Cell>? submitted)
    {
        if (reference is null ||
            !TryCreate(reference.WholeWidth, reference.WholeHeight,
                reference.FirstDenominator, reference.SecondDenominator,
                reference.FirstCut, reference.SecondCut, out var canonical) ||
            canonical is null ||
            !canonical.Cells.SequenceEqual(reference.Cells) ||
            submitted is null || submitted.Count != canonical.Cells.Count)
            return false;

        var available = canonical.Cells.ToDictionary(
            c => (c.X, c.Y), c => (c.Width, c.Height));
        var seen = new HashSet<(int X, int Y)>();
        int shaded = 0;
        foreach (var cell in submitted)
        {
            if (!available.TryGetValue((cell.X, cell.Y), out var size) ||
                !seen.Add((cell.X, cell.Y)) ||
                (cell.Width, cell.Height) != size)
                return false; // no overlap, gap, unequal or extra/out-of-bounds cells

            if (!cell.Shaded) continue;
            shaded++;
            if (reference.FirstCut == Direction.Vertical
                    ? cell.X >= reference.WholeWidth / reference.FirstDenominator
                    : cell.Y >= reference.WholeHeight / reference.FirstDenominator)
                return false; // a second/foreign partition is not selected
        }

        return shaded == 1 && seen.Count == available.Count;
    }

    public static bool RepresentsSameAreaOfSameOriginalWhole(Grid? a, Grid? b) =>
        a is not null && b is not null &&
        IsValidDigitalGrid(a, a.Cells) &&
        IsValidDigitalGrid(b, b.Cells) &&
        a.WholeWidth == b.WholeWidth && a.WholeHeight == b.WholeHeight &&
        (long)a.NumeratorOfOriginalWhole * b.DenominatorOfOriginalWhole ==
        (long)b.NumeratorOfOriginalWhole * a.DenominatorOfOriginalWhole;

    private static bool ApprovedPair(int first, int second) =>
        (first == 3 && second == 4) || (first == 4 && second == 3) ||
        (first == 2 && second == 5) || (first == 5 && second == 2);
}
