using System.Globalization;
using System.Net;
using System.Text;
using Edulytics.Core.Mathematics.Visuals;

namespace Edulytics.Web.Presentation;

/// <summary>
/// Deterministic renderer for typed Advanced Mathematics visual contracts.
/// It accepts no arbitrary SVG/HTML and does not perform mathematical
/// verification; exact engines remain authoritative for the mathematics.
/// </summary>
public static class AdvancedMathematicsVisualRenderer
{
    private const int Width = 720;
    private const int Height = 440;
    private const double Left = 62d;
    private const double Right = 692d;
    private const double Top = 34d;
    private const double Bottom = 398d;
    private const int CurveSamples = 240;

    public static string RenderSvg(MathematicalVisualSpec spec)
    {
        MathematicalVisualSpecValidator.Validate(spec);

        return spec switch
        {
            CoordinatePlaneVisualSpec coordinate =>
                CoordinatePlane(coordinate),
            FunctionGraphVisualSpec function =>
                FunctionGraph(function),
            MultiFunctionGraphVisualSpec multiple =>
                MultiFunctionGraph(multiple),
            GeometryDiagramVisualSpec geometry =>
                GeometryDiagram(geometry),
            VectorDiagramVisualSpec vectors =>
                VectorDiagram(vectors),
            MatrixDisplayVisualSpec matrices =>
                MatrixDisplay(matrices),
            ArgandDiagramVisualSpec argand =>
                ArgandDiagram(argand),
            RegionOfIntegrationVisualSpec region =>
                RegionOfIntegration(region),
            StatisticalPlotVisualSpec statistics =>
                StatisticalPlot(statistics),
            _ => throw new InvalidOperationException(
                $"Unsupported mathematical visual kind: {spec.Kind}.")
        };
    }

    private static string CoordinatePlane(
        CoordinatePlaneVisualSpec spec)
    {
        var plot = new PlotContext(spec.Window);
        var sb = Start(spec.Accessibility);
        Axes(sb, plot);

        foreach (var segment in spec.Segments)
            Segment(sb, plot, segment);

        foreach (var point in spec.Points)
            Point(sb, plot, point);

        return End(sb);
    }

    private static string FunctionGraph(
        FunctionGraphVisualSpec spec)
    {
        var plot = new PlotContext(spec.Window);
        var sb = Start(spec.Accessibility);
        Axes(sb, plot);
        Curve(sb, plot, spec.Curve, 0);

        foreach (var point in spec.HighlightedPoints)
            Point(sb, plot, point);

        return End(sb);
    }

    private static string MultiFunctionGraph(
        MultiFunctionGraphVisualSpec spec)
    {
        var plot = new PlotContext(spec.Window);
        var sb = Start(spec.Accessibility);
        Axes(sb, plot);

        for (var i = 0; i < spec.Curves.Count; i++)
            Curve(sb, plot, spec.Curves[i], i);

        foreach (var point in spec.HighlightedPoints)
            Point(sb, plot, point);

        return End(sb);
    }

    private static string GeometryDiagram(
        GeometryDiagramVisualSpec spec)
    {
        var sb = Start(spec.Accessibility);

        var points = spec.Points;
        if (points.Count == 0)
            return End(sb);

        var bounds = BoundsFor(points);
        var plot = new PlotContext(bounds);

        foreach (var polygon in spec.Polygons)
        {
            var coords = polygon
                .Select(index => points[index])
                .Select(point =>
                    $"{F(plot.X(point.X))},{F(plot.Y(point.Y))}");

            sb.Append(
                $"<polygon points='{string.Join(" ", coords)}' class='amv-polygon'/>");
        }

        foreach (var segment in spec.Segments)
            Segment(sb, plot, segment);

        foreach (var point in points)
            Point(sb, plot, point);

        return End(sb);
    }

