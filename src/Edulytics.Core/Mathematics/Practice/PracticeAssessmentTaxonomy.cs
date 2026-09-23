namespace Edulytics.Core.Mathematics.Practice;

public enum PracticeQuestionForm
{
    Identify = 1,
    Select = 2,
    Classify = 3,
    Sort = 4,
    Compare = 5,
    Calculate = 6,
    Apply = 7,
    Explain = 8,
    ErrorAnalysis = 9,
    Transfer = 10
}

public enum PracticeCognitiveOperation
{
    Recall = 1,
    Understand = 2,
    Apply = 3,
    Reason = 4,
    Transfer = 5
}

public enum PracticeCognitiveDifficulty
{
    Standard = 1,
    Stretch = 2,
    Challenge = 3
}

public sealed record PracticeQuestionFormCapability(
    PracticeQuestionForm Form,
    PracticeCognitiveOperation CognitiveOperation,
    PracticeCognitiveDifficulty MinimumDifficulty,
    PracticeCognitiveDifficulty MaximumDifficulty,
    IReadOnlyList<int> VariantSlots);

/// <summary>
/// Truthful registry of learner-facing question forms currently implemented by
/// each Practice family. Families without a specialized multi-form generator
/// expose one conservative capability; richer capabilities are added only
/// when the generator actually implements them.
/// </summary>
public static class PracticeQuestionFormCapabilityRegistry
{
    private static readonly int[] AllVariantSlots =
        Enumerable.Range(0, 16).ToArray();

    public static IReadOnlyList<PracticeQuestionFormCapability> Resolve(
        string family)
    {
        if (string.IsNullOrWhiteSpace(family))
            return [];

        family = family.Trim();

        if (string.Equals(
                family,
                "supporting.geometry.shape_dimension",
                StringComparison.Ordinal))
        {
            return
            [
                new(
                    PracticeQuestionForm.Identify,
                    PracticeCognitiveOperation.Recall,
                    PracticeCognitiveDifficulty.Standard,
                    PracticeCognitiveDifficulty.Standard,
                    [0, 1, 2, 3]),
                new(
                    PracticeQuestionForm.Classify,
                    PracticeCognitiveOperation.Understand,
                    PracticeCognitiveDifficulty.Standard,
                    PracticeCognitiveDifficulty.Stretch,
                    [4, 5, 6, 7]),
                new(
                    PracticeQuestionForm.ErrorAnalysis,
                    PracticeCognitiveOperation.Reason,
                    PracticeCognitiveDifficulty.Stretch,
                    PracticeCognitiveDifficulty.Challenge,
                    [8, 9, 10, 11]),
                new(
                    PracticeQuestionForm.Transfer,
                    PracticeCognitiveOperation.Transfer,
                    PracticeCognitiveDifficulty.Challenge,
                    PracticeCognitiveDifficulty.Challenge,
                    [12, 13, 14, 15])
            ];
        }

        if (family.Contains(
                "compare",
                StringComparison.OrdinalIgnoreCase) ||
            family.Contains(
                "order",
                StringComparison.OrdinalIgnoreCase))
        {
            return
            [
                new(
                    PracticeQuestionForm.Compare,
                    PracticeCognitiveOperation.Understand,
                    PracticeCognitiveDifficulty.Standard,
                    PracticeCognitiveDifficulty.Standard,
                    AllVariantSlots)
            ];
        }

        if (family.Contains(
                "recognize",
                StringComparison.OrdinalIgnoreCase) ||
            family.Contains(
                "classify",
                StringComparison.OrdinalIgnoreCase) ||
            family.Contains(
                "criterion",
                StringComparison.OrdinalIgnoreCase))
        {
            return
            [
                new(
                    PracticeQuestionForm.Classify,
                    PracticeCognitiveOperation.Understand,
                    PracticeCognitiveDifficulty.Standard,
                    PracticeCognitiveDifficulty.Standard,
                    AllVariantSlots)
            ];
        }

        if (family.Contains(
                "reasoning",
                StringComparison.OrdinalIgnoreCase) ||
            family.Contains(
                "multistep",
                StringComparison.OrdinalIgnoreCase))
        {
            return
            [
                new(
                    PracticeQuestionForm.Apply,
                    PracticeCognitiveOperation.Reason,
                    PracticeCognitiveDifficulty.Standard,
                    PracticeCognitiveDifficulty.Standard,
                    AllVariantSlots)
            ];
        }

        return
        [
            new(
                PracticeQuestionForm.Calculate,
                PracticeCognitiveOperation.Apply,
                PracticeCognitiveDifficulty.Standard,
                PracticeCognitiveDifficulty.Challenge,
                AllVariantSlots)
        ];
    }

    public static PracticeQuestionFormCapability? ResolveForVariant(
        string family,
        int variantSlot)
    {
        var normalized =
            ((variantSlot % 16) + 16) % 16;

        return Resolve(family)
            .FirstOrDefault(capability =>
                capability.VariantSlots.Contains(normalized));
    }
}
