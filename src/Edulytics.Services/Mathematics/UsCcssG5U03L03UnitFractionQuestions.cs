using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// G5 IM K–5 2021 U03:L03 course-specific AUTHORING CANDIDATE only.
/// Requires separately approved exclusive shared Practice registration before
/// it can be selected by a learner; tests are AUTHORED_NOT_RUN.
/// Source A–D grid arguments, shaded source pictures, estimates and written
/// explanations remain TEACHER_REVIEW_REQUIRED, not machine scored.
/// </summary>
public static class UsCcssG5U03L03UnitFractionQuestions
{
    public const string PatternFamily =
        "usccss.g5.u03.l03.four_columns_unit_fraction_pattern";
    public const string EquationFamily =
        "usccss.g5.u03.l03.unit_fraction_equation_value";

    public sealed record Problem(string Family, string Prompt, string Solution,
        AssessmentItemType ItemType, IReadOnlyDictionary<string, int> Parameters);

    private static readonly int[] SourceRows = [2, 3, 4, 5, 6];
    private static readonly (int First, int Second)[] SourceEquations =
        [(5, 3), (2, 6), (4, 6)];

    public static bool Supports(string? family) =>
        family is PatternFamily or EquationFamily;

    /// <summary>
    /// Build a strictly bounded exact digital choice. The six-row scenario
    /// represents the original teacher task's "predict the next diagram";
    /// the three equation cases reproduce the public numerical source tasks.
    /// </summary>
    public static Problem Build(string family, Random random)
    {
        if (!Supports(family))
            throw new InvalidOperationException("Not a Grade 5 U03:L03 unit-fraction family.");
        ArgumentNullException.ThrowIfNull(random);
        int caseCount = family == PatternFamily ? SourceRows.Length : SourceEquations.Length;
        IReadOnlyDictionary<string, int> p = new Dictionary<string, int>(StringComparer.Ordinal)
        {
            ["case"] = random.Next(caseCount),
            ["slot"] = random.Next(3),
            ["reversed"] = random.Next(2)
        };
        if (!TryParameters(family, p, out int first, out int second))
            throw new InvalidOperationException("Invalid Grade 5 unit-fraction source case.");
        int product = checked(first * second);
        string answer = $"1/{product}";
        string[] options = p["slot"] switch
        {
            0 => [answer, $"1/{first}", $"1/{second}"],
            1 => [$"1/{first}", answer, $"1/{second}"],
            _ => [$"1/{first}", $"1/{second}", answer]
        };
        string request = family == PatternFamily
            ? $"The same ORIGINAL unit square has FOUR equal columns and {second} equal rows. " +
              (second == 6 ? "This is the predicted NEXT diagram after the source A–D pattern. " :
               "This matches one member of the source A–D four-column pattern. ") +
              "Exactly one equal cell is shaded. "
            : $"Calculate the ORIGINAL source unit-fraction expression " +
              (p["reversed"] == 0 ? $"(1/{first}) × (1/{second})" :
               $"(1/{second}) × (1/{first})") + " of the SAME original whole. ";
        string prompt = request +
            $"Which is the exact shaded fraction? Select 1: {options[0]}; " +
            $"2: {options[1]}; 3: {options[2]}. " +
            "Original source-image arguments and learner drawings are TEACHER_REVIEW_REQUIRED.";
        string explanation = $"The ORIGINAL whole has {first} × {second} = {product} " +
            $"equal cells, so a single cell is {answer}. " +
            "An objective numeric selection is not an assessment of the open source drawing, " +
            "the estimations or the revised written diagram explanation.";
        return new Problem(family, prompt, explanation, AssessmentItemType.Numeric, p);
    }

    public static string Solve(string family, IReadOnlyDictionary<string, int> p)
    {
        if (!TryParameters(family, p, out _, out _))
            throw new InvalidOperationException("Invalid Grade 5 U03:L03 selected-family parameters.");
        return (p["slot"] + 1).ToString(CultureInfo.InvariantCulture);
    }

    public static bool Verify(string family, IReadOnlyDictionary<string, int>? p, string? answer) =>
        TryParameters(family, p, out _, out _) &&
        string.Equals(answer, (p!["slot"] + 1).ToString(CultureInfo.InvariantCulture),
            StringComparison.Ordinal);

    private static bool TryParameters(string? family, IReadOnlyDictionary<string, int>? p,
        out int first, out int second)
    {
        first = second = 0;
        if (!Supports(family) || p is null || p.Count != 3 ||
            !p.TryGetValue("case", out int sourceCase) ||
            !p.TryGetValue("slot", out int slot) || slot is < 0 or > 2 ||
            !p.TryGetValue("reversed", out int reversed) || reversed is < 0 or > 1)
            return false;
        if (family == PatternFamily)
        {
            if (sourceCase < 0 || sourceCase >= SourceRows.Length)
                return false;
            first = 4;
            second = SourceRows[sourceCase];
            return true;
        }
        if (sourceCase < 0 || sourceCase >= SourceEquations.Length)
            return false;
        (first, second) = SourceEquations[sourceCase];
        return true;
    }
}