    private static string VectorDiagram(
        VectorDiagramVisualSpec spec)
    {
        var plot = new PlotContext(spec.Window);
        var sb = Start(spec.Accessibility, includeArrowMarker: true);
        Axes(sb, plot);

        foreach (var vector in spec.Vectors)
        {
            sb.Append(
                $"<line x1='{F(plot.X(vector.Start.X))}' " +
                $"y1='{F(plot.Y(vector.Start.Y))}' " +
                $"x2='{F(plot.X(vector.End.X))}' " +
                $"y2='{F(plot.Y(vector.End.Y))}' " +
                "class='amv-vector' marker-end='url(#amv-arrow)'/>");

            if (!string.IsNullOrWhiteSpace(vector.Label))
            {
                Label(
                    sb,
                    (plot.X(vector.Start.X) + plot.X(vector.End.X)) / 2d,
                    (plot.Y(vector.Start.Y) + plot.Y(vector.End.Y)) / 2d - 10d,
                    vector.Label!);
            }
        }

        foreach (var point in spec.Points)
            Point(sb, plot, point);

        return End(sb);
    }

    private static string MatrixDisplay(
        MatrixDisplayVisualSpec spec)
    {
        var sb = Start(spec.Accessibility);
        var count = spec.Matrices.Count;
        var available = 620d;
        var boxWidth = available / count;
        var startX = 50d;

        for (var m = 0; m < count; m++)
        {
            var matrix = spec.Matrices[m];
            var x = startX + m * boxWidth;
            var y = 80d;
            var cellWidth = Math.Min(
                70d,
                (boxWidth - 36d) / matrix.Columns);
            var cellHeight = 48d;
            var matrixWidth = cellWidth * matrix.Columns;
            var matrixHeight = cellHeight * matrix.Rows;

            if (!string.IsNullOrWhiteSpace(matrix.Label))
                Label(sb, x + matrixWidth / 2d, y - 26d, matrix.Label!);

            sb.Append(
                $"<path d='M {F(x + 12)} {F(y)} H {F(x)} " +
                $"V {F(y + matrixHeight)} H {F(x + 12)}' class='amv-bracket'/>");
            sb.Append(
                $"<path d='M {F(x + matrixWidth + 18)} {F(y)} " +
                $"H {F(x + matrixWidth + 30)} V {F(y + matrixHeight)} " +
                $"H {F(x + matrixWidth + 18)}' class='amv-bracket'/>");

            for (var row = 0; row < matrix.Rows; row++)
            {
                for (var col = 0; col < matrix.Columns; col++)
                {
                    var index = row * matrix.Columns + col;
                    Label(
                        sb,
                        x + 15d + col * cellWidth + cellWidth / 2d,
                        y + row * cellHeight + cellHeight / 2d + 6d,
                        matrix.Values[index],
                        "amv-matrix-value");
                }
            }
        }

        return End(sb);
    }

    private static string ArgandDiagram(
        ArgandDiagramVisualSpec spec)
    {
        var plot = new PlotContext(spec.Window);
        var sb = Start(spec.Accessibility);
        Axes(sb, plot, "Re", "Im");

        foreach (var complex in spec.Points)
        {
            var point = new MathematicalPoint(
                complex.Real,
                complex.Imaginary,
                complex.Label);
            Point(sb, plot, point);
            sb.Append(
                $"<line x1='{F(plot.X(0m))}' y1='{F(plot.Y(0m))}' " +
                $"x2='{F(plot.X(complex.Real))}' y2='{F(plot.Y(complex.Imaginary))}' " +
                "class='amv-guide'/>");
        }

        return End(sb);
    }

    private static string RegionOfIntegration(
        RegionOfIntegrationVisualSpec spec)
    {
        var plot = new PlotContext(spec.Window);
        var sb = Start(spec.Accessibility);
        Axes(sb, plot);

        var upper = SampleCurve(
            spec.UpperCurve,
            spec.XFrom,
            spec.XTo,
            120);
        var lower = SampleCurve(
            spec.LowerCurve,
            spec.XFrom,
            spec.XTo,
            120);

        if (upper.Count > 1 &&
            lower.Count > 1 &&
            upper.All(p => IsFinite(p.Y)) &&
            lower.All(p => IsFinite(p.Y)))
        {
            var polygon = upper
                .Concat(lower.AsEnumerable().Reverse())
                .Select(point =>
                    $"{F(plot.X(point.X))},{F(plot.Y(point.Y))}");

            sb.Append(
                $"<polygon points='{string.Join(" ", polygon)}' class='amv-region'/>");
        }

        Curve(sb, plot, spec.UpperCurve, 0);
        Curve(sb, plot, spec.LowerCurve, 1);

        foreach (var point in spec.Intersections)
            Point(sb, plot, point);

        return End(sb);
    }

