using System.Text.Json;
using Edulytics.Core.Entities;

namespace Edulytics.Services.AdaptivePractice;

public static class AdaptiveRemediationStageCodes
{
    public const string TargetedRetry = "TARGETED_RETRY";
    public const string ScaffoldedRecovery = "SCAFFOLDED_RECOVERY";
    public const string FreshConfirmation = "FRESH_CONFIRMATION";
}

public sealed record AdaptiveRemediationGuidance(
    string StageCode,
    string Hint,
    string? WorkedExample,
    string? MisconceptionId,
    int AttemptNumber);

/// <summary>
/// Server-side, answer-aware remediation guidance.
///
/// It may use the verified answer internally but must not reveal the current
/// item's final answer in Hint Level 1 or in the independent worked example.
/// </summary>
public sealed class AdaptiveRemediationGuidanceEngine(
    AdaptiveMisconceptionClassifier classifier)
{
    public AdaptiveRemediationGuidance Build(
        AssessmentItem item,
        string submittedAnswer,
        int attemptNumber)
    {
        ArgumentNullException.ThrowIfNull(item);

        if (string.IsNullOrWhiteSpace(submittedAnswer) ||
            attemptNumber <= 0)
        {
            throw new InvalidOperationException(
                "Adaptive remediation guidance requires a learner answer and positive attempt number.");
        }

        var classification =
            classifier.Classify(
                item,
                submittedAnswer);

        if (string.Equals(
                item.GenerationFamily,
                "supporting.number.rounding",
                StringComparison.Ordinal) &&
            TryReadRoundingParameters(
                item.GenerationParametersJson,
                out var value,
                out var place))
        {
            return BuildRounding(
                value,
                place,
                classification?.MisconceptionId,
                attemptNumber);
        }

        var method = string.IsNullOrWhiteSpace(item.Solution)
            ? "Re-read the question, identify the exact mathematical relationship, and check each step before trying again."
            : item.Solution.Trim();

        if (attemptNumber == 1)
        {
            return new(
                AdaptiveRemediationStageCodes.TargetedRetry,
                method,
                null,
                classification?.MisconceptionId,
                attemptNumber);
        }

        return new(
            AdaptiveRemediationStageCodes.ScaffoldedRecovery,
            "Stop and rebuild the method before solving the new item.",
            $"Worked strategy: {method} Apply the method to the new numbers rather than reusing the previous result.",
            classification?.MisconceptionId,
            attemptNumber);
    }

    private static AdaptiveRemediationGuidance BuildRounding(
        int value,
        int place,
        string? misconceptionId,
        int attemptNumber)
    {
        var rightPlace = Math.Max(1, place / 10);
        var decidingDigit =
            Math.Abs(value / rightPlace) % 10;
        var roundsUp = decidingDigit >= 5;

        var targeted = misconceptionId switch
        {
            "rounding.lower_places_not_zeroed" =>
                $"You are rounding to the nearest {place}. After choosing the nearer multiple, every digit to the right of the {place} place must become zero.",

            "rounding.wrong_direction_up" or
            "rounding.wrong_direction_down" =>
                $"Look at the digit immediately to the right of the {place} place. It is {decidingDigit}. Because it is {(roundsUp ? "5 or more" : "less than 5")}, {(roundsUp ? "increase" : "keep")} the target-place digit, then zero all lower places.",

            "rounding.adjacent_multiple" =>
                $"Write the two consecutive multiples of {place} around {value}. Compare how far {value} is from each one, then choose the nearer multiple.",

            _ =>
                $"Find the two multiples of {place} on either side of {value}. Use the halfway point and the digit immediately to the right of the target place to decide which multiple is nearer."
        };

        if (attemptNumber == 1)
        {
            return new(
                AdaptiveRemediationStageCodes.TargetedRetry,
                targeted,
                null,
                misconceptionId,
                attemptNumber);
        }

        var sampleBase = 4 * place;
        var sampleOffset = Math.Max(1, 3 * rightPlace);
        var sample = sampleBase + sampleOffset;
        var sampleRounded = sampleBase;

        return new(
            AdaptiveRemediationStageCodes.ScaffoldedRecovery,
            "The same item is now closed. Use the worked example, then solve the fresh recovery question.",
            $"Worked example: round {sample} to the nearest {place}. The deciding digit is 3, which is less than 5, so keep the target-place digit and replace all lower digits with zero. The result is {sampleRounded}.",
            misconceptionId,
            attemptNumber);
    }

    private static bool TryReadRoundingParameters(
        string? json,
        out int value,
        out int place)
    {
        value = 0;
        place = 0;

        if (string.IsNullOrWhiteSpace(json))
            return false;

        try
        {
            using var document = JsonDocument.Parse(json);

            if (!document.RootElement.TryGetProperty(
                    "parameters",
                    out var parameters) ||
                !parameters.TryGetProperty(
                    "value",
                    out var valueElement) ||
                !parameters.TryGetProperty(
                    "place",
                    out var placeElement))
            {
                return false;
            }

            return valueElement.TryGetInt32(out value) &&
                   placeElement.TryGetInt32(out place) &&
                   place > 0;
        }
        catch (JsonException)
        {
            return false;
        }
    }
}
