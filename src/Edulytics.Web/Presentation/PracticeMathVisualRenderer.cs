using System.Globalization;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Edulytics.Web.Presentation;

/// <summary>
/// Deterministic learner-facing mathematics visuals generated only from
/// server-owned exact question-family parameters. No arbitrary HTML or
/// model-generated image content is accepted.
/// </summary>
public static class PracticeMathVisualRenderer
{
    public static string? RenderSvg(string? family, string? generationParametersJson)
    {
        if (string.IsNullOrWhiteSpace(family) || string.IsNullOrWhiteSpace(generationParametersJson))
            return null;

        try
        {
            using var document = JsonDocument.Parse(generationParametersJson);
            var root = document.RootElement;
            var parameters = root.TryGetProperty("parameters", out var nested)
                ? nested
                : root;

            return family switch
            {
                "geometry.coordinate.gradient_between_points" => CoordinateGradient(parameters),
                "geometry.angles.parallel_lines" => ParallelLines(parameters),
                "geometry.angles.supplementary" => Supplementary(parameters),
                "geometry.angles.algebraic_supplementary" => AlgebraicSupplementary(parameters),
                "geometry.similarity.find_missing_length" or
                "geometry.similarity.scale_factor" => Similarity(parameters),
                "geometry.congruence.identify_criterion" => Congruence(parameters),
                "geometry.surface_area.rectangular_prism" or
                "geometry.volume.rectangular_prism" or
                "geometry.surface_area_volume.rectangular_prism_surface_area" or
                "geometry.surface_area_volume.rectangular_prism_volume" => RectangularPrism(parameters),
                "geometry.rectangle.area.exact" or
                "geometry.rectangle.perimeter.exact" or
                "geometry.perimeter_area.rectangle_area" or
                "geometry.perimeter_area.rectangle_perimeter" => Rectangle(parameters),
                "geometry.right_triangle.pythagorean.exact" or
                "geometry.right_triangle.pythagorean.find_leg_exact" or
                "trigonometry.right_triangle.ratio_exact" or
                "trigonometry.right_triangle.find_side_exact" or
                "trigonometry.right_triangle.find_angle_exact" or
                "trigonometry.modelling.contextual" => RightTriangle(parameters),
                "fractions.represent.interpret.fraction_bar" => FractionBar(parameters),
                "vectors.add.exact_rational" or
                "vectors.subtract.exact_rational" or
                "vectors.scalar_multiply.exact_rational" or
                "vectors.magnitude.exact" or
                "vectors.between_points.exact" => VectorPlane(parameters),
                _ => null
            };
        }
        catch (JsonException)
        {
            return null;
        }
    }

    private static string FractionBar(JsonElement p)
    {
        var numerator = GetInt(p, "numerator");
        var denominator = GetInt(p, "denominator");
        if (denominator <= 0 || numerator < 0 || numerator > denominator)
            throw new JsonException("Invalid fraction-bar parameters.");

        var sb = SvgStart("Fraction bar divided into equal parts with the required number shaded.");
        const double x = 45;
        const double y = 92;
        const double width = 330;
        const double height = 82;
        var cell = width / denominator;

        for (var i = 0; i < denominator; i++)
        {
            var cx = x + i * cell;
            var fillClass = i < numerator ? "fraction-shaded" : "fraction-empty";
            sb.Append($"<rect x='{F(cx)}' y='{F(y)}' width='{F(cell)}' height='{F(height)}' class='fraction-cell {fillClass}'/>");
        }

        Text(sb, 210, 208, $"{numerator} shaded out of {denominator} equal parts", "hint");
        return SvgEnd(sb);
    }

    private static string CoordinateGradient(JsonElement p)
    {
        var x1 = GetInt(p, "x1");
        var y1 = GetInt(p, "y1");
        var x2 = GetInt(p, "x2");
        var y2 = GetInt(p, "y2");

        var all = new[] { x1, y1, x2, y2, -1, 1 };
        var max = Math.Max(4, all.Max(v => Math.Abs(v)) + 1);
        double X(int value) => 210 + value * (170d / max);
        double Y(int value) => 130 - value * (100d / max);

        var sb = SvgStart("Coordinate grid showing the two points and the line joining them.");
        sb.Append("<line x1='35' y1='130' x2='385' y2='130' class='axis'/>");
        sb.Append("<line x1='210' y1='20' x2='210' y2='240' class='axis'/>");

        for (var i = -max; i <= max; i++)
        {
            var px = F(X(i));
            var py = F(Y(i));
            sb.Append($"<line x1='{px}' y1='126' x2='{px}' y2='134' class='tick'/>");
            sb.Append($"<line x1='206' y1='{py}' x2='214' y2='{py}' class='tick'/>");
        }

        sb.Append($"<line x1='{F(X(x1))}' y1='{F(Y(y1))}' x2='{F(X(x2))}' y2='{F(Y(y2))}' class='shape'/>");
        Point(sb, X(x1), Y(y1), $"({x1}, {y1})");
        Point(sb, X(x2), Y(y2), $"({x2}, {y2})");
        return SvgEnd(sb);
    }