    private static string StatisticalPlot(
        StatisticalPlotVisualSpec spec)
    {
        var plot = new PlotContext(spec.Window);
        var sb = Start(spec.Accessibility);
        Axes(sb, plot);

        switch (spec.PlotKind)
        {
            case StatisticalPlotKind.Scatter:
                foreach (var point in spec.Points)
                {
                    Point(
                        sb,
                        plot,
                        new MathematicalPoint(
                            point.X,
                            point.Y,
                            point.Label));
                }
                break;

            case StatisticalPlotKind.Line:
                var ordered = spec.Points.OrderBy(x => x.X).ToArray();
                if (ordered.Length > 1)
                {
                    var points = ordered.Select(point =>
                        $"{F(plot.X(point.X))},{F(plot.Y(point.Y))}");
                    sb.Append(
                        $"<polyline points='{string.Join(" ", points)}' class='amv-series'/>");
                }

                foreach (var point in ordered)
                {
                    Point(
                        sb,
                        plot,
                        new MathematicalPoint(
                            point.X,
                            point.Y,
                            point.Label));
                }
                break;

            case StatisticalPlotKind.Bar:
                var barWidth = Math.Max(
                    5d,
                    Math.Min(
                        44d,
                        (Right - Left) /
                        Math.Max(1, spec.Points.Count) * 0.65d));

                foreach (var point in spec.Points)
                {
                    var x = plot.X(point.X) - barWidth / 2d;
                    var yZero = plot.Y(0m);
                    var yValue = plot.Y(point.Y);
                    var top = Math.Min(yZero, yValue);
                    var height = Math.Max(1d, Math.Abs(yZero - yValue));

                    sb.Append(
                        $"<rect x='{F(x)}' y='{F(top)}' " +
                        $"width='{F(barWidth)}' height='{F(height)}' " +
                        "class='amv-bar'/>");

                    if (!string.IsNullOrWhiteSpace(point.Label))
                    {
                        Label(
                            sb,
                            plot.X(point.X),
                            Math.Min(Bottom - 4d, Math.Max(Top + 14d, top - 8d)),
                            point.Label!);
                    }
                }
                break;

            default:
                throw new InvalidOperationException(
                    $"Unsupported statistical plot kind: {spec.PlotKind}.");
        }

        return End(sb);
    }

    private static void Axes(
        StringBuilder sb,
        PlotContext plot,
        string xLabel = "x",
        string yLabel = "y")
    {
        GridLines(sb, plot);

        var xAxisY = plot.Window.YMin <= 0m &&
                     plot.Window.YMax >= 0m
            ? plot.Y(0m)
            : Bottom;

        var yAxisX = plot.Window.XMin <= 0m &&
                     plot.Window.XMax >= 0m
            ? plot.X(0m)
            : Left;

        sb.Append(
            $"<line x1='{F(Left)}' y1='{F(xAxisY)}' " +
            $"x2='{F(Right)}' y2='{F(xAxisY)}' class='amv-axis'/>");
        sb.Append(
            $"<line x1='{F(yAxisX)}' y1='{F(Top)}' " +
            $"x2='{F(yAxisX)}' y2='{F(Bottom)}' class='amv-axis'/>");

        Label(sb, Right - 4d, xAxisY - 10d, xLabel, "amv-axis-label");
        Label(sb, yAxisX + 16d, Top + 12d, yLabel, "amv-axis-label");
    }

