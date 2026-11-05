using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Small exact extensions for IM Algebra 1 Unit 1. Only objective components
/// are automated; source graph construction, contextual interpretation and
/// discussion remain teacher-reviewed.
/// </summary>
public static class UsCcssAlg1Unit1StatisticsSpreadsheetQuestions
{
    public const string DistributionShapeFamily =
        "statistics.distribution_shape.classify";
    public const string IqrFamily = "statistics.center_spread.iqr";
    public const string MadFamily = "statistics.center_spread.mad";

    public sealed record Problem(
        string Family,
        string Prompt,
        string Solution,
        AssessmentItemType ItemType,
        IReadOnlyDictionary<string, int> Parameters);

    public static bool Supports(string? family) =>
        family is DistributionShapeFamily or IqrFamily or MadFamily;

    public static Problem Build(string family, Random random)
    {
        if (!Supports(family))
            throw new InvalidOperationException("Unsupported ALG1 Unit 1 extension family.");
        ArgumentNullException.ThrowIfNull(random);

        if (family == DistributionShapeFamily)
            return BuildDistributionShape(random);
        if (family == IqrFamily)
            return BuildIqr(random);
        return BuildMad(random);
    }

    public static string Solve(
        string family,
        IReadOnlyDictionary<string, int> parameters)
    {
        if (family == DistributionShapeFamily)
        {
            if (!ValidateShape(parameters, out var shape, out _, out _, out _))
                throw new InvalidOperationException("Invalid distribution-shape parameters.");
            return shape switch
            {
                0 => "uniform",
                1 => "symmetric bell-shaped",
                2 => "right-skewed",
                3 => "left-skewed",
                4 => "bimodal",
                _ => throw new InvalidOperationException("Unsupported distribution shape.")
            };
        }

        if (family == IqrFamily)
        {
            if (!ValidateStep(parameters, out _, out var step, out _))
                throw new InvalidOperationException("Invalid IQR parameters.");
            return (6m * step).ToString(CultureInfo.InvariantCulture);
        }

        if (family == MadFamily)
        {
            if (!ValidateStep(parameters, out _, out var step, out _))
                throw new InvalidOperationException("Invalid MAD parameters.");
            return (6m * step / 5m).ToString(CultureInfo.InvariantCulture);
        }

        throw new InvalidOperationException("Unsupported ALG1 Unit 1 extension family.");
    }

    public static bool Verify(
        string family,
        IReadOnlyDictionary<string, int> parameters,
        string? answer)
    {
        if (!Supports(family) || string.IsNullOrWhiteSpace(answer))
            return false;

        if (family == DistributionShapeFamily)
            return ValidateShape(parameters, out _, out _, out _, out _) &&
                NormalizeLabel(answer) == NormalizeLabel(Solve(family, parameters));

        return ValidateStep(parameters, out _, out _, out _) &&
            TryExactDecimal(answer, out var submitted) &&
            decimal.TryParse(
                Solve(family, parameters),
                NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture,
                out var expected) &&
            submitted == expected;
    }

    private static Problem BuildDistributionShape(Random random)
    {
        var shape = random.Next(0, 5);
        var shift = random.Next(0, 7);
        var scale = random.Next(1, 4);
        var variant = random.Next(0, 16);
        var frequencies = Frequencies(shape, scale);
        var values = Enumerable.Range(1 + shift, 5).ToArray();

        var prompt =
            $"A synthetic numerical distribution has values {string.Join(", ", values)} " +
            $"with frequencies {string.Join(", ", frequencies)} in the same order. " +
            "Classify its overall shape as uniform, symmetric bell-shaped, right-skewed, " +
            "left-skewed, or bimodal. Enter only the classification. " +
            "Suggesting a real-world context and critiquing a graph remain teacher-reviewed.";

        return new(
            DistributionShapeFamily,
            prompt,
            "Compare mirrored frequencies, peaks and the direction of any sparse tail.",
            AssessmentItemType.ShortAnswer,
            new Dictionary<string, int>(StringComparer.Ordinal)
            {
                ["shape"] = shape,
                ["shift"] = shift,
                ["scale"] = scale,
                ["variant"] = variant
            });
    }

