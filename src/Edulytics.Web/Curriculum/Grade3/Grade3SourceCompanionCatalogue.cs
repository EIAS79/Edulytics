using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Text.Json;

namespace Edulytics.Web.Curriculum.Grade3;

/// <summary>
/// Provisional first-party questions for G3 L09–L143. This registry deliberately
/// does NOT certify original publisher task, artwork, curriculum or Rich V2 parity.
/// </summary>
public static class Grade3SourceCompanionCatalogue
{
    public const string ResourceName = "Edulytics.Web.Grade3.UnverifiedOriginalCompanions";

    /// <summary>
    /// Mathematically redrawn data from the text and alternative-text description
    /// of the 2021 U01 L09/L10 scaled picture graph (not publisher artwork).
    /// Visual appearance, image rights and full 2021 diagram parity are not certified.
    /// </summary>
    public static IReadOnlyList<Grade3RoadSignCategory> Local2021RoadSignGraph { get; } =
        Array.AsReadOnly(new[]
        {
            new Grade3RoadSignCategory("Stop signs", 2),
            new Grade3RoadSignCategory("Yield signs", 1),
            new Grade3RoadSignCategory("Speed limit signs", 3),
            new Grade3RoadSignCategory("Street signs", 5)
        });

    public const int SourceGraphSignsPerSymbol = 2;

    private static readonly Lazy<IReadOnlyDictionary<string, Grade3CompanionLesson>> Values =
        new(Load, LazyThreadSafetyMode.ExecutionAndPublication);

    public static bool TryGet(string? code, out Grade3CompanionLesson? lesson)
    {
        if (code is not null && Values.Value.TryGetValue(code, out var found))
        {
            lesson = found;
            return true;
        }

        lesson = null;
        return false;
    }

    public static IReadOnlyCollection<string> Codes => Values.Value.Keys.ToArray();