    private static void GridLines(
        StringBuilder sb,
        PlotContext plot)
    {
        foreach (var x in TickValues(
                     plot.Window.XMin,
                     plot.Window.XMax,
                     plot.Window.XTick))
        {
            var px = plot.X(x);
            sb.Append(
                $"<line x1='{F(px)}' y1='{F(Top)}' " +
                $"x2='{F(px)}' y2='{F(Bottom)}' class='amv-grid'/>");
        }

        foreach (var y in TickValues(
                     plot.Window.YMin,
                     plot.Window.YMax,
                     plot.Window.YTick))
        {
            var py = plot.Y(y);
            sb.Append(
                $"<line x1='{F(Left)}' y1='{F(py)}' " +
                $"x2='{F(Right)}' y2='{F(py)}' class='amv-grid'/>");
        }
    }

    private static IEnumerable<decimal> TickValues(
        decimal min,
        decimal max,
        decimal step)
    {
        var start = Math.Ceiling(min / step) * step;
        var maximumTicks = 80;

        for (var current = start;
             current <= max && maximumTicks-- > 0;
             current += step)
        {
            yield return current;
        }
    }

    private static void Curve(
        StringBuilder sb,
        PlotContext plot,
        MathematicalCurveSpec curve,
        int seriesIndex)
    {
        var points = SampleCurve(
            curve,
            plot.Window.XMin,
            plot.Window.XMax,
            CurveSamples);

        var segments = SplitFiniteSegments(
            points,
            plot.Window);

        foreach (var segment in segments)
        {
            if (segment.Count < 2)
                continue;

            var path = new StringBuilder();
            for (var i = 0; i < segment.Count; i++)
            {
                var point = segment[i];
                path.Append(i == 0 ? "M " : " L ");
                path.Append(F(plot.X(point.X)));
                path.Append(' ');
                path.Append(F(plot.Y(point.Y)));
            }

            sb.Append(
                $"<path d='{path}' class='amv-curve amv-series-{seriesIndex % 4}'/>");
        }

        if (!string.IsNullOrWhiteSpace(curve.Label))
        {
            Label(
                sb,
                Right - 82d,
                Top + 24d + seriesIndex * 22d,
                curve.Label!,
                $"amv-legend amv-series-{seriesIndex % 4}-text");
        }
    }

    private static List<SampledPoint> SampleCurve(
        MathematicalCurveSpec curve,
        decimal xFrom,
        decimal xTo,
        int samples)
    {
        var result = new List<SampledPoint>(samples + 1);
        var span = xTo - xFrom;

        for (var i = 0; i <= samples; i++)
        {
            var ratio = i / (decimal)samples;
            var x = xFrom + span * ratio;
            var y = Evaluate(curve, x);
            result.Add(new SampledPoint(x, y));
        }

        return result;
    }

    private static double Evaluate(
        MathematicalCurveSpec curve,
        decimal xValue)
    {
        var x = (double)xValue;

        return curve.Kind switch
        {
            MathematicalCurveKind.Polynomial =>
                EvaluatePolynomial(curve.Parameters, x),

            MathematicalCurveKind.Sine =>
                Trig(curve.Parameters, x, Math.Sin),

            MathematicalCurveKind.Cosine =>
                Trig(curve.Parameters, x, Math.Cos),

            MathematicalCurveKind.Tangent =>
                Trig(curve.Parameters, x, Math.Tan),

            _ => double.NaN
        };
    }

    private static double EvaluatePolynomial(
        IReadOnlyList<decimal> coefficients,
        double x)
    {
        var result = 0d;
        for (var i = coefficients.Count - 1; i >= 0; i--)
            result = result * x + (double)coefficients[i];
        return result;
    }

    private static double Trig(
        IReadOnlyList<decimal> parameters,
        double x,
        Func<double, double> operation)
    {
        var amplitude = (double)parameters[0];
        var angularFrequency = (double)parameters[1];
        var phase = (double)parameters[2];
        var verticalShift = (double)parameters[3];
        var inner = angularFrequency * x + phase;
        var value = operation(inner);

        if (!double.IsFinite(value) ||
            Math.Abs(value) > 1_000_000d)
        {
            return double.NaN;
        }

        return amplitude * value + verticalShift;
    }

