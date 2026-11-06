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
    public const string SpreadsheetFormulaFamily =
        "spreadsheet.cell_reference.evaluate";
    public const string SpreadsheetRecursiveFillFamily =
        "spreadsheet.recursive_fill.evaluate";
    public const string HistogramBinFamily =
        "statistics.histogram.half_open_bin_count";
    public const string ExtremeMeanReplacementFamily =
        "statistics.extreme_value.mean_after_replacement";
    public const string PopulationSdFamily =
        "statistics.center_spread.population_sd";
    public const string OutlierFenceFamily =
        "statistics.outliers.iqr_fence";
    public const string OutlierFlagFamily =
        "statistics.outliers.classify_iqr_flag";

    public sealed record Problem(
        string Family,
        string Prompt,
        string Solution,
        AssessmentItemType ItemType,
        IReadOnlyDictionary<string, int> Parameters);

    public static bool Supports(string? family) =>
        family is DistributionShapeFamily or IqrFamily or MadFamily or
            SpreadsheetFormulaFamily or SpreadsheetRecursiveFillFamily or
            HistogramBinFamily or ExtremeMeanReplacementFamily or
            PopulationSdFamily or OutlierFenceFamily or OutlierFlagFamily;

    public static Problem Build(string family, Random random)
    {
        if (!Supports(family))
            throw new InvalidOperationException("Unsupported ALG1 Unit 1 extension family.");
        ArgumentNullException.ThrowIfNull(random);

        if (family == DistributionShapeFamily)
            return BuildDistributionShape(random);
        if (family == IqrFamily)
            return BuildIqr(random);
        if (family == MadFamily)
            return BuildMad(random);
        if (family == SpreadsheetFormulaFamily)
            return BuildSpreadsheetFormula(random);
        if (family == SpreadsheetRecursiveFillFamily)
            return BuildSpreadsheetRecursiveFill(random);
        if (family == HistogramBinFamily)
            return BuildHistogramBin(random);
        if (family == ExtremeMeanReplacementFamily)
            return BuildExtremeMeanReplacement(random);
        if (family == PopulationSdFamily)
            return BuildPopulationSd(random);
        return BuildOutlierQuestion(family, random);
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

        if (family == SpreadsheetFormulaFamily)
        {
            if (!ValidateSpreadsheet(parameters, out var a, out var b, out var c, out var op, out _))
                throw new InvalidOperationException("Invalid spreadsheet parameters.");
            var value = op switch
            {
                0 => a + 2 * b,
                1 => a * b - c,
                2 => (a + b) * c,
                3 => a * a + b,
                _ => throw new InvalidOperationException("Unsupported spreadsheet operation.")
            };
            return value.ToString(CultureInfo.InvariantCulture);
        }

        if (family == SpreadsheetRecursiveFillFamily)
        {
            if (!ValidateRecursiveFill(parameters, out var start, out var step, out var row, out _))
                throw new InvalidOperationException("Invalid recursive-fill parameters.");
            return (start + (row - 1) * step).ToString(CultureInfo.InvariantCulture);
        }

        if (family == HistogramBinFamily)
        {
            if (!ValidateHistogram(parameters, out _, out _, out var c0, out var c1, out var c2, out var target, out _))
                throw new InvalidOperationException("Invalid histogram parameters.");
            var count = target switch { 0 => c0, 1 => c1, 2 => c2, _ => -1 };
            return count.ToString(CultureInfo.InvariantCulture);
        }

        if (family == ExtremeMeanReplacementFamily)
        {
            if (!ValidateExtreme(parameters, out var center, out var delta, out _))
                throw new InvalidOperationException("Invalid extreme-value parameters.");
            var newMean = center + 0.5m + delta / 10m;
            return newMean.ToString(CultureInfo.InvariantCulture);
        }

        if (family == PopulationSdFamily)
        {
            if (!ValidatePopulationSd(parameters, out _, out var spread, out _))
                throw new InvalidOperationException("Invalid population-SD parameters.");
            return spread.ToString(CultureInfo.InvariantCulture);
        }

        if (family is OutlierFenceFamily or OutlierFlagFamily)
        {
            if (!ValidateOutlier(parameters, out var center, out var delta, out var side, out _))
                throw new InvalidOperationException("Invalid outlier parameters.");
            var lowerFence = center - 5.5m;
            var upperFence = center + 6.5m;
            if (family == OutlierFlagFamily)
                return center + delta > upperFence ? "yes" : "no";
            return (side == 0 ? lowerFence : upperFence)
                .ToString(CultureInfo.InvariantCulture);
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

        if (family == OutlierFlagFamily)
            return ValidateOutlier(parameters, out _, out _, out _, out _) &&
                NormalizeYesNo(answer) == Solve(family, parameters);

        var parametersValid = family switch
        {
            SpreadsheetFormulaFamily =>
                ValidateSpreadsheet(parameters, out _, out _, out _, out _, out _),
            SpreadsheetRecursiveFillFamily =>
                ValidateRecursiveFill(parameters, out _, out _, out _, out _),
            HistogramBinFamily =>
                ValidateHistogram(parameters, out _, out _, out _, out _, out _, out _, out _),
            ExtremeMeanReplacementFamily =>
                ValidateExtreme(parameters, out _, out _, out _),
            PopulationSdFamily =>
                ValidatePopulationSd(parameters, out _, out _, out _),
            OutlierFenceFamily =>
                ValidateOutlier(parameters, out _, out _, out _, out _),
            _ => ValidateStep(parameters, out _, out _, out _)
        };

        return parametersValid &&
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

    private static Problem BuildSpreadsheetFormula(Random random)
    {
        var a = random.Next(-6, 16);
        var b = random.Next(-6, 16);
        var c = random.Next(1, 10);
        var op = random.Next(0, 4);
        var variant = random.Next(0, 16);
        var formula = op switch
        {
            0 => "=A2+2*A3",
            1 => "=A2*A3-A4",
            2 => "=(A2+A3)*A4",
            3 => "=A2^2+A3",
            _ => throw new InvalidOperationException("Unsupported spreadsheet operation.")
        };

        return new(
            SpreadsheetFormulaFamily,
            $"A spreadsheet has A2={a}, A3={b}, A4={c}. " +
            $"Cell B2 contains {formula}. What numerical value should B2 display? " +
            "Enter one number. Entering/testing the formula in spreadsheet software remains teacher-reviewed.",
            "Substitute the current cell values into the formula and apply ordinary order of operations.",
            AssessmentItemType.Numeric,
            new Dictionary<string, int>(StringComparer.Ordinal)
            {
                ["a"] = a, ["b"] = b, ["c"] = c, ["op"] = op, ["variant"] = variant
            });
    }

    private static Problem BuildSpreadsheetRecursiveFill(Random random)
    {
        var start = random.Next(5, 41);
        var step = random.Next(-8, 16);
        if (step == 0)
            step = 6;
        var row = random.Next(3, 11);
        var variant = random.Next(0, 16);

        return new(
            SpreadsheetRecursiveFillFamily,
            $"Cell B1 contains {start}. Cell B2 contains =B1{(step >= 0 ? "+" : "")}{step} " +
            $"and that relative-reference formula is filled down through B{row}. " +
            $"What value should appear in B{row}? Enter one number. " +
            "Performing the drag/fill action in spreadsheet software remains teacher-reviewed.",
            "Each copied row references the immediately previous cell, so apply the same change once per row.",
            AssessmentItemType.Numeric,
            new Dictionary<string, int>(StringComparer.Ordinal)
            {
                ["start"] = start, ["step"] = step, ["row"] = row, ["variant"] = variant
            });
    }

    private static Problem BuildHistogramBin(Random random)
    {
        var start = random.Next(0, 21);
        var width = random.Next(2, 5);
        var c0 = random.Next(1, 5);
        var c1 = random.Next(1, 5);
        var c2 = random.Next(1, 5);
        var target = random.Next(0, 3);
        var variant = random.Next(0, 16);
        var values = new List<int>();
        AddBin(values, start, width, 0, c0);
        AddBin(values, start, width, 1, c1);
        AddBin(values, start, width, 2, c2);
        values.Sort();

        var left = start + target * width;
        var right = left + width;
        return new(
            HistogramBinFamily,
            $"The synthetic data are {string.Join(", ", values)}. " +
            $"Using consecutive histogram bins of width {width} starting at {start}, " +
            $"how many observations belong to [{left}, {right})? " +
            "The left endpoint is included and the right endpoint is excluded. Enter one number.",
            "Count exactly the observations with left <= value < right.",
            AssessmentItemType.Numeric,
            new Dictionary<string, int>(StringComparer.Ordinal)
            {
                ["start"] = start, ["width"] = width, ["c0"] = c0, ["c1"] = c1,
                ["c2"] = c2, ["target"] = target, ["variant"] = variant
            });
    }

    private static void AddBin(List<int> values, int start, int width, int bin, int count)
    {
        var left = start + bin * width;
        for (var i = 0; i < count; i++)
            values.Add(left + (i % width));
    }

    private static Problem BuildExtremeMeanReplacement(Random random)
    {
        var center = random.Next(4, 31);
        var delta = random.Next(10, 61);
        var variant = random.Next(0, 16);
        var before = new[]
        {
            center - 2, center - 1, center - 1, center, center,
            center + 1, center + 1, center + 2, center + 2, center + 3
        };
        var after = before.ToArray();
        after[^1] += delta;

        return new(
            ExtremeMeanReplacementFamily,
            $"The ordered data are {string.Join(", ", before)}. " +
            $"Only the largest value is replaced, giving {string.Join(", ", after)}. " +
            "What is the new mean? Enter an exact decimal or fraction. " +
            "Explaining why the median is more resistant to this extreme change remains teacher-reviewed.",
            "Keep the other nine observations fixed, update the sum by the replacement amount, and divide by 10.",
            AssessmentItemType.Numeric,
            new Dictionary<string, int>(StringComparer.Ordinal)
            {
                ["center"] = center, ["delta"] = delta, ["variant"] = variant
            });
    }

    private static Problem BuildPopulationSd(Random random)
    {
        var center = random.Next(5, 41);
        var spread = random.Next(1, 10);
        var variant = random.Next(0, 16);
        var values = new[]
        {
            center - spread, center - spread,
            center + spread, center + spread
        };

        return new(
            PopulationSdFamily,
            $"The synthetic population data are {string.Join(", ", values)}. " +
            "Calculate the population standard deviation σ, dividing the squared-deviation sum by n (not n-1). Enter one exact number.",
            "Find the population mean, average the squared deviations using n, then take the square root.",
            AssessmentItemType.Numeric,
            new Dictionary<string, int>(StringComparer.Ordinal)
            {
                ["center"] = center, ["spread"] = spread, ["variant"] = variant
            });
    }

    private static Problem BuildOutlierQuestion(string family, Random random)
    {
        var center = random.Next(8, 31);
        var delta = random.Next(3, 16);
        var side = random.Next(0, 2);
        var variant = random.Next(0, 16);
        var values = new[]
        {
            center - 2, center - 1, center,
            center + 1, center + 2, center + delta
        };

        if (family == OutlierFlagFamily)
        {
            return new(
                family,
                $"The ordered synthetic data are {string.Join(", ", values)}. " +
                "Using the 1.5×IQR rule and the median-of-halves convention, " +
                "is the largest value flagged as an upper outlier? Enter yes or no. " +
                "Whether a flagged observation should be deleted remains teacher-reviewed.",
                "Find Q1, Q3 and IQR; compare the largest value with Q3 + 1.5×IQR.",
                AssessmentItemType.ShortAnswer,
                new Dictionary<string, int>(StringComparer.Ordinal)
                {
                    ["center"] = center, ["delta"] = delta, ["side"] = side, ["variant"] = variant
                });
        }

        return new(
            family,
            $"The ordered synthetic data are {string.Join(", ", values)}. " +
            $"Using the median-of-halves convention and the 1.5×IQR rule, find the {(side == 0 ? "lower" : "upper")} fence. " +
            "Enter an exact decimal or fraction.",
            "Compute Q1 and Q3, then IQR=Q3-Q1; lower=Q1-1.5IQR and upper=Q3+1.5IQR.",
            AssessmentItemType.Numeric,
            new Dictionary<string, int>(StringComparer.Ordinal)
            {
                ["center"] = center, ["delta"] = delta, ["side"] = side, ["variant"] = variant
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

    private static bool ValidateSpreadsheet(
        IReadOnlyDictionary<string, int>? parameters,
        out int a,
        out int b,
        out int c,
        out int op,
        out int variant)
    {
        a = b = c = op = variant = 0;
        return parameters is not null &&
            parameters.TryGetValue("a", out a) && a is >= -6 and <= 15 &&
            parameters.TryGetValue("b", out b) && b is >= -6 and <= 15 &&
            parameters.TryGetValue("c", out c) && c is >= 1 and <= 9 &&
            parameters.TryGetValue("op", out op) && op is >= 0 and <= 3 &&
            parameters.TryGetValue("variant", out variant) && variant is >= 0 and <= 15;
    }

    private static bool ValidateRecursiveFill(
        IReadOnlyDictionary<string, int>? parameters,
        out int start,
        out int step,
        out int row,
        out int variant)
    {
        start = step = row = variant = 0;
        return parameters is not null &&
            parameters.TryGetValue("start", out start) && start is >= 5 and <= 40 &&
            parameters.TryGetValue("step", out step) && step is >= -8 and <= 15 && step != 0 &&
            parameters.TryGetValue("row", out row) && row is >= 3 and <= 10 &&
            parameters.TryGetValue("variant", out variant) && variant is >= 0 and <= 15;
    }

    private static bool ValidateHistogram(
        IReadOnlyDictionary<string, int>? parameters,
        out int start,
        out int width,
        out int c0,
        out int c1,
        out int c2,
        out int target,
        out int variant)
    {
        start = width = c0 = c1 = c2 = target = variant = 0;
        return parameters is not null &&
            parameters.TryGetValue("start", out start) && start is >= 0 and <= 20 &&
            parameters.TryGetValue("width", out width) && width is >= 2 and <= 4 &&
            parameters.TryGetValue("c0", out c0) && c0 is >= 1 and <= 4 &&
            parameters.TryGetValue("c1", out c1) && c1 is >= 1 and <= 4 &&
            parameters.TryGetValue("c2", out c2) && c2 is >= 1 and <= 4 &&
            parameters.TryGetValue("target", out target) && target is >= 0 and <= 2 &&
            parameters.TryGetValue("variant", out variant) && variant is >= 0 and <= 15;
    }

    private static bool ValidateExtreme(
        IReadOnlyDictionary<string, int>? parameters,
        out int center,
        out int delta,
        out int variant)
    {
        center = delta = variant = 0;
        return parameters is not null &&
            parameters.TryGetValue("center", out center) && center is >= 4 and <= 30 &&
            parameters.TryGetValue("delta", out delta) && delta is >= 10 and <= 60 &&
            parameters.TryGetValue("variant", out variant) && variant is >= 0 and <= 15;
    }

    private static bool ValidatePopulationSd(
        IReadOnlyDictionary<string, int>? parameters,
        out int center,
        out int spread,
        out int variant)
    {
        center = spread = variant = 0;
        return parameters is not null &&
            parameters.TryGetValue("center", out center) && center is >= 5 and <= 40 &&
            parameters.TryGetValue("spread", out spread) && spread is >= 1 and <= 9 &&
            parameters.TryGetValue("variant", out variant) && variant is >= 0 and <= 15;
    }

    private static bool ValidateOutlier(
        IReadOnlyDictionary<string, int>? parameters,
        out int center,
        out int delta,
        out int side,
        out int variant)
    {
        center = delta = side = variant = 0;
        return parameters is not null &&
            parameters.TryGetValue("center", out center) && center is >= 8 and <= 30 &&
            parameters.TryGetValue("delta", out delta) && delta is >= 3 and <= 15 &&
            parameters.TryGetValue("side", out side) && side is >= 0 and <= 1 &&
            parameters.TryGetValue("variant", out variant) && variant is >= 0 and <= 15;
    }

    private static string NormalizeYesNo(string value)
    {
        var text = value.Trim().ToLowerInvariant();
        return text switch
        {
            "y" or "yes" or "true" => "yes",
            "n" or "no" or "false" => "no",
            _ => text
        };
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