    private static string ParallelLines(JsonElement p)
    {
        var known = GetInt(p, "knownAngle");
        var relationship = GetInt(p, "relationship");

        var sb = SvgStart("Two parallel lines cut by a transversal with one known angle and one unknown angle.");
        sb.Append("<line x1='45' y1='75' x2='375' y2='75' class='shape'/>");
        sb.Append("<line x1='45' y1='190' x2='375' y2='190' class='shape'/>");
        sb.Append("<line x1='120' y1='230' x2='300' y2='30' class='shape'/>");
        sb.Append("<path d='M80 66 l12 0 M84 72 l12 0' class='mark'/>");
        sb.Append("<path d='M310 181 l12 0 M314 187 l12 0' class='mark'/>");
        Text(sb, 230, 61, $"{known}°", "label");
        Text(sb, relationship == 0 ? 270 : 200, relationship == 0 ? 176 : 105, "x°", "unknown");
        return SvgEnd(sb);
    }

    private static string Supplementary(JsonElement p)
    {
        var known = GetInt(p, "knownAngle");
        var sb = SvgStart("A straight line split into two adjacent angles.");
        sb.Append("<line x1='45' y1='185' x2='375' y2='185' class='shape'/>");
        sb.Append("<line x1='210' y1='185' x2='295' y2='55' class='shape'/>");
        sb.Append("<path d='M260 185 A50 50 0 0 0 237 143' class='arc'/>");
        sb.Append("<path d='M205 135 A50 50 0 0 0 160 185' class='arc'/>");
        Text(sb, 270, 155, $"{known}°", "label");
        Text(sb, 155, 155, "x°", "unknown");
        return SvgEnd(sb);
    }

    private static string AlgebraicSupplementary(JsonElement p)
    {
        var coefficientA = GetInt(p, "coefficientA");
        var offsetA = GetInt(p, "offsetA");
        var coefficientB = GetInt(p, "coefficientB");
        var offsetB = GetInt(p, "offsetB");

        var sb = SvgStart("A straight line split into two adjacent algebraic angles.");
        sb.Append("<line x1=\"45\" y1=\"185\" x2=\"375\" y2=\"185\" class=\"shape\"/>");
        sb.Append("<line x1=\"210\" y1=\"185\" x2=\"295\" y2=\"55\" class=\"shape\"/>");
        sb.Append("<path d=\"M260 185 A50 50 0 0 0 237 143\" class=\"arc\"/>");
        sb.Append("<path d=\"M205 135 A50 50 0 0 0 160 185\" class=\"arc\"/>");
        Text(sb, 270, 155, $"({coefficientA}x + {offsetA})°", "label");
        Text(sb, 125, 155, $"({coefficientB}x + {offsetB})°", "label");
        return SvgEnd(sb);
    }

    private static string Similarity(JsonElement p)
    {
        var source = GetInt(p, "sourceLength");
        var target = TryGetInt(p, "targetLength");
        var factor = TryGetInt(p, "scaleFactor");

        var sb = SvgStart("Two similar triangles showing corresponding sides.");
        sb.Append("<polygon points='45,205 145,205 95,105' class='shape fill'/>");
        sb.Append("<polygon points='220,220 385,220 302,55' class='shape fill'/>");
        Text(sb, 85, 224, source.ToString(CultureInfo.InvariantCulture), "label");
        Text(sb, 290, 242, target?.ToString(CultureInfo.InvariantCulture) ?? "?", "label");
        Text(sb, 94, 90, "A", "point-label");
        Text(sb, 301, 40, "A′", "point-label");
        if (factor.HasValue)
            Text(sb, 210, 28, $"scale factor {factor.Value}", "hint");
        return SvgEnd(sb);
    }

