using System.Text.RegularExpressions;

namespace Edulytics.Core.Mathematics.Practice;

public sealed record PracticeAnswerLeakCheck(
    bool IsSafe,
    string ReasonCode)
{
    public static PracticeAnswerLeakCheck Safe() =>
        new(true, "NO_ANSWER_LEAK");

    public static PracticeAnswerLeakCheck Unsafe(string reasonCode) =>
        new(false, reasonCode);
}

/// <summary>
/// Fail-closed checks for explicit learner-visible answer disclosure.
///
/// This validator deliberately does not reject ordinary prompts that contain
/// legitimate answer choices (for example "2D or 3D"). It rejects explicit
/// answer statements and family-specific visual disclosures that reveal the
/// target before the learner answers.
/// </summary>
public static class PracticeAnswerLeakValidator
{
    private static readonly string[] ExplicitDisclosurePrefixes =
    [
        "answer is",
        "correct answer is",
        "the answer is",
        "solution is",
        "correct response is"
    ];

    public static PracticeAnswerLeakCheck ValidatePrompt(
        string? family,
        string? prompt,
        string? correctAnswer)
    {
        if (string.IsNullOrWhiteSpace(prompt))
            return PracticeAnswerLeakCheck.Unsafe("EMPTY_LEARNER_PROMPT");

        if (string.IsNullOrWhiteSpace(correctAnswer))
            return PracticeAnswerLeakCheck.Unsafe("EMPTY_CORRECT_ANSWER");

        var normalizedPrompt = Normalize(prompt);
        var normalizedAnswer = Normalize(correctAnswer);

        foreach (var prefix in ExplicitDisclosurePrefixes)
        {
            if (normalizedPrompt.Contains(
                    $"{prefix} {normalizedAnswer}",
                    StringComparison.OrdinalIgnoreCase))
            {
                return PracticeAnswerLeakCheck.Unsafe(
                    "PROMPT_EXPLICITLY_REVEALS_ANSWER");
            }
        }

        return PracticeAnswerLeakCheck.Safe();
    }

    public static PracticeAnswerLeakCheck ValidateVisual(
        string? family,
        string? correctAnswer,
        string? learnerVisibleVisual)
    {
        if (string.IsNullOrWhiteSpace(learnerVisibleVisual))
            return PracticeAnswerLeakCheck.Safe();

        var normalizedFamily = family?.Trim() ?? string.Empty;
        var normalizedVisual = Normalize(learnerVisibleVisual);
        var normalizedAnswer = Normalize(correctAnswer);

        foreach (var prefix in ExplicitDisclosurePrefixes)
        {
            if (!string.IsNullOrWhiteSpace(normalizedAnswer) &&
                normalizedVisual.Contains(
                    $"{prefix} {normalizedAnswer}",
                    StringComparison.OrdinalIgnoreCase))
            {
                return PracticeAnswerLeakCheck.Unsafe(
                    "VISUAL_EXPLICITLY_REVEALS_ANSWER");
            }
        }

        if (string.Equals(
                normalizedFamily,
                "supporting.geometry.shape_dimension",
                StringComparison.Ordinal))
        {
            if (Regex.IsMatch(
                    learnerVisibleVisual,
                    @"\b(?:2D|3D)\s+shape\b",
                    RegexOptions.IgnoreCase |
                    RegexOptions.CultureInvariant))
            {
                return PracticeAnswerLeakCheck.Unsafe(
                    "SHAPE_DIMENSION_VISUAL_REVEALS_CLASSIFICATION");
            }
        }

        return PracticeAnswerLeakCheck.Safe();
    }

    private static string Normalize(string value) =>
        Regex.Replace(
                value,
                @"\s+",
                " ",
                RegexOptions.CultureInvariant)
            .Trim()
            .TrimEnd('.', '!', '?', ':', ';');
}
