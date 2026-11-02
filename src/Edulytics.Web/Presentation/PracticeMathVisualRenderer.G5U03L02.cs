using System.Text;
using System.Text.Json;
using Area = Edulytics.Core.Mathematics.Practice.UsCcssG5U03L02UnitFractionArea;

namespace Edulytics.Web.Presentation;

public static partial class PracticeMathVisualRenderer
{
    private const string G5U03L02Product =
        "usccss.g5.u03.l02.unit_fraction_of_unit_fraction";
    private const string G5U03L02Compare =
        "usccss.g5.u03.l02.same_original_whole_order_comparison";

    /// <summary>
    /// Edulytiks-owned, source-skill-bound drawing, not an IM publisher image.
    /// Uses the exact same original-whole canonical grid as the registered
    /// Grade 5 question generator. Never scores a pupil's freehand drawing,
    /// teacher explanation or the original source's gated figure.
    /// </summary>
    private static string? GradeFiveUnitFractionAreaVisual(
        string family, JsonElement parameters)
    {
        if (family is not (G5U03L02Product or G5U03L02Compare) ||
            parameters.ValueKind != JsonValueKind.Object ||
            parameters.EnumerateObject().Count() != 5)
            return null;

        var scenario = TryGetInt(parameters, "case");
        var layout = TryGetInt(parameters, "layout");
        var slot = TryGetInt(parameters, "slot");
        var comparison = TryGetInt(parameters, "comparison");
        var differentWhole = TryGetInt(parameters, "differentWhole");
        if (scenario is null or < 0 or > 3 ||
            layout is null or < 0 or > 3 ||
            slot is null or < 0 or > 2 ||
            comparison is null or < 0 or > 3 ||
            differentWhole is null or < 0 or > 1)
            return null;

        static (int first, int second) Parts(int index) => index switch
        {
            0 => (3, 4),
            1 => (4, 3),
            2 => (2, 5),
            _ => (5, 2)
        };

        var (first, second) = Parts(scenario.Value);
        if (!Area.TryCreate(60, 60, first, second,
                (layout.Value & 1) == 0
                    ? Area.Direction.Horizontal : Area.Direction.Vertical,
                (layout.Value & 2) == 0
                    ? Area.Direction.Vertical : Area.Direction.Horizontal,
                out var a) || a is null ||
            !Area.IsValidDigitalGrid(a, a.Cells))
            return null;

        var sb = SvgStart(
            "Edulytiks original equal-area model for a selected unit-fraction problem. " +
            "All parts are measured relative to an original whole. " +
            "Written geometric reasoning remains teacher-reviewed.");

        if (family == G5U03L02Product)
        {
            Text(sb, 78, 31, "One original unit square", "label");
            DrawGradeFiveOriginalWhole(sb, a, 136, 62, 148, 148);
        }
        else
        {
            var (otherFirst, otherSecond) = Parts(comparison.Value);
            var otherWidth = differentWhole.Value == 0 ? 60 : 120;
            if (!Area.TryCreate(otherWidth, 60, otherFirst, otherSecond,
                    (layout.Value & 1) == 0
                        ? Area.Direction.Vertical : Area.Direction.Horizontal,
                    (layout.Value & 2) == 0
                        ? Area.Direction.Horizontal : Area.Direction.Vertical,
                    out var b) || b is null ||
                !Area.IsValidDigitalGrid(b, b.Cells))
                return null;

            Text(sb, 46, 35, "Diagram A", "label");
            Text(sb, 242, 35, "Diagram B", "label");
            DrawGradeFiveOriginalWhole(sb, a, 36, 67, 130, 130);
            DrawGradeFiveOriginalWhole(sb, b, 230, 67,
                differentWhole.Value == 0 ? 130 : 160,
                differentWhole.Value == 0 ? 130 : 80);
            // Do NOT print SAME/DIFFERENT or the yes/no comparison answer:
            // the dimensions themselves are the visual evidence to interpret.
            Text(sb, 52, 238, "Compare the shaded parts against each original whole", "hint");
        }

        return SvgEnd(sb);
    }

    private static void DrawGradeFiveOriginalWhole(
        StringBuilder sb, Area.Grid grid,
        double x, double y, double width, double height)
    {
        foreach (var cell in grid.Cells)
        {
            var klass = cell.Shaded
                ? "fraction-cell fraction-shaded"
                : "fraction-cell fraction-empty";
            sb.Append($"<rect x='{F(x + cell.X * width / grid.WholeWidth)}' " +
                $"y='{F(y + cell.Y * height / grid.WholeHeight)}' " +
                $"width='{F(cell.Width * width / grid.WholeWidth)}' " +
                $"height='{F(cell.Height * height / grid.WholeHeight)}' " +
                $"class='{klass}'/>");
        }
    }
}