    private static string Congruence(JsonElement p)
    {
        var sideA = GetInt(p, "sideA");
        var sideB = GetInt(p, "sideB");
        var sideC = GetInt(p, "sideC");
        var criterion = GetInt(p, "criterion");

        var sb = SvgStart("Two triangles with corresponding equality information for a congruence question.");
        sb.Append("<polygon points='35,205 155,205 105,85' class='shape fill'/>");
        sb.Append("<polygon points='240,205 380,205 305,85' class='shape fill'/>");
        Text(sb, 72, 222, sideA.ToString(CultureInfo.InvariantCulture), "label");
        Text(sb, 285, 222, sideA.ToString(CultureInfo.InvariantCulture), "label");

        if (criterion == 0)
        {
            Text(sb, 42, 145, sideB.ToString(CultureInfo.InvariantCulture), "label");
            Text(sb, 350, 145, sideB.ToString(CultureInfo.InvariantCulture), "label");
            Text(sb, 125, 145, sideC.ToString(CultureInfo.InvariantCulture), "label");
            Text(sb, 250, 145, sideC.ToString(CultureInfo.InvariantCulture), "label");
        }

        Text(sb, 210, 35, "corresponding measurements are equal", "hint");
        return SvgEnd(sb);
    }

    private static string Rectangle(JsonElement p)
    {
        var length = GetInt(p, "length");
        var width = GetInt(p, "width");
        var sb = SvgStart("Rectangle with labelled length and width.");
        sb.Append("<rect x='75' y='60' width='270' height='145' class='shape fill'/>");
        Text(sb, 202, 230, length.ToString(CultureInfo.InvariantCulture), "label");
        Text(sb, 48, 137, width.ToString(CultureInfo.InvariantCulture), "label");
        return SvgEnd(sb);
    }

    private static string RectangularPrism(JsonElement p)
    {
        var length = GetInt(p, "length");
        var width = GetInt(p, "width");
        var height = GetInt(p, "height");
        var sb = SvgStart("Rectangular prism with labelled length, width and height.");
        sb.Append("<polygon points='95,85 290,85 350,45 155,45' class='shape fill'/>");
        sb.Append("<polygon points='95,85 290,85 290,205 95,205' class='shape fill'/>");
        sb.Append("<polygon points='290,85 350,45 350,165 290,205' class='shape fill'/>");
        sb.Append("<line x1='95' y1='85' x2='155' y2='45' class='shape'/>");
        sb.Append("<line x1='155' y1='45' x2='155' y2='165' class='shape dash'/>");
        sb.Append("<line x1='155' y1='165' x2='350' y2='165' class='shape dash'/>");
        sb.Append("<line x1='95' y1='205' x2='155' y2='165' class='shape dash'/>");
        Text(sb, 180, 229, $"l = {length}", "label");
        Text(sb, 330, 195, $"w = {width}", "label");
        Text(sb, 64, 150, $"h = {height}", "label");
        return SvgEnd(sb);
    }

    private static string RightTriangle(JsonElement p)
    {
        var legA = TryGetInt(p, "legA");
        var legB = TryGetInt(p, "legB");
        var opposite = TryGetInt(p, "opposite");
        var adjacent = TryGetInt(p, "adjacent");
        var hypotenuse = TryGetInt(p, "hypotenuse");
        var expectedAngle = TryGetInt(p, "expectedAngle");
        var knownLeg = TryGetInt(p, "knownLeg");
        var askLeg = TryGetInt(p, "askLeg");

        int? vertical;
        int? horizontal;
        string? verticalLabel = null;
        string? horizontalLabel = null;

        if (knownLeg.HasValue && askLeg.HasValue)
        {
            if (askLeg.Value == 0)
            {
                vertical = null;
                horizontal = knownLeg.Value;
                verticalLabel = "x";
            }
            else
            {
                vertical = knownLeg.Value;
                horizontal = null;
                horizontalLabel = "x";
            }
        }
        else
        {
            vertical = opposite ?? legA;
            horizontal = adjacent ?? legB;
        }

        var sb = SvgStart("Right triangle with side and angle labels tied to the question values.");
        sb.Append("<polygon points='85,215 335,215 85,55' class='shape fill'/>");
        sb.Append("<path d='M85 195 L105 195 L105 215' class='mark'/>");
        Text(sb, 45, 140, vertical?.ToString(CultureInfo.InvariantCulture) ?? "opposite", "label");
        Text(sb, 205, 238, horizontal?.ToString(CultureInfo.InvariantCulture) ?? "adjacent", "label");
        Text(sb, 225, 122, hypotenuse?.ToString(CultureInfo.InvariantCulture) ?? "hypotenuse", hypotenuse.HasValue ? "label" : "hint");
        Text(sb, 306, 198, expectedAngle.HasValue ? $"{expectedAngle.Value}°" : "θ", "unknown");
        return SvgEnd(sb);
    }

