using System.Text.Json;

namespace Edulytics.Web.Presentation;

public static partial class PracticeMathVisualRenderer
{
    // First-party diagram built from exact, server-verified Grade 4 U06:L22
    // parameters. It depicts dimensions only, never the calculated answer.
    private static string? GradeFourLShapedMeasurementVisual(JsonElement p)
    {
        var mode = GetInt(p, "caseType");
        var sheetWidth = GetInt(p, "sheetWidthInches");
        var sheetHeight = GetInt(p, "sheetHeightInches");
        var bannerHeight = GetInt(p, "bannerHeightInches");
        var bannerFeet = GetInt(p, "bannerLengthFeet");
        var leftFeet = GetInt(p, "leftWidthFeet");
        var rightYards = GetInt(p, "rightWidthYards");
        var lowerYards = GetInt(p, "lowerHeightYards");
        var riseFeet = GetInt(p, "upperRiseFeet");
        var supplyInches = GetInt(p, "suppliedInches");

        // Geometry and measurement metadata must match the exact generator's
        // accepted region. Unsupported/problematic metadata is not illustrated.
        if (mode is < 0 or > 4 ||
            sheetWidth is < 36 or > 49 ||
            sheetHeight is < 24 or > 34 ||
            bannerHeight != 8 ||
            bannerFeet is < 6 or > 8 ||
            leftFeet is < 8 or > 13 ||
            rightYards is < 2 or > 4 ||
            lowerYards is < 3 or > 5 ||
            riseFeet is < 2 or > 6)
            return null;

        var bottomFeet = leftFeet + 3 * rightYards;
        var lowerFeet = 3 * lowerYards;
        var totalHeightFeet = lowerFeet + riseFeet;
        var perimeterFeet = 2 * (bottomFeet + totalHeightFeet);
        if (supplyInches <= 12 * perimeterFeet ||
            supplyInches > 12 * perimeterFeet + 500)
            return null;

        if (mode == 0)
        {
            // A single 6-8 ft banner is wider than the specified poster sheet.
            // The original task compares AREAS, not physical fit. Separate
            // rectangles avoid misleadingly suggesting that the banner fits.
            var sb = SvgStart("Separate poster sheet and banner rectangles, not to scale. Compare their areas after converting feet to inches.");
            sb.Append("<rect x='50' y='72' width='128' height='111' class='shape fill'/>");
            sb.Append("<rect x='225' y='95' width='160' height='47' class='shape fill'/>");
            Text(sb, 114, 202, $"Poster: {sheetWidth} in × {sheetHeight} in", "hint");
            Text(sb, 305, 163, $"Banner: {bannerFeet} ft × {bannerHeight} in", "hint");
            return SvgEnd(sb);
        }

        // The six exterior edges are bottom, right rise, inset, inner rise,
        // upper horizontal, and left side. Coordinates encode actual ratios.
        const double x0 = 70;
        const double y0 = 207;
        var scale = Math.Min(250d / bottomFeet, 144d / totalHeightFeet);
        var xRight = x0 + bottomFeet * scale;
        var xInner = x0 + leftFeet * scale;
        var yStep = y0 - lowerFeet * scale;
        var yTop = yStep - riseFeet * scale;

        var svg = SvgStart("Six-sided L-shaped classroom with dimension annotations. Diagram is proportional and does not show the perimeter or area answer.");
        svg.Append($"<polygon points='{F(x0)},{F(y0)} {F(xRight)},{F(y0)} {F(xRight)},{F(yStep)} {F(xInner)},{F(yStep)} {F(xInner)},{F(yTop)} {F(x0)},{F(yTop)}' class='shape fill'/>");
        Text(svg, (x0 + xRight) / 2, 229, $"{bottomFeet} ft", "hint");
        Text(svg, xRight + 12, (y0 + yStep) / 2, $"{lowerYards} yd", "hint");
        Text(svg, (xInner + xRight) / 2, yStep - 9, $"{rightYards} yd", "hint");
        Text(svg, xInner + 9, (yTop + yStep) / 2, $"{riseFeet} ft", "hint");
        Text(svg, (x0 + xInner) / 2, yTop - 9, $"{leftFeet} ft", "hint");
        Text(svg, 30, (y0 + yTop) / 2, $"{totalHeightFeet} ft", "hint");
        if (mode == 4)
            Text(svg, 210, 251, $"Available baseboard: {supplyInches} in", "hint");
        return SvgEnd(svg);
    }
}
