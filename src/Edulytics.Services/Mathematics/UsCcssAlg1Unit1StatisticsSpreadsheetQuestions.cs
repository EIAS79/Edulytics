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

    public sealed record Problem(
        string Family,
        string Prompt,
        string Solution,
        AssessmentItemType ItemType,
        IReadOnlyDictionary<string, int> Parameters);

    public static bool Supports(string? family) =>
        string.Equals(family, DistributionShapeFamily, StringComparison.Ordinal);

    public static Problem Build(string family, Random random)
    {
        if (!Supports(family))
            throw new InvalidOperationException("Unsupported ALG1 Unit 1 extension family.");
        ArgumentNullException.ThrowIfNull(random);

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
            family,
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

    public static string Solve(
        string family,
        IReadOnlyDictionary<string, int> parameters)
    {
        if (!Validate(family, parameters, out var shape, out _, out _, out _))
            throw new InvalidOperationException("Invalid ALG1 Unit 1 extension parameters.");

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

    public static bool Verify(
        string family,
        IReadOnlyDictionary<string, int> parameters,
        string? answer)
    {
        if (!Validate(family, parameters, out _, out _, out _, out _) ||
            string.IsNullOrWhiteSpace(answer))
            return false;

        return Normalize(answer) == Normalize(Solve(family, parameters));
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

    private static bool Validate(
        string? family,
        IReadOnlyDictionary<string, int>? parameters,
        out int shape,
        out int shift,
        out int scale,
        out int variant)
    {
        shape = shift = scale = variant = -1;
        return Supports(family) &&
            parameters is not null &&
            parameters.TryGetValue("shape", out shape) && shape is >= 0 and <= 4 &&
            parameters.TryGetValue("shift", out shift) && shift is >= 0 and <= 6 &&
            parameters.TryGetValue("scale", out scale) && scale is >= 1 and <= 3 &&
            parameters.TryGetValue("variant", out variant) && variant is >= 0 and <= 15;
    }

    private static string Normalize(string value)
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
}
