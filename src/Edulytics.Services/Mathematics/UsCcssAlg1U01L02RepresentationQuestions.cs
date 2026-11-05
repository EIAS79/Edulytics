using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// ALG1 IM-1E U01:L02: deterministic scalar readings of FIRST-PARTY
/// lesson graphs. This is a course-local adapter, not a replacement engine.
/// Graph construction, interpretation, source P1-P5 and Cool-down always
/// require human teacher review; no persisted teacher rubric is asserted.
/// </summary>
public static class UsCcssAlg1U01L02RepresentationQuestions
{
    public const string DotFamily = "usccss.alg1.u01.l02.dot_frequency";
    public const string HistogramFamily = "usccss.alg1.u01.l02.histogram_half_open_bin_count";
    public const string FiveNumberFamily = "usccss.alg1.u01.l02.five_number_component";

    private static readonly int[] TwelveTrips = [2, 3, 3, 4, 5, 5, 6, 7, 7, 8, 9, 10];
    private static readonly int[] EightIndependent = [3, 4, 4, 5, 7, 8, 9, 10];
    private static readonly string[] FiveLabels =
        ["minimum", "first quartile (lower-half median)", "median",
         "third quartile (upper-half median)", "maximum"];

    public sealed record Problem(
        string Family, string Prompt, string Solution,
        AssessmentItemType ItemType, IReadOnlyDictionary<string, int> Parameters);

    public static bool Supports(string? family) =>
        family is DotFamily or HistogramFamily or FiveNumberFamily;

    public static Problem Build(string family, Random random)
    {
        if (!Supports(family))
            throw new InvalidOperationException("Unsupported ALG1 U01:L02 family.");
        ArgumentNullException.ThrowIfNull(random);
        int dataChoice = random.Next(0, 2);
        int[] values = Values(dataChoice);
        int count = family switch
        {
            DotFamily => values.Distinct().Count(),
            HistogramFamily => (values[^1] - values[0]) / 2 + 1,
            _ => 5
        };
        int selection = random.Next(0, count);
        var parameters = new Dictionary<string, int>(StringComparer.Ordinal)
        {
            ["dataset"] = dataChoice,
            ["selection"] = selection
        };

        string display = string.Join(", ", values);
        string prompt = family switch
        {
            DotFamily =>
                $"The first-party numerical dataset is {display} minutes. " +
                $"For the DOT PLOT, how many observations occur at exactly {values.Distinct().OrderBy(v => v).ElementAt(selection)} minutes? " +
                "Enter one number. Drawing the dot plot and interpreting the distribution require teacher review.",
            HistogramFamily =>
                $"The first-party dataset is {display} minutes. " +
                "Use consecutive width-2 histogram bins beginning at the minimum. " +
                $"How many values are in [{values[0] + 2 * selection}, {values[0] + 2 * (selection + 1)})? " +
                "The left endpoint is INCLUDED; the right endpoint is EXCLUDED. Enter one number. " +
                "Drawing and explaining the full histogram require teacher review.",
            _ =>
                $"The first-party dataset in ascending order is {display} minutes. " +
                $"Find the {FiveLabels[selection]}. Use the median of each half for quartiles. " +
                "Enter one number (a fraction or exact decimal is allowed). " +
                "Drawing and interpreting the box plot require teacher review."
        };
        var solution = family switch
        {
            DotFamily => "Count dots at exactly the named value; drawing and justification are teacher-reviewed.",
            HistogramFamily => "Count observations with left <= value < right; endpoints may not be double-counted. Drawing and interpretation require teacher review.",
            _ => "Find the ordered five-number summary using the medians of the lower and upper halves."
        };
        return new(family, prompt, solution, AssessmentItemType.Numeric, parameters);
    }

    public static string Solve(string family, IReadOnlyDictionary<string, int> parameters)
    {
        if (!Validate(family, parameters, out var dataset, out int selection))
            throw new InvalidOperationException("Invalid ALG1 L02 graph parameters.");
        int[] values = Values(dataset);
        decimal value = family switch
        {
            DotFamily => values.Count(v => v == values.Distinct().OrderBy(x => x).ElementAt(selection)),
            HistogramFamily =>
                values.Count(v => v >= values[0] + selection * 2 &&
                                  v < values[0] + (selection + 1) * 2),
            FiveNumberFamily => Summary(values)[selection],
            _ => throw new InvalidOperationException("Unapproved family.")
        };
        return value.ToString(CultureInfo.InvariantCulture);
    }

    public static bool Verify(string family, IReadOnlyDictionary<string, int> parameters, string? answer) =>
        Validate(family, parameters, out _, out _) &&
        TryExactDecimal(answer, out decimal submitted) &&
        decimal.TryParse(Solve(family, parameters), NumberStyles.AllowDecimalPoint,
            CultureInfo.InvariantCulture, out decimal expected) &&
        submitted == expected;

    private static int[] Values(int dataset) => dataset == 0 ? TwelveTrips : EightIndependent;

    private static bool Validate(
        string? family, IReadOnlyDictionary<string, int>? parameters,
        out int dataset, out int selection)
    {
        dataset = -1;
        selection = -1;
        if (!Supports(family) || parameters is not { Count: 2 } ||
            !parameters.TryGetValue("dataset", out dataset) || dataset is < 0 or > 1 ||
            !parameters.TryGetValue("selection", out selection) || selection < 0)
            return false;
        var values = Values(dataset);
        int limit = family switch
        {
            DotFamily => values.Distinct().Count(),
            HistogramFamily => (values[^1] - values[0]) / 2 + 1,
            _ => 5
        };
        return selection < limit;
    }

    private static decimal[] Summary(int[] values)
    {
        int n = values.Length;
        return [values[0], Median(values.Take(n / 2).ToArray()),
            Median(values), Median(values.Skip((n + 1) / 2).ToArray()), values[^1]];
    }

    private static decimal Median(int[] ordered)
    {
        int n = ordered.Length;
        return n % 2 == 1 ? ordered[n / 2]
            : (ordered[n / 2 - 1] + (decimal)ordered[n / 2]) / 2;
    }

    private static bool TryExactDecimal(string? raw, out decimal result)
    {
        result = 0;
        if (string.IsNullOrWhiteSpace(raw) || raw.Length > 32)
            return false;
        string text = raw.Trim();
        const NumberStyles styles = NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint;
        if (decimal.TryParse(text, styles, CultureInfo.InvariantCulture, out result))
            return true;
        string[] fraction = text.Split('/', StringSplitOptions.None);
        if (fraction.Length != 2 ||
            !decimal.TryParse(fraction[0], styles, CultureInfo.InvariantCulture, out decimal top) ||
            !decimal.TryParse(fraction[1], styles, CultureInfo.InvariantCulture, out decimal bottom) ||
            bottom == 0)
            return false;
        try { result = top / bottom; return true; }
        catch (OverflowException) { return false; }
    }
}