    private static IReadOnlyList<List<SampledPoint>> SplitFiniteSegments(
        IReadOnlyList<SampledPoint> points,
        MathematicalAxisWindow window)
    {
        var result = new List<List<SampledPoint>>();
        var current = new List<SampledPoint>();
        var ySpan = (double)(window.YMax - window.YMin);
        var threshold = Math.Max(1d, ySpan * 3d);

        foreach (var point in points)
        {
            if (!IsFinite(point.Y) ||
                point.Y < (double)window.YMin - threshold ||
                point.Y > (double)window.YMax + threshold)
            {
                if (current.Count > 1)
                    result.Add(current);
                current = [];
                continue;
            }

            if (current.Count > 0 &&
                Math.Abs(current[^1].Y - point.Y) > threshold)
            {
                if (current.Count > 1)
                    result.Add(current);
                current = [];
            }

            current.Add(point);
        }

        if (current.Count > 1)
            result.Add(current);

        return result;
    }

    private static void Segment(
        StringBuilder sb,
        PlotContext plot,
        MathematicalSegment segment)
    {
        sb.Append(
            $"<line x1='{F(plot.X(segment.Start.X))}' " +
            $"y1='{F(plot.Y(segment.Start.Y))}' " +
            $"x2='{F(plot.X(segment.End.X))}' " +
            $"y2='{F(plot.Y(segment.End.Y))}' " +
            $"class='{(segment.Dashed ? "amv-segment amv-dashed" : "amv-segment")}'/>");

        if (!string.IsNullOrWhiteSpace(segment.Label))
        {
            Label(
                sb,
                (plot.X(segment.Start.X) + plot.X(segment.End.X)) / 2d,
                (plot.Y(segment.Start.Y) + plot.Y(segment.End.Y)) / 2d - 8d,
                segment.Label!);
        }
    }

    private static void Point(
        StringBuilder sb,
        PlotContext plot,
        MathematicalPoint point)
    {
        var x = plot.X(point.X);
        var y = plot.Y(point.Y);

        sb.Append(
            $"<circle cx='{F(x)}' cy='{F(y)}' r='5' class='amv-point'/>");

        if (!string.IsNullOrWhiteSpace(point.Label))
            Label(sb, x + 10d, y - 10d, point.Label!);
    }

    private static MathematicalAxisWindow BoundsFor(
        IReadOnlyList<MathematicalPoint> points)
    {
        var minX = points.Min(x => x.X);
        var maxX = points.Max(x => x.X);
        var minY = points.Min(x => x.Y);
        var maxY = points.Max(x => x.Y);

        if (minX == maxX)
        {
            minX -= 1m;
            maxX += 1m;
        }

        if (minY == maxY)
        {
            minY -= 1m;
            maxY += 1m;
        }

        var xPadding = Math.Max(1m, (maxX - minX) * 0.15m);
        var yPadding = Math.Max(1m, (maxY - minY) * 0.15m);

        return new MathematicalAxisWindow(
            minX - xPadding,
            maxX + xPadding,
            minY - yPadding,
            maxY + yPadding,
            Math.Max(1m, DecimalTick(maxX - minX)),
            Math.Max(1m, DecimalTick(maxY - minY)));
    }

    private static decimal DecimalTick(decimal span)
    {
        if (span <= 10m) return 1m;
        if (span <= 25m) return 2m;
        if (span <= 60m) return 5m;
        if (span <= 120m) return 10m;
        return Math.Ceiling(span / 12m);
    }