    private static Problem BuildIqr(Random random)
    {
        var @base = random.Next(0, 31);
        var step = random.Next(1, 7);
        var variant = random.Next(0, 16);
        var values = IqrValues(@base, step);
        return new(
            IqrFamily,
            $"The ordered synthetic data are {string.Join(", ", values)}. " +
            "Using the median-of-halves convention, calculate the interquartile range (IQR). Enter one exact number.",
            "Find Q1 from the lower four values and Q3 from the upper four values, then compute Q3 - Q1.",
            AssessmentItemType.Numeric,
            new Dictionary<string, int>(StringComparer.Ordinal)
            {
                ["base"] = @base, ["step"] = step, ["variant"] = variant
            });
    }

    private static Problem BuildMad(Random random)
    {
        var center = random.Next(10, 51);
        var step = random.Next(1, 8);
        var variant = random.Next(0, 16);
        var values = MadValues(center, step);
        return new(
            MadFamily,
            $"The synthetic data are {string.Join(", ", values)}. " +
            "Calculate the mean absolute deviation (MAD) from the mean. Enter an exact decimal or fraction.",
            "Find the mean, compute every absolute distance from it, then average those distances.",
            AssessmentItemType.Numeric,
            new Dictionary<string, int>(StringComparer.Ordinal)
            {
                ["base"] = center, ["step"] = step, ["variant"] = variant
            });
    }

    private static int[] Frequencies(int shape, int scale) => shape switch
    {
        0 => [2 * scale, 2 * scale, 2 * scale, 2 * scale, 2 * scale],
        1 => [1 * scale, 2 * scale, 4 * scale, 2 * scale, 1 * scale],
        2 => [5 * scale, 3 * scale, 2 * scale, 1 * scale, 1 * scale],
        3 => [1 * scale, 1 * scale, 2 * scale, 3 * scale, 5 * scale],
        4 => [4 * scale, 1 * scale, 1 * scale, 1 * scale, 4 * scale],
        _ => throw new InvalidOperationException("Unsupported distribution shape.")
    };

    private static int[] IqrValues(int @base, int step) =>
        [@base, @base + step, @base + 2 * step, @base + 3 * step,
         @base + 5 * step, @base + 7 * step, @base + 8 * step, @base + 10 * step];

    private static int[] MadValues(int center, int step) =>
        [center - 2 * step, center - step, center, center + step, center + 2 * step];

    private static bool ValidateShape(
        IReadOnlyDictionary<string, int>? parameters,
        out int shape,
        out int shift,
        out int scale,
        out int variant)
    {
        shape = shift = scale = variant = -1;
        return parameters is not null &&
            parameters.TryGetValue("shape", out shape) && shape is >= 0 and <= 4 &&
            parameters.TryGetValue("shift", out shift) && shift is >= 0 and <= 6 &&
            parameters.TryGetValue("scale", out scale) && scale is >= 1 and <= 3 &&
            parameters.TryGetValue("variant", out variant) && variant is >= 0 and <= 15;
    }

    private static bool ValidateStep(
        IReadOnlyDictionary<string, int>? parameters,
        out int @base,
        out int step,
        out int variant)
    {
        @base = step = variant = -1;
        return parameters is not null &&
            parameters.TryGetValue("base", out @base) &&
            parameters.TryGetValue("step", out step) && step is >= 1 and <= 7 &&
            parameters.TryGetValue("variant", out variant) && variant is >= 0 and <= 15;
    }

    private static string NormalizeLabel(string value)
    {
        var text = value.Trim().ToLowerInvariant()
            .Replace('_', ' ')
            .Replace('-', ' ');
        text = string.Join(' ', text.Split(
            (char[]?)null,
            StringSplitOptions.RemoveEmptyEntries));

        return text switch
        {
            "bell shaped" or "symmetric bell shaped" or "bell shaped symmetric" =>
                "symmetric bell shaped",
            "skewed right" or "right skewed" => "right skewed",
            "skewed left" or "left skewed" => "left skewed",
            _ => text
        };
    }

    private static bool TryExactDecimal(string? raw, out decimal result)
    {
        result = 0;
        if (string.IsNullOrWhiteSpace(raw) || raw.Length > 32)
            return false;

        var text = raw.Trim();
        const NumberStyles styles =
            NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint;
        if (decimal.TryParse(text, styles, CultureInfo.InvariantCulture, out result))
            return true;

        var fraction = text.Split('/', StringSplitOptions.None);
        if (fraction.Length != 2 ||
            !decimal.TryParse(fraction[0], styles, CultureInfo.InvariantCulture, out var top) ||
            !decimal.TryParse(fraction[1], styles, CultureInfo.InvariantCulture, out var bottom) ||
            bottom == 0)
            return false;

        try
        {
            result = top / bottom;
            return true;
        }
        catch (OverflowException)
        {
            return false;
        }
    }
}
