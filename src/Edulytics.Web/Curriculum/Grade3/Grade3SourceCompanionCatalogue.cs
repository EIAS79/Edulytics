using System.Globalization;
using System.Reflection;
using System.Text.Json;

namespace Edulytics.Web.Curriculum.Grade3;

/// <summary>
/// Provisional first-party questions for G3 L09–L143. This registry deliberately
/// does NOT certify original publisher task, artwork, curriculum or Rich V2 parity.
/// </summary>
public static class Grade3SourceCompanionCatalogue
{
    public const string ResourceName = "Edulytics.Web.Grade3.UnverifiedOriginalCompanions";
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

        if (q.AnswerType == "rational")
        {
            if (!TryFraction(submitted, out var an, out var ad) ||
                !TryFraction(q.Expected, out var en, out var ed))
                return new(false, true, "Write a valid fraction such as 3/4.");
            // Long arithmetic avoids overflow with bounded fractions.
            var correct = (long)an * ed == (long)en * ad;
            return new(correct, true, correct
                ? "Equivalent fraction accepted for this original example."
                : "Use equal parts of the same whole, then check numerator and denominator.");
        }

        return new(null, true, "Unknown task type. Teacher review only.");
    }

    private static bool TryFraction(string? text, out int numerator, out int denominator)
    {
        numerator = 0;
        denominator = 0;
        if (text is null || text.Length > 24) return false;
        var bits = text.Trim().Split('/');
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
    public string FirstPartyFamily { get; set; } = "";
    public Grade3OriginalQuestion? IndependentPractice { get; set; }
    public Grade3OriginalQuestion? OriginalEdulyticsCoolDown { get; set; }
    public string OpenTeacherTask { get; set; } = "";
    public string TeacherReviewNote { get; set; } = "";
    public bool ReadyForStageD { get; set; }
}

public sealed class Grade3OriginalQuestion
{
    public string Prompt { get; set; } = "";
    public string AnswerType { get; set; } = "";
    public string? Expected { get; set; }
}