    private static StringBuilder Start(
        MathematicalVisualAccessibility accessibility,
        bool includeArrowMarker = false)
    {
        var title = Encode(accessibility.Title);
        var description = Encode(accessibility.Description);
        var sb = new StringBuilder(8_192);

        sb.Append(
            $"<svg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 {Width} {Height}' " +
            "role='img' aria-labelledby='amv-title amv-desc'>");
        sb.Append($"<title id='amv-title'>{title}</title>");
        sb.Append($"<desc id='amv-desc'>{description}</desc>");
        sb.Append(
            "<style>" +
            ".amv-bg{fill:#fff}.amv-grid{stroke:#d7e0e7;stroke-width:1}" +
            ".amv-axis{stroke:#173c47;stroke-width:2}.amv-axis-label,.amv-label{fill:#173c47;font:600 14px system-ui,sans-serif}" +
            ".amv-segment{stroke:#173c47;stroke-width:3;fill:none}.amv-dashed{stroke-dasharray:8 6}" +
            ".amv-point{fill:#173c47;stroke:#fff;stroke-width:2}.amv-guide{stroke:#7a8c96;stroke-width:1.5;stroke-dasharray:5 5}" +
            ".amv-vector{stroke:#173c47;stroke-width:3}.amv-bracket{stroke:#173c47;stroke-width:3;fill:none}" +
            ".amv-matrix-value{fill:#173c47;font:700 17px ui-monospace,monospace}" +
            ".amv-polygon{fill:#eef4f7;stroke:#173c47;stroke-width:2}" +
            ".amv-curve{fill:none;stroke-width:3;stroke-linecap:round;stroke-linejoin:round}" +
            ".amv-series-0{stroke:#173c47}.amv-series-1{stroke:#6f4a8e}.amv-series-2{stroke:#946200}.amv-series-3{stroke:#2d6a55}" +
            ".amv-series-0-text{fill:#173c47}.amv-series-1-text{fill:#6f4a8e}.amv-series-2-text{fill:#946200}.amv-series-3-text{fill:#2d6a55}" +
            ".amv-legend{font:700 14px system-ui,sans-serif}.amv-region{fill:#dce9ef;fill-opacity:.75;stroke:none}" +
            ".amv-series{fill:none;stroke:#173c47;stroke-width:2.5}.amv-bar{fill:#dce9ef;stroke:#173c47;stroke-width:2}" +
            "</style>");
        sb.Append($"<rect x='0' y='0' width='{Width}' height='{Height}' rx='18' class='amv-bg'/>");

        if (includeArrowMarker)
        {
            sb.Append(
                "<defs><marker id='amv-arrow' markerWidth='10' markerHeight='10' refX='8' refY='3' orient='auto' markerUnits='strokeWidth'>" +
                "<path d='M0,0 L0,6 L9,3 z' fill='#173c47'/></marker></defs>");
        }

        return sb;
    }

    private static string End(StringBuilder sb)
    {
        sb.Append("</svg>");
        return sb.ToString();
    }

    private static void Label(
        StringBuilder sb,
        double x,
        double y,
        string value,
        string cssClass = "amv-label")
    {
        sb.Append(
            $"<text x='{F(x)}' y='{F(y)}' text-anchor='middle' class='{cssClass}'>" +
            $"{Encode(value)}</text>");
    }

    private static string Encode(string value) =>
        WebUtility.HtmlEncode(value);

    private static string F(double value) =>
        value.ToString("0.###", CultureInfo.InvariantCulture);

    private static bool IsFinite(double value) =>
        !double.IsNaN(value) &&
        !double.IsInfinity(value);

    private readonly record struct SampledPoint(
        decimal X,
        double Y);

    private sealed class PlotContext(
        MathematicalAxisWindow window)
    {
        public MathematicalAxisWindow Window { get; } = window;

        public double X(decimal x) =>
            Left +
            ((double)(x - Window.XMin) /
             (double)(Window.XMax - Window.XMin)) *
            (Right - Left);

        public double Y(decimal y) =>
            Bottom -
            ((double)(y - Window.YMin) /
             (double)(Window.YMax - Window.YMin)) *
            (Bottom - Top);

        public double Y(double y) =>
            Bottom -
            ((y - (double)Window.YMin) /
             (double)(Window.YMax - Window.YMin)) *
            (Bottom - Top);
    }
}