    private static IReadOnlyDictionary<string, Grade3CompanionLesson> Load()
    {
        using var stream = typeof(Grade3SourceCompanionCatalogue).Assembly
            .GetManifestResourceStream(ResourceName)
            ?? throw new InvalidOperationException("Grade 3 source companion bank was not embedded.");
        var bank = JsonSerializer.Deserialize<Grade3CompanionBank>(stream,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
            ?? throw new InvalidOperationException("Grade 3 companion bank is missing.");
        var result = new Dictionary<string, Grade3CompanionLesson>(StringComparer.Ordinal);
        foreach (var item in bank.Lessons)
        {
            if (item.Ordinal < 9 || item.Ordinal > 143 ||
                !item.LessonCode.StartsWith("PED:US-CCSS-MATH:G3:", StringComparison.Ordinal) ||
                !result.TryAdd(item.LessonCode, item))
                throw new InvalidOperationException("Invalid/duplicate Grade 3 lesson bank identity.");
            if (item.IndependentPractice is null || item.OriginalEdulyticsCoolDown is null)
                throw new InvalidOperationException("Missing first-party task for " + item.LessonCode);
        }
        if (result.Count != 135)
            throw new InvalidOperationException($"Expected 135 first-pass Grade 3 companion lessons, got {result.Count}.");
        return result;
    }

    /// <summary>
    /// Strict numeric checker. Equivalent reduced/unreduced rational answers are
    /// accepted, but drawings and explanations are ALWAYS teacher-reviewed.
    /// </summary>
    public static Grade3CompanionFeedback Grade(
        Grade3OriginalQuestion q, string? submitted)
    {
        if (q.AnswerType == "teacher")
            return new(null, true,
                "Open construction, dialogue or explanation: only teacher review can assess this activity.");

        if (string.IsNullOrWhiteSpace(submitted) || submitted.Length > 100)
            return new(false, true, "Enter an answer and explain your strategy to your teacher.");

        if (q.AnswerType == "comparison")
        {
            var ok = submitted.Trim() is ">" or "<" or "=" &&
                string.Equals(submitted.Trim(), q.Expected, StringComparison.Ordinal);
            return new(ok, true, ok ? "The comparison symbol matches this first-party example."
                : "Compare the quantities using the same whole.");
        }

        if (q.AnswerType == "multiplication-expression")
        {
            // In Grade 3 this family assesses a written expression, NOT only
            // its product. The first factor identifies the equal-group count,
            // the second the number in each group. Other justified models
            // are always eligible for human consideration, never auto-certified.
            const string syntax = @"^([0-9]{1,3})\s*[xX×*]\s*([0-9]{1,3})(?:\s*=\s*([0-9]{1,5}))?$";
            var actual = Regex.Match(submitted.Trim(), syntax, RegexOptions.CultureInvariant);
            var expected = Regex.Match(q.Expected ?? "", syntax, RegexOptions.CultureInvariant);
            if (!actual.Success || !expected.Success)
                return new(false, true, "Write groups × objects per group; a product alone does not demonstrate the requested expression.");

            var groups = int.Parse(actual.Groups[1].Value, CultureInfo.InvariantCulture);
            var each = int.Parse(actual.Groups[2].Value, CultureInfo.InvariantCulture);
            var requiredGroups = int.Parse(expected.Groups[1].Value, CultureInfo.InvariantCulture);
            var requiredEach = int.Parse(expected.Groups[2].Value, CultureInfo.InvariantCulture);
            if (actual.Groups[3].Success &&
                int.Parse(actual.Groups[3].Value, CultureInfo.InvariantCulture) != groups * each)
                return new(false, true, "The stated total contradicts the factors. Check your expression.");

            var correct = groups == requiredGroups && each == requiredEach;
            return new(correct, true, correct
                ? "The expression matches the number and size of the groups; teacher still reviews the drawing."
                : "Check which number describes the groups and which describes the members in each group. Teacher reviews alternative justifications.");
        }

        if (q.AnswerType == "integer")
        {
            if (!int.TryParse(submitted.Trim(), NumberStyles.Integer,
                    CultureInfo.InvariantCulture, out var value) ||
                !int.TryParse(q.Expected, NumberStyles.Integer,
                    CultureInfo.InvariantCulture, out var expected))
                return new(false, true, "Enter a whole number.");

            return new(value == expected, true,
                value == expected ? "The number is correct for this original example."
                    : "Review the context and quantities. Drawing or reasoning still requires a teacher.");
        }

        if (q.AnswerType == "clock-time")
        {
            if (!TryClockTime(submitted, out var actual) ||
                !TryClockTime(q.Expected, out var expected))
                return new(false, true,
                    "Write a valid 12-hour clock time such as 3:51 p.m.; include a.m. or p.m.");

            var correct = actual == expected;
            return new(correct, true,
                correct ? "The complete time is mathematically correct. Clock drawing and strategy still require a teacher."
                        : "Recheck both the hour and minute values; crossing an hour changes the hour hand.");
        }

        if (q.AnswerType == "rational")
        {
            if (!TryFraction(submitted, out var an, out var ad) ||
                !TryFraction(q.Expected, out var en, out var ed))
                return new(false, true, "Write a valid fraction such as 3/4.");
            // Value equivalence alone is not sufficient when the question specifies
            // how to represent the partition (e.g. "in eighths").
            if (q.RequiredDenominator is int denominator && ad != denominator)
                return new(false, true,
                    $"Write the answer in {denominator}ths as requested; an equivalent value with a different denominator does not demonstrate the specified partition.");
            // Long arithmetic avoids overflow with bounded fractions.
            var correct = (long)an * ed == (long)en * ad;
            return new(correct, true, correct
                ? "Equivalent fraction accepted for this original example."
                : "Use equal parts of the same whole, then check numerator and denominator.");
        }

        return new(null, true, "Unknown task type. Teacher review only.");
    }

    private static bool TryClockTime(string? text, out int minutesSinceMidnight)
    {
        minutesSinceMidnight = 0;
        if (string.IsNullOrWhiteSpace(text) || text.Length > 24) return false;
        // 12-hour digital notation only. Without context, an analogue dial
        // alone cannot distinguish a.m. from p.m., so the prompt supplies it.
        var match = Regex.Match(text.Trim(),
            @"^(0?[1-9]|1[0-2]):([0-5][0-9])\s*([ap])\.?m\.?$",
            RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
        if (!match.Success) return false;
        var hour = int.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
        var minute = int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);
        var pm = string.Equals(match.Groups[3].Value, "p", StringComparison.OrdinalIgnoreCase);
        minutesSinceMidnight = (hour % 12 + (pm ? 12 : 0)) * 60 + minute;
        return true;
    }

    private static bool TryFraction(string? text, out int numerator, out int denominator)
    {
        numerator = 0;
        denominator = 0;
        if (text is null || text.Length > 24) return false;
        // A legitimate length such as "1 3/4" is the same number as 7/4.
        // Grade 3 measurement lessons explicitly use mixed inch lengths.
        var parts = text.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length == 2)
        {
            if (!int.TryParse(parts[0], NumberStyles.None, CultureInfo.InvariantCulture, out var whole) ||
                whole is < 0 or > 10000 ||
                !TryFraction(parts[1], out var fractionalNumerator, out var fractionalDenominator) ||
                fractionalNumerator < 0 || fractionalNumerator >= fractionalDenominator)
                return false;
            var total = (long)whole * fractionalDenominator + fractionalNumerator;
            if (total > 10000) return false;
            numerator = (int)total;
            denominator = fractionalDenominator;
            return true;
        }
        if (parts.Length != 1) return false;
        var bits = parts[0].Split('/');
        if (bits.Length == 1 &&
            int.TryParse(bits[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out var n))
        {
            numerator = n;
            denominator = 1;
            return numerator is >= 0 and <= 10000;
        }
        if (bits.Length != 2 ||
            !int.TryParse(bits[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out numerator) ||
            !int.TryParse(bits[1], NumberStyles.Integer, CultureInfo.InvariantCulture, out denominator))
            return false;

        return numerator is >= 0 and <= 10000 && denominator is > 0 and <= 10000;
    }
}

public sealed record Grade3RoadSignCategory(string Category, int SymbolCount)
{
    public int TotalSigns => SymbolCount * Grade3SourceCompanionCatalogue.SourceGraphSignsPerSymbol;
}

public sealed record Grade3CompanionFeedback(bool? NumericCorrect, bool TeacherReview, string Message);

public sealed class Grade3CompanionBank
{
    public List<Grade3CompanionLesson> Lessons { get; set; } = [];
}

public sealed class Grade3CompanionLesson
{
    public int Ordinal { get; set; }
    public string LessonCode { get; set; } = "";
    public string Title { get; set; } = "";
    public int UnitNumber { get; set; }
    public int LessonNumber { get; set; }
    public string TeacherLearningGoals { get; set; } = "";
    public string StudentFacingGoal { get; set; } = "";
    public List<string> SourceActivityNames { get; set; } = [];
    // An optional, independently mapped 1:1 set of publicly verifiable
    // source numeric questions. Still separate from teacher-only components.
    public List<Grade3OriginalQuestion> SourceSubtasks { get; set; } = [];
    public string FirstPartyFamily { get; set; } = "";
    public Grade3OriginalQuestion? SourceBasedCheck { get; set; }
    public Grade3SourceCheckProvenance? SourceBasedCheckProvenance { get; set; }
    public bool ReviewedStudentFacingText { get; set; }
    public Grade3OriginalQuestion? IndependentPractice { get; set; }
    public Grade3OriginalQuestion? OriginalEdulyticsCoolDown { get; set; }
    public string OpenTeacherTask { get; set; } = "";
    public string TeacherReviewNote { get; set; } = "";
    public Grade3TeachingGuidance? IndependentTeachingGuidance { get; set; }
    public bool ReadyForStageD { get; set; }
}

public sealed class Grade3TeachingGuidance
{
    public string SourceGoal { get; set; } = "";
    public string SourceLearningGoals { get; set; } = "";
    public List<string> TeachingSteps { get; set; } = [];
    public string Caution { get; set; } = "";
}
public sealed class Grade3SourceCheckProvenance
{
    public string LocalHtml { get; set; } = "";
    public string Verified { get; set; } = "";
    public bool FigurePixelsInspected { get; set; }
}
public sealed class Grade3OriginalQuestion
{
    public string Id { get; set; } = "";
    public string SourceActivity { get; set; } = "";
    public string Prompt { get; set; } = "";
    public string AnswerType { get; set; } = "";
    public string? Expected { get; set; }
    /// <summary>When a representation in named equal parts is demanded, its denominator must match.</summary>
    public int? RequiredDenominator { get; set; }
}
