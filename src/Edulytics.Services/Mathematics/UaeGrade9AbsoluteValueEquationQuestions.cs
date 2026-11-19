using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Source-specific Grade 9 General absolute-value equation adapter.
/// Extends the existing Exact Skill question kernel; it must be semantically
/// audited with real Arabic prompts in Stage D before promotion to READY_VERIFIED.
/// </summary>
internal static class UaeGrade9AbsoluteValueEquationQuestions
{
    internal const string TwoSolutions = "uae.g9.u02.l05.absolute_equation_two_solutions";
    internal const string OneSolution = "uae.g9.u02.l05.absolute_equation_zero_distance";
    internal const string NoSolution = "uae.g9.u02.l05.absolute_equation_negative_distance";

    internal sealed record Question(
        string Family, string Prompt, string Solution,
        AssessmentItemType ItemType, IReadOnlyDictionary<string, int> Parameters);

    internal static bool Supports(string? family) =>
        family is TwoSolutions or OneSolution or NoSolution;

    internal static Question Build(string family, Random random)
    {
        if (!Supports(family))
            throw new ArgumentException("Unsupported Grade 9 absolute-value family.", nameof(family));

        int coefficient = random.Next(1, 5);
        int center = random.Next(-8, 9);
        int delta = random.Next(1, 7);
        int offset = -coefficient * center;
        int rhs = family switch
        {
            TwoSolutions => coefficient * delta,
            OneSolution => 0,
            _ => -random.Next(1, 11)
        };
        string offsetPart = offset switch
        {
            > 0 => " + " + offset.ToString(CultureInfo.InvariantCulture),
            < 0 => " − " + (-offset).ToString(CultureInfo.InvariantCulture),
            _ => ""
        };
        string lhs = coefficient == 1 ? "س" : coefficient.ToString(CultureInfo.InvariantCulture) + "س";
        var p = new Dictionary<string, int>
        {
            ["coefficient"] = coefficient,
            ["offset"] = offset,
            ["rhs"] = rhs
        };
        return new Question(
            family,
            $"أوجد مجموعة حل المعادلة |{lhs}{offsetPart}| = {rhs}، مع مراعاة أن القيمة المطلقة غير سالبة.",
            "إذا كان الطرف الأيمن سالبًا فلا حل. إذا كان صفرًا فالتعبير داخل القيمة المطلقة يساوي صفرًا. وإذا كان موجبًا فحل المعادلتين الخطيّتين بمساواة التعبير بالقيمة الموجبة والسالبة.",
            AssessmentItemType.ShortAnswer,
            p);
    }

    internal static string Solve(string family, IReadOnlyDictionary<string, int> p)
    {
        if (!Supports(family) || !ValidParameters(p))
            throw new ArgumentException("Invalid absolute-value equation parameters.", nameof(p));
        int a = p["coefficient"], b = p["offset"], rhs = p["rhs"];
        if (rhs < 0) return "لا حل";
        if (rhs == 0) return (-b / a).ToString(CultureInfo.InvariantCulture);
        int first = (-b - rhs) / a;
        int second = (-b + rhs) / a;
        return $"{Math.Min(first, second)}, {Math.Max(first, second)}";
    }

    internal static bool Verify(
        string family, IReadOnlyDictionary<string, int> p, string? answer)
    {
        if (!Supports(family) || !ValidParameters(p) || string.IsNullOrWhiteSpace(answer))
            return false;
        var a = p["coefficient"];
        var b = p["offset"];
        var rhs = p["rhs"];
        string normalized = string.Concat(answer.Trim().Select(c => c switch
        {
            >= '٠' and <= '٩' => (char)('0' + c - '٠'),
            >= '۰' and <= '۹' => (char)('0' + c - '۰'),
            '−' or '–' => '-',
            '،' or '؛' or ';' => ',',
            _ => c
        }));
        if (rhs < 0)
            return normalized is "لا حل" or "لا يوجد حل" or "المجموعة الخالية" or "∅" or "{}";
        var parts = normalized.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        int expectedCount = rhs == 0 ? 1 : 2;
        if (parts.Length != expectedCount)
            return false;
        var values = new HashSet<int>();
        foreach (var part in parts)
        {
            if (!int.TryParse(part, NumberStyles.Integer, CultureInfo.InvariantCulture, out int x))
                return false;
            if (!values.Add(x) || Math.Abs((long)a * x + b) != rhs)
                return false;
        }
        return values.Count == expectedCount;
    }

    private static bool ValidParameters(IReadOnlyDictionary<string, int> p) =>
        p.TryGetValue("coefficient", out int a) && a > 0 &&
        p.TryGetValue("offset", out int b) &&
        p.TryGetValue("rhs", out int rhs) &&
        b % a == 0 &&
        (rhs <= 0 || rhs % a == 0);
}
