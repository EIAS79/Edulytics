using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// IM Algebra 1 U01:L01 P3: six first-party statistical-question choices.
/// The exact digital subskill accepts an unordered, nonduplicated set of
/// A–F choice IDs. Group survey, P1/P2 explanations, and Cool-down remain
/// TEACHER_REVIEW_REQUIRED and are NEVER marked correct by this helper.
/// A learner receives no answer letters in the generated prompt/feedback.
/// </summary>
public static class UsCcssAlg1U01L01StatisticalSelectionQuestions
{
    public const string Family = "usccss.alg1.u01.l01.statistical_questions_p3";

    public sealed record Problem(
        string Family,
        string Prompt,
        string Solution,
        AssessmentItemType ItemType,
        IReadOnlyDictionary<string, int> Parameters);

    public static bool Supports(string? family) =>
        string.Equals(family, Family, StringComparison.Ordinal);

    public static Problem Build(string family, Random random)
    {
        if (!Supports(family))
            throw new InvalidOperationException("Unsupported ALG1 U01:L01 P3 family.");
        ArgumentNullException.ThrowIfNull(random);

        var parameters = new Dictionary<string, int>(StringComparer.Ordinal)
        {
            ["classSize"] = random.Next(15, 45),
            ["window"] = random.Next(0, 4),
            ["category"] = random.Next(0, 4)
        };
        if (!ValidParameters(parameters))
            throw new InvalidOperationException("Malformed ALG1 P3 parameters.");

        var period = parameters["window"] switch
        {
            0 => "last week",
            1 => "yesterday",
            2 => "this school week",
            _ => "during the last seven days"
        };
        var category = parameters["category"] switch
        {
            0 => "lunch option",
            1 => "sport",
            2 => "board game",
            _ => "after-school club"
        };
        var prompt =
            "Select ALL statistical questions from six choices A-F (more than one). " +
            "Type the selected letters separated by commas, in any order; do not add other text. " +
            "For every choice consider whether answers vary across a POPULATION. " +
            $"A. How many pages did each of {parameters["classSize"]} pupils read {period}? " +
            "B. How many pages are in one specifically named edition of a book? " +
            $"C. Which {category} does each person in a class prefer? " +
            "D. What did one particular named person eat for lunch yesterday? " +
            "E. How many minutes does each pupil take to travel to school? " +
            "F. What is the capital city of Poland? " +
            "Open explanations, proposed surveys, privacy consent and the Cool-down are reviewed by a teacher.";

        return new(Family, prompt,
            "A statistical question anticipates variability across a GROUP; " +
            "a fixed value for one known object or person is not a statistical distribution. " +
            "Explain the population and variable to your teacher.",
            AssessmentItemType.ShortAnswer,
            parameters);
    }

    public static string Solve(string family, IReadOnlyDictionary<string, int> parameters)
    {
        if (!Supports(family) || !ValidParameters(parameters))
            throw new InvalidOperationException("Invalid ALG1 U01:L01 P3 parameters.");
        return "A,C,E";
    }

    public static bool Verify(
        string family,
        IReadOnlyDictionary<string, int> parameters,
        string? answer) =>
        Supports(family) && ValidParameters(parameters) &&
        TryNormalizeChoiceSet(answer, out var normalized) &&
        string.Equals(normalized, "ACE", StringComparison.Ordinal);

    /// <summary>
    /// For an explicitly authorized persisted answer-grading adapter.
    /// Not yet integrated into shared PracticeService without its own lease.
    /// The stored teacher-only key must itself represent exactly A,C,E.
    /// </summary>
    public static bool GradePersistedAnswer(string? expected, string? submitted) =>
        TryNormalizeChoiceSet(expected, out var expectedSet) &&
        string.Equals(expectedSet, "ACE", StringComparison.Ordinal) &&
        TryNormalizeChoiceSet(submitted, out var submittedSet) &&
        string.Equals(submittedSet, expectedSet, StringComparison.Ordinal);

    private static bool ValidParameters(IReadOnlyDictionary<string, int>? parameters) =>
        parameters is { Count: 3 } &&
        parameters.TryGetValue("classSize", out var count) && count is >= 15 and < 45 &&
        parameters.TryGetValue("window", out var window) && window is >= 0 and < 4 &&
        parameters.TryGetValue("category", out var category) && category is >= 0 and < 4;

    private static bool TryNormalizeChoiceSet(string? response, out string normalized)
    {
        normalized = "";
        if (string.IsNullOrWhiteSpace(response))
            return false;
        var tokens = response.Split(',', StringSplitOptions.None);
        if (tokens.Length != 3)
            return false;
        var choices = new HashSet<char>();
        foreach (var token in tokens)
        {
            var candidate = token.Trim().ToUpperInvariant();
            if (candidate.Length != 1 || candidate[0] is < 'A' or > 'F' ||
                !choices.Add(candidate[0]))
                return false;
        }
        normalized = new string(choices.OrderBy(x => x).ToArray());
        return true;
    }
}
