namespace Edulytics.Core.Mathematics.Practice;

/// <summary>
/// Original Edulytics area-grid representation for 2021 Grade 5 Unit 3 Lesson 1.
/// It validates only an equally partitioned DIGITAL grid. Freehand drawings,
/// explanations and identification of the whole still require teacher review.
/// This class does not register a shared Practice question family.
/// </summary>
public static class UsCcssG5U03L01AreaModel
{
    public enum CutDirection { Horizontal, Vertical }

    public readonly record struct Cell(int X, int Y, int Width, int Height, bool IsShaded);

    public sealed record Diagram(
        int OriginalWidth,
        int OriginalHeight,
        int SubdivisionsOfHalf,
        CutDirection HalfCut,
        CutDirection SubdivisionCut,
        int NumeratorOfOriginalWhole,
        int DenominatorOfOriginalWhole,
        IReadOnlyList<Cell> Cells)
    {
        public bool TeacherReviewRequired => true;
    }

    /// <summary>
    /// Build a full-original-whole grid. The left/top half is selected, and one
    /// equal cell within that half is shaded. The two source tasks use third or
    /// quarter of one half; no fraction of a NEW half-whole is returned.
    /// </summary>
    public static bool TryCreate(
        int originalWidth, int originalHeight, int subdivisionsOfHalf,
        CutDirection halfCut, CutDirection subdivisionCut,
        out Diagram? diagram)
    {
        diagram = null;
        if (originalWidth <= 0 || originalHeight <= 0 ||
            subdivisionsOfHalf is not (3 or 4) ||
            !Enum.IsDefined(halfCut) || !Enum.IsDefined(subdivisionCut))
            return false;

        var columns = (halfCut == CutDirection.Vertical ? 2 : 1) *
                      (subdivisionCut == CutDirection.Vertical ? subdivisionsOfHalf : 1);
        var rows = (halfCut == CutDirection.Horizontal ? 2 : 1) *
                   (subdivisionCut == CutDirection.Horizontal ? subdivisionsOfHalf : 1);
        if (originalWidth % columns != 0 || originalHeight % rows != 0)
            return false; // no unequal/sliver cells disguised as equal fractions

        var cellWidth = originalWidth / columns;
        var cellHeight = originalHeight / rows;
        var cells = new List<Cell>(2 * subdivisionsOfHalf);
        for (var row = 0; row < rows; row++)
            for (var column = 0; column < columns; column++)
                cells.Add(new Cell(
                    column * cellWidth, row * cellHeight,
                    cellWidth, cellHeight, row == 0 && column == 0));

        diagram = new Diagram(originalWidth, originalHeight, subdivisionsOfHalf,
            halfCut, subdivisionCut, 1, 2 * subdivisionsOfHalf, cells.AsReadOnly());
        return true;
    }

    /// <summary>
    /// Validate equal, nonoverlapping grid cells covering the ORIGINAL whole,
    /// and exactly one shaded cell in the indicated first half. This is not
    /// an automated score for freehand construction or mathematical explanation.
    /// </summary>
    public static bool IsValidDigitalGrid(Diagram? reference, IReadOnlyList<Cell>? submitted)
    {
        if (!IsCanonicalDiagram(reference) || submitted is null ||
            submitted.Count != reference!.Cells.Count)
            return false;

        var expected = new HashSet<(int X, int Y)>();
        foreach (var cell in reference.Cells)
            expected.Add((cell.X, cell.Y));

        var seen = new HashSet<(int X, int Y)>();
        var shaded = 0;
        var width = reference.Cells[0].Width;
        var height = reference.Cells[0].Height;
        foreach (var cell in submitted)
        {
            if (cell.Width != width || cell.Height != height ||
                !expected.Contains((cell.X, cell.Y)) ||
                !seen.Add((cell.X, cell.Y)))
                return false; // unequal partitions, missing area, overlap, out of bounds

            if (!cell.IsShaded)
                continue;

            shaded++;
            if (reference.HalfCut == CutDirection.Vertical
                ? cell.X >= reference.OriginalWidth / 2
                : cell.Y >= reference.OriginalHeight / 2)
                return false; // shaded a piece from outside the chosen half
        }

        return seen.Count == expected.Count && shaded == 1;
    }

    /// <summary>Compare fractions only when both use the same original whole.</summary>
    public static bool RepresentsSameAreaOfOriginalWhole(Diagram? first, Diagram? second) =>
        IsCanonicalDiagram(first) && IsCanonicalDiagram(second) &&
        first!.OriginalWidth == second!.OriginalWidth &&
        first.OriginalHeight == second.OriginalHeight &&
        (long)first.NumeratorOfOriginalWhole * second.DenominatorOfOriginalWhole ==
        (long)second.NumeratorOfOriginalWhole * first.DenominatorOfOriginalWhole;

    private static bool IsCanonicalDiagram(Diagram? diagram)
    {
        if (diagram is null ||
            !TryCreate(diagram.OriginalWidth, diagram.OriginalHeight,
                diagram.SubdivisionsOfHalf, diagram.HalfCut, diagram.SubdivisionCut,
                out var generated) ||
            generated is null ||
            diagram.NumeratorOfOriginalWhole != generated.NumeratorOfOriginalWhole ||
            diagram.DenominatorOfOriginalWhole != generated.DenominatorOfOriginalWhole ||
            diagram.Cells is null ||
            diagram.Cells.Count != generated.Cells.Count)
            return false;

        for (var i = 0; i < generated.Cells.Count; i++)
            if (diagram.Cells[i] != generated.Cells[i])
                return false;
        return true;
    }
}
