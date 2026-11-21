using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Edulytics.Core.Curriculum;

/// <summary>
/// Parses simple first-party Arabic slope-intercept coordinate descriptions
/// for an accessible diagram. Nonmatching prose is left as prose, never
/// guessed into mathematical lines.
/// </summary>
public static class RichLessonCoordinateDiagram
{
    public sealed record Line(double Slope, double Intercept, string Relation)
    {
        public bool IsDashed => Relation is "<" or ">";
        public bool ShadeBelow => Relation is "<" or "\u2264";
        public bool ShadeAbove => Relation is ">" or "\u2265";
    }

    public sealed record Point(double X, double Y, string Label);

    private static readonly Regex EquationPattern = new(
        @"(?:\u0635|y)\s*(?<relation>[=<>\u2264\u2265])\s*(?<coefficient>[+-]?\d*(?:\.\d+)?)\s*(?:\u0633|x)\s*(?<intercept>[+-]\s*\d+(?:\.\d+)?)?",
        RegexOptions.Compiled | RegexOptions.CultureInvariant,
        TimeSpan.FromMilliseconds(200));

    private static readonly Regex PointPattern = new(
        @"\(\s*(?<x>[+-]?\d+(?:\.\d+)?)\s*,\s*(?<y>[+-]?\d+(?:\.\d+)?)\s*\)",
        RegexOptions.Compiled | RegexOptions.CultureInvariant,
        TimeSpan.FromMilliseconds(200));

    public static IReadOnlyList<Line> ParseLines(string? description)
    {
        var normalized = Normalize(description);
        var lines = new List<Line>();
        foreach (Match match in EquationPattern.Matches(normalized))
        {
            var coefficient = match.Groups["coefficient"].Value;
            var signedCoefficient = coefficient switch
            {
                "" or "+" => "1",
                "-" => "-1",
                _ => coefficient
            };
            var bias = match.Groups["intercept"].Value.Replace(" ", "", StringComparison.Ordinal);
            if (!double.TryParse(signedCoefficient, NumberStyles.Float, CultureInfo.InvariantCulture, out var slope) ||
                (bias.Length > 0 &&
                 !double.TryParse(bias, NumberStyles.Float, CultureInfo.InvariantCulture, out _)))
                continue;
            var intercept = bias.Length == 0 ? 0d : double.Parse(bias, CultureInfo.InvariantCulture);

            // Graph is restricted to the signed [-10,10] educational viewport;
            // unbounded or nonfinite expressions remain available as text only.
            if (!double.IsFinite(slope) || !double.IsFinite(intercept) ||
                Math.Abs(slope) > 100 || Math.Abs(intercept) > 100)
                continue;
            lines.Add(new Line(slope, intercept, match.Groups["relation"].Value));
            if (lines.Count == 4)
                break;
        }
        return lines;
    }

    public static IReadOnlyList<Point> ParsePoints(IEnumerable<string> items)
    {
        var points = new List<Point>();
        foreach (var item in items)
        {
            var match = PointPattern.Match(Normalize(item));
            if (!match.Success ||
                !double.TryParse(match.Groups["x"].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out var x) ||
                !double.TryParse(match.Groups["y"].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out var y) ||
                !double.IsFinite(x) || !double.IsFinite(y) ||
                Math.Abs(x) > 10 || Math.Abs(y) > 10)
                continue;
            points.Add(new Point(x, y, item));
        }
        return points;
    }

    private static string Normalize(string? text)
    {
        if (string.IsNullOrEmpty(text))
            return string.Empty;
        var result = new StringBuilder(text.Length);
        foreach (var character in text)
        {
            var digit = character is >= '\u0660' and <= '\u0669'
                ? character - '\u0660'
                : character is >= '\u06f0' and <= '\u06f9'
                    ? character - '\u06f0'
                    : -1;
            result.Append(digit >= 0 ? (char)('0' + digit) : character switch
            {
                '\u2212' or '\u2013' => '-',
                '\u066b' => '.',
                '\u060c' => ',',
                _ => character
            });
        }
        return result.ToString();
    }
}
