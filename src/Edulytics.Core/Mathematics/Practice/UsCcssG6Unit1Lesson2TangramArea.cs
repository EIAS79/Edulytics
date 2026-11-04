namespace Edulytics.Core.Mathematics.Practice;

/// <summary>
/// Grade 6 IM U01:L02 first-party tangram arithmetic, in exact HALF-SQUARE units.
/// This course-scoped helper is NOT the selected Practice route, and cannot
/// certify polygon placement, edge matches, or a student's freehand drawing.
/// </summary>
public static class UsCcssG6Unit1Lesson2TangramArea
{
    // Source-compatible first-party inventory:
    // 1 square (area 1), 4 small triangles (each 1/2),
    // 1 medium triangle (area 1), 2 large triangles (each area 2).
    public readonly record struct Pieces(
        int Squares, int SmallTriangles, int MediumTriangles, int LargeTriangles);

    public static readonly Pieces FullKit = new(1, 4, 1, 2);
    public const int FullKitAreaInHalfSquareUnits = 16; // 8 square units.

    public readonly record struct AreaCheck(
        bool ValidInventory,
        bool ExactTargetArea,
        int AreaInHalfSquareUnits,
        string Status,
        bool TeacherReviewRequired)
    {
        // A numeric match never establishes a valid constructed silhouette.
        public bool GeometryVerified => false;
    }

    /// <summary>
    /// Compare exact piece areas with a target measured in half-square units.
    /// A match is only a numeric prerequisite; teacher verifies assembled shape,
    /// no gap/overlap, non-square criterion, distinctness and explanations.
    /// </summary>
    public static AreaCheck Check(Pieces? selected, int targetHalfSquareUnits)
    {
        if (selected is null || targetHalfSquareUnits < 1 ||
            targetHalfSquareUnits > FullKitAreaInHalfSquareUnits)
            return Invalid("MALFORMED_SELECTION_OR_TARGET");

        var p = selected.Value;
        if (p.Squares is < 0 or > 1 || p.SmallTriangles is < 0 or > 4 ||
            p.MediumTriangles is < 0 or > 1 || p.LargeTriangles is < 0 or > 2)
            return Invalid("PIECES_OUTSIDE_SOURCE_KIT");

        int pieces = p.Squares + p.SmallTriangles + p.MediumTriangles + p.LargeTriangles;
        if (pieces == 0)
            return Invalid("NO_PIECES");

        // Whole-square area weights: 2, 1, 2, 4 * half-square units.
        int halfUnits = 2 * p.Squares + p.SmallTriangles +
                        2 * p.MediumTriangles + 4 * p.LargeTriangles;

        if (halfUnits != targetHalfSquareUnits)
            return new(true, false, halfUnits, "AREA_MISMATCH", true);

        return new(true, true, halfUnits, "AREA_MATCH_REQUIRES_TEACHER_REVIEW", true);
    }

    public static AreaCheck CheckFullKit(int targetHalfSquareUnits) =>
        Check(FullKit, targetHalfSquareUnits);

    /// <summary>
    /// The shapes and figures still require human review even if two piece sets
    /// have the same numerical area. This method does NOT certify distinctness.
    /// </summary>
    public static bool SameExactArea(Pieces? first, Pieces? second, int targetHalfSquareUnits)
    {
        var a = Check(first, targetHalfSquareUnits);
        var b = Check(second, targetHalfSquareUnits);
        return a.ValidInventory && b.ValidInventory &&
               a.ExactTargetArea && b.ExactTargetArea;
    }

    private static AreaCheck Invalid(string reason) =>
        new(false, false, 0, reason, true);
}
