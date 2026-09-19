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
                "supporting.circle.arc_angle_degrees" => CircleArc(parameters),
                "geometry.coordinate.gradient_between_points" => CoordinateGradient(parameters),
                "geometry.coordinate.evaluate_linear_rule" or
                "geometry.coordinate.y_intercept_from_rule" => StraightLine(parameters),
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
                "geometry.perimeter_area.rectangle_missing_side" => RectangleMissingSide(parameters),
                "geometry.perimeter_area.triangle_area" => TriangleArea(parameters),
                "geometry.polygons.interior_angle_sum" or
                "geometry.polygons.missing_interior_angle" or
                "geometry.polygons.regular_interior_angle" => Polygon(parameters),
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
                "vectors.dot.exact_rational" or
                "vectors.magnitude.exact" or
                "vectors.between_points.exact" or
                "supporting.vectors.add" or
                "supporting.vectors.magnitude" => VectorPlane(parameters),
                "fractions.equivalent.number_line" => NumberLineParameters(parameters),
                "supporting.geometry.analytic.mixed" or
                "supporting.geometry.axis_distance" or
                "supporting.geometry.midpoint" or
                "supporting.geometry.reflect_axis" or
                "supporting.geometry.rotate90" or
                "supporting.geometry.translate_point" or
                "supporting.transformations.mixed" => CoordinateParameterVisual(parameters),
                "supporting.geometry.angle_around_point" or
                "supporting.geometry.angle_classify" or
                "supporting.geometry.turn_degrees" => AngleParameterVisual(parameters),
                "supporting.geometry.circle_area_pi_coefficient" or
                "supporting.geometry.circle_circumference_pi_coefficient" or
                "supporting.geometry.locus_equidistant" => CircleParameterVisual(parameters),
                "supporting.geometry.cuboid_volume" or
                "supporting.geometry.solid.mixed" or
                "supporting.geometry.surface_area_cuboid" => SolidParameterVisual(parameters),
                "supporting.geometry.compound_area" or
                "supporting.geometry.parallelogram_area" or
                "supporting.geometry.plane.mixed" or
                "supporting.geometry.shape_dimension" or
                "supporting.geometry.triangle_area" => PlaneGeometryParameterVisual(parameters),
                "supporting.trigonometry.graph_special" => TrigonometryGraphVisual(parameters),
                "supporting.trigonometry.area_sine_double" or
                "supporting.trigonometry.bearing" or
                "supporting.trigonometry.cosine_rule_square" or
                "supporting.trigonometry.sine_rule_exact" => TrigonometryTriangleVisual(parameters),
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

    private static string CircleArc(JsonElement p)
    {
        var numerator = GetInt(p, "numerator");
        var denominator = GetInt(p, "denominator");
        if (denominator <= 0 || numerator <= 0 || numerator >= denominator)
            throw new JsonException("Invalid circle-arc fraction.");

        var angle = 360d * numerator / denominator;
        var endRadians = (-90d + angle) * Math.PI / 180d;
        const double cx = 210;
        const double cy = 130;
        const double radius = 88;
        var sx = cx;
        var sy = cy - radius;
        var ex = cx + radius * Math.Cos(endRadians);
        var ey = cy + radius * Math.Sin(endRadians);
        var largeArc = angle > 180 ? 1 : 0;

        var sb = SvgStart("Circle showing the stated arc as a fraction of a full turn.");
        sb.Append("<circle cx='210' cy='130' r='88' class='shape fill'/>");
        sb.Append($"<path d='M {F(sx)} {F(sy)} A {F(radius)} {F(radius)} 0 {largeArc} 1 {F(ex)} {F(ey)}' class='arc-highlight'/>");
        sb.Append($"<line x1='210' y1='130' x2='{F(sx)}' y2='{F(sy)}' class='shape'/>");
        sb.Append($"<line x1='210' y1='130' x2='{F(ex)}' y2='{F(ey)}' class='shape'/>");
        Text(sb, 210, 135, $"{numerator}/{denominator} turn", "label");
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

    private static string StraightLine(JsonElement p)
    {
        var gradient = GetInt(p, "gradient");
        var intercept = GetInt(p, "intercept");
        var xValue = TryGetInt(p, "x");
        var yValue = xValue.HasValue ? gradient * xValue.Value + intercept : (int?)null;

        var max = Math.Max(
            6,
            new[] {
                Math.Abs(intercept),
                Math.Abs(xValue ?? 0),
                Math.Abs(yValue ?? 0),
                Math.Abs(gradient * 4 + intercept),
                Math.Abs(-gradient * 4 + intercept)
            }.Max() + 1);

        double X(int value) => 210 + value * (160d / max);
        double Y(int value) => 130 - value * (100d / max);

        var sb = SvgStart("Coordinate grid showing the straight-line rule from the question.");
        sb.Append("<line x1='35' y1='130' x2='385' y2='130' class='axis'/>");
        sb.Append("<line x1='210' y1='20' x2='210' y2='240' class='axis'/>");

        var leftX = -max;
        var rightX = max;
        var leftY = gradient * leftX + intercept;
        var rightY = gradient * rightX + intercept;
        sb.Append($"<line x1='{F(X(leftX))}' y1='{F(Y(leftY))}' x2='{F(X(rightX))}' y2='{F(Y(rightY))}' class='shape'/>");

        Point(sb, X(0), Y(intercept), $"(0, {intercept})");
        if (xValue.HasValue && yValue.HasValue)
            Point(sb, X(xValue.Value), Y(yValue.Value), $"({xValue.Value}, {yValue.Value})");

        Text(sb, 210, 246, $"y = {gradient}x {(intercept >= 0 ? "+" : "−")} {Math.Abs(intercept)}", "hint");
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

    private static string RectangleMissingSide(JsonElement p)
    {
        var knownSide = GetInt(p, "knownSide");
        var area = GetInt(p, "area");
        var sb = SvgStart("Rectangle with one known side and its area; the other side is unknown.");
        sb.Append("<rect x='75' y='60' width='270' height='145' class='shape fill'/>");
        Text(sb, 202, 230, knownSide.ToString(CultureInfo.InvariantCulture), "label");
        Text(sb, 48, 137, "x", "unknown");
        Text(sb, 210, 135, $"area = {area}", "hint");
        return SvgEnd(sb);
    }

    private static string TriangleArea(JsonElement p)
    {
        var @base = GetInt(p, "base");
        var height = GetInt(p, "height");
        var sb = SvgStart("Triangle with labelled base and perpendicular height.");
        sb.Append("<polygon points='70,205 350,205 240,55' class='shape fill'/>");
        sb.Append("<line x1='240' y1='55' x2='240' y2='205' class='dash shape'/>");
        sb.Append("<path d='M240 185 L260 185 L260 205' class='mark'/>");
        Text(sb, 202, 232, $"base = {@base}", "label");
        Text(sb, 252, 135, $"h = {height}", "label");
        return SvgEnd(sb);
    }

    private static string Polygon(JsonElement p)
    {
        var sides = GetInt(p, "sides");
        if (sides < 3 || sides > 20)
            throw new JsonException("Unsupported polygon side count.");

        var sb = SvgStart($"A {sides}-sided polygon for an interior-angle question.");
        var points = new List<string>(sides);
        const double cx = 210;
        const double cy = 132;
        const double radius = 95;
        for (var i = 0; i < sides; i++)
        {
            var angle = -Math.PI / 2 + 2 * Math.PI * i / sides;
            var x = cx + radius * Math.Cos(angle);
            var y = cy + radius * Math.Sin(angle);
            points.Add($"{F(x)},{F(y)}");
        }
        sb.Append($"<polygon points='{string.Join(" ", points)}' class='shape fill'/>");
        Text(sb, 210, 244, $"{sides} sides", "hint");

        if (p.TryGetProperty("knownSum", out var known) && known.TryGetInt32(out var knownSum))
            Text(sb, 210, 132, $"known angles total {knownSum}°", "label");
        else
            Text(sb, 210, 132, "interior angles", "label");

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
        Text(sb, 45, 140, verticalLabel ?? vertical?.ToString(CultureInfo.InvariantCulture) ?? "opposite", "label");
        Text(sb, 205, 238, horizontalLabel ?? horizontal?.ToString(CultureInfo.InvariantCulture) ?? "adjacent", "label");
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

    private static string NumberLineParameters(JsonElement p)
    {
        var values = IntegerParameters(p);
        var numerator = ValueOr(values, "numerator", "n", "leftNumerator", "sourceNumerator");
        var denominator = Math.Max(1, ValueOr(values, "denominator", "d", "leftDenominator", "sourceDenominator", fallback: 4));
        numerator = Math.Clamp(numerator == 0 ? 1 : numerator, 0, denominator);

        var sb = SvgStart("Number line showing an exact fraction position.");
        const double x0 = 55;
        const double x1 = 365;
        const double y = 132;
        sb.Append($"<line x1='{F(x0)}' y1='{F(y)}' x2='{F(x1)}' y2='{F(y)}' class='axis'/>");
        for (var i = 0; i <= denominator; i++)
        {
            var x = x0 + (x1 - x0) * i / denominator;
            sb.Append($"<line x1='{F(x)}' y1='124' x2='{F(x)}' y2='140' class='tick'/>");
        }
        var px = x0 + (x1 - x0) * numerator / denominator;
        Point(sb, px, y, $"{numerator}/{denominator}");
        Text(sb, x0, 162, "0", "label");
        Text(sb, x1 - 5, 162, "1", "label");
        AppendParameterSummary(sb, values);
        return SvgEnd(sb);
    }

    private static string CoordinateParameterVisual(JsonElement p)
    {
        var values = IntegerParameters(p);
        var x1 = ValueOr(values, "x", "x1", "sourceX", "startX", fallback: values.Values.ElementAtOrDefault(0));
        var y1 = ValueOr(values, "y", "y1", "sourceY", "startY", fallback: values.Values.ElementAtOrDefault(1));
        var x2 = ValueOr(values, "x2", "targetX", "endX", fallback: values.Values.ElementAtOrDefault(2));
        var y2 = ValueOr(values, "y2", "targetY", "endY", fallback: values.Values.ElementAtOrDefault(3));
        var max = Math.Max(4, new[] { x1, y1, x2, y2 }.Max(v => Math.Abs(v)) + 2);
        double X(int value) => 210 + value * (155d / max);
        double Y(int value) => 130 - value * (95d / max);

        var sb = SvgStart("Coordinate plane showing the values used by the transformation or analytic-geometry question.");
        sb.Append("<line x1='40' y1='130' x2='380' y2='130' class='axis'/>");
        sb.Append("<line x1='210' y1='25' x2='210' y2='235' class='axis'/>");
        Point(sb, X(x1), Y(y1), $"A({x1}, {y1})");
        if (values.Count >= 4 || x2 != 0 || y2 != 0)
            Point(sb, X(x2), Y(y2), $"B({x2}, {y2})");
        AppendParameterSummary(sb, values);
        return SvgEnd(sb);
    }

    private static string AngleParameterVisual(JsonElement p)
    {
        var values = IntegerParameters(p);
        var angle = Math.Clamp(
            Math.Abs(ValueOr(values, "angle", "knownAngle", "degrees", "turn", fallback: values.Values.FirstOrDefault())),
            0,
            360);
        var radians = -angle * Math.PI / 180d;
        const double cx = 150;
        const double cy = 175;
        const double radius = 100;
        var ex = cx + radius * Math.Cos(radians);
        var ey = cy + radius * Math.Sin(radians);

        var sb = SvgStart("Angle diagram tied to the generated question values.");
        sb.Append($"<line x1='{F(cx)}' y1='{F(cy)}' x2='{F(cx + radius)}' y2='{F(cy)}' class='shape'/>");
        sb.Append($"<line x1='{F(cx)}' y1='{F(cy)}' x2='{F(ex)}' y2='{F(ey)}' class='shape'/>");
        sb.Append($"<path d='M {F(cx + 48)} {F(cy)} A 48 48 0 0 0 {F(cx + 48 * Math.Cos(radians))} {F(cy + 48 * Math.Sin(radians))}' class='arc'/>");
        Text(sb, 185, 145, angle > 0 ? $"{angle}°" : "θ", "label");
        AppendParameterSummary(sb, values);
        return SvgEnd(sb);
    }

    private static string CircleParameterVisual(JsonElement p)
    {
        var values = IntegerParameters(p);
        var radius = Math.Max(1, Math.Abs(ValueOr(values, "radius", "r", "distance", fallback: values.Values.FirstOrDefault())));
        var sb = SvgStart("Circle or locus diagram tied to the generated question values.");
        sb.Append("<circle cx='190' cy='125' r='82' class='shape fill'/>");
        sb.Append("<line x1='190' y1='125' x2='272' y2='125' class='shape'/>");
        Text(sb, 222, 116, $"r = {radius}", "label");
        AppendParameterSummary(sb, values);
        return SvgEnd(sb);
    }

    private static string SolidParameterVisual(JsonElement p)
    {
        var values = IntegerParameters(p);
        var length = Math.Abs(ValueOr(values, "length", "l", "a", fallback: values.Values.ElementAtOrDefault(0)));
        var width = Math.Abs(ValueOr(values, "width", "w", "b", fallback: values.Values.ElementAtOrDefault(1)));
        var height = Math.Abs(ValueOr(values, "height", "h", "c", fallback: values.Values.ElementAtOrDefault(2)));
        length = length == 0 ? 1 : length;
        width = width == 0 ? 1 : width;
        height = height == 0 ? 1 : height;

        var sb = SvgStart("Solid-geometry diagram tied to the generated dimensions.");
        sb.Append("<polygon points='90,85 285,85 345,45 150,45' class='shape fill'/>");
        sb.Append("<polygon points='90,85 285,85 285,205 90,205' class='shape fill'/>");
        sb.Append("<polygon points='285,85 345,45 345,165 285,205' class='shape fill'/>");
        sb.Append("<line x1='90' y1='85' x2='150' y2='45' class='shape'/>");
        Text(sb, 175, 228, $"l={length}", "label");
        Text(sb, 315, 193, $"w={width}", "label");
        Text(sb, 62, 150, $"h={height}", "label");
        AppendParameterSummary(sb, values);
        return SvgEnd(sb);
    }

    private static string PlaneGeometryParameterVisual(JsonElement p)
    {
        var values = IntegerParameters(p);
        var a = Math.Abs(ValueOr(values, "base", "length", "a", "width", fallback: values.Values.ElementAtOrDefault(0)));
        var b = Math.Abs(ValueOr(values, "height", "b", fallback: values.Values.ElementAtOrDefault(1)));
        a = a == 0 ? 1 : a;
        b = b == 0 ? 1 : b;

        var sb = SvgStart("Plane-geometry diagram tied to the generated dimensions.");
        sb.Append("<polygon points='70,205 330,205 280,70 120,70' class='shape fill'/>");
        sb.Append("<line x1='120' y1='70' x2='120' y2='205' class='dash shape'/>");
        Text(sb, 185, 230, $"base = {a}", "label");
        Text(sb, 130, 145, $"h = {b}", "label");
        AppendParameterSummary(sb, values);
        return SvgEnd(sb);
    }

    private static string TrigonometryTriangleVisual(JsonElement p)
    {
        var values = IntegerParameters(p);
        var sb = SvgStart("Triangle diagram tied to the generated trigonometry values.");
        sb.Append("<polygon points='75,210 345,210 125,55' class='shape fill'/>");
        sb.Append("<path d='M125 190 A35 35 0 0 1 153 174' class='arc'/>");
        Text(sb, 145, 176, "θ", "unknown");
        AppendParameterSummary(sb, values);
        return SvgEnd(sb);
    }

    private static string TrigonometryGraphVisual(JsonElement p)
    {
        var values = IntegerParameters(p);
        var sb = SvgStart("Trigonometric graph with deterministic axes and generated parameters.");
        sb.Append("<line x1='35' y1='130' x2='385' y2='130' class='axis'/>");
        sb.Append("<line x1='55' y1='25' x2='55' y2='235' class='axis'/>");
        var points = new List<string>();
        for (var i = 0; i <= 32; i++)
        {
            var x = 55 + i * 10;
            var y = 130 - 70 * Math.Sin(i * Math.PI / 8);
            points.Add($"{F(x)},{F(y)}");
        }
        sb.Append($"<polyline points='{string.Join(" ", points)}' class='shape'/>");
        AppendParameterSummary(sb, values);
        return SvgEnd(sb);
    }

    private static SortedDictionary<string, int> IntegerParameters(JsonElement p)
    {
        var result = new SortedDictionary<string, int>(StringComparer.Ordinal);
        if (p.ValueKind != JsonValueKind.Object)
            return result;
        foreach (var property in p.EnumerateObject())
        {
            if (property.Value.TryGetInt32(out var value))
                result[property.Name] = value;
        }
        return result;
    }

    private static int ValueOr(
        IReadOnlyDictionary<string, int> values,
        string first,
        string? second = null,
        string? third = null,
        string? fourth = null,
        int fallback = 0)
    {
        foreach (var key in new[] { first, second, third, fourth })
        {
            if (key is not null && values.TryGetValue(key, out var value))
                return value;
        }
        return fallback;
    }

    private static void AppendParameterSummary(
        StringBuilder sb,
        IReadOnlyDictionary<string, int> values)
    {
        var summary = string.Join(
            "   ",
            values.Take(4).Select(pair => $"{pair.Key}={pair.Value}"));
        if (!string.IsNullOrWhiteSpace(summary))
            Text(sb, 210, 250, summary, "hint");
    }

    private static StringBuilder SvgStart(string label)
    {
        var safeLabel = WebUtility.HtmlEncode(label);
        var sb = new StringBuilder();
        sb.Append($"<svg class='practice-math-visual' viewBox='0 0 420 260' role='img' aria-label='{safeLabel}'>");
        sb.Append("<style>.fraction-cell{stroke:currentColor;stroke-width:2}.fraction-shaded{fill:rgba(18,114,133,.38)}.fraction-empty{fill:rgba(255,255,255,.35)}.shape{stroke:currentColor;stroke-width:3;fill:none;stroke-linecap:round;stroke-linejoin:round}.fill{fill:rgba(255,255,255,.28)}.axis{stroke:currentColor;stroke-width:1.5;opacity:.55}.tick{stroke:currentColor;stroke-width:1;opacity:.45}.mark,.arc{stroke:currentColor;stroke-width:2;fill:none}.arc-highlight{stroke:currentColor;stroke-width:8;fill:none;stroke-linecap:round}.dash{stroke-dasharray:7 6;opacity:.7}.label{font:700 16px system-ui,sans-serif;fill:currentColor}.unknown{font:800 18px system-ui,sans-serif;fill:currentColor}.hint{font:600 12px system-ui,sans-serif;fill:currentColor;opacity:.75}.point-label{font:700 13px system-ui,sans-serif;fill:currentColor}.point{fill:currentColor}.vector{stroke:currentColor;stroke-width:3}.secondary{stroke-dasharray:4 3}.arrow-head{fill:currentColor}</style>");
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