    private static string VectorPlane(JsonElement p)
    {
        var ax = TryGetInt(p, "ax") ?? TryGetInt(p, "x1") ?? 2;
        var ay = TryGetInt(p, "ay") ?? TryGetInt(p, "y1") ?? 1;
        var bx = TryGetInt(p, "bx") ?? TryGetInt(p, "x2") ?? -1;
        var by = TryGetInt(p, "by") ?? TryGetInt(p, "y2") ?? 3;
        var max = Math.Max(4, new[] { ax, ay, bx, by }.Max(v => Math.Abs(v)) + 1);
        double X(int value) => 210 + value * (160d / max);
        double Y(int value) => 130 - value * (100d / max);

        var sb = SvgStart("Coordinate plane with directed vector arrows.");
        sb.Append("<defs><marker id='practice-vector-arrow' markerWidth='8' markerHeight='8' refX='6' refY='3' orient='auto'><path d='M0,0 L0,6 L7,3 z' class='arrow-head'/></marker></defs>");
        sb.Append("<line x1='35' y1='130' x2='385' y2='130' class='axis'/>");
        sb.Append("<line x1='210' y1='20' x2='210' y2='240' class='axis'/>");
        sb.Append($"<line x1='210' y1='130' x2='{F(X(ax))}' y2='{F(Y(ay))}' class='vector' marker-end='url(#practice-vector-arrow)'/>");
        sb.Append($"<line x1='210' y1='130' x2='{F(X(bx))}' y2='{F(Y(by))}' class='vector secondary' marker-end='url(#practice-vector-arrow)'/>");
        Text(sb, X(ax) + 10, Y(ay) - 8, "a", "label");
        Text(sb, X(bx) + 10, Y(by) - 8, "b", "label");
        return SvgEnd(sb);
    }

    private static StringBuilder SvgStart(string label)
    {
        var safeLabel = WebUtility.HtmlEncode(label);
        var sb = new StringBuilder();
        sb.Append($"<svg class='practice-math-visual' viewBox='0 0 420 260' role='img' aria-label='{safeLabel}'>");
        sb.Append("<style>.fraction-cell{stroke:currentColor;stroke-width:2}.fraction-shaded{fill:rgba(18,114,133,.38)}.fraction-empty{fill:rgba(255,255,255,.35)}.shape{stroke:currentColor;stroke-width:3;fill:none;stroke-linecap:round;stroke-linejoin:round}.fill{fill:rgba(255,255,255,.28)}.axis{stroke:currentColor;stroke-width:1.5;opacity:.55}.tick{stroke:currentColor;stroke-width:1;opacity:.45}.mark,.arc{stroke:currentColor;stroke-width:2;fill:none}.dash{stroke-dasharray:7 6;opacity:.7}.label{font:700 16px system-ui,sans-serif;fill:currentColor}.unknown{font:800 18px system-ui,sans-serif;fill:currentColor}.hint{font:600 12px system-ui,sans-serif;fill:currentColor;opacity:.75}.point-label{font:700 13px system-ui,sans-serif;fill:currentColor}.point{fill:currentColor}.vector{stroke:currentColor;stroke-width:3}.secondary{stroke-dasharray:4 3}.arrow-head{fill:currentColor}</style>");
        return sb;
    }

    private static string SvgEnd(StringBuilder sb)
    {
        sb.Append("</svg>");
        return sb.ToString();
    }

    private static void Point(StringBuilder sb, double x, double y, string label)
    {
        sb.Append($"<circle cx='{F(x)}' cy='{F(y)}' r='5' class='point'/>");
        Text(sb, x + 8, y - 8, label, "point-label");
    }

    private static void Text(StringBuilder sb, double x, double y, string text, string cssClass)
    {
        sb.Append($"<text x='{F(x)}' y='{F(y)}' class='{cssClass}'>{WebUtility.HtmlEncode(text)}</text>");
    }

    private static int GetInt(JsonElement p, string name)
    {
        if (!p.TryGetProperty(name, out var value) || !value.TryGetInt32(out var result))
            throw new JsonException($"Missing integer diagram parameter: {name}");
        return result;
    }

    private static int? TryGetInt(JsonElement p, string name) =>
        p.TryGetProperty(name, out var value) && value.TryGetInt32(out var result)
            ? result
            : null;

    private static string F(double value) =>
        value.ToString("0.##", CultureInfo.InvariantCulture);
}
