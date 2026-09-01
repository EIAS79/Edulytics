using System.Security.Cryptography;
using System.Text;
using Edulytics.Core.AssessmentIntelligence;
using Edulytics.Core.Entities;
using Edulytics.Core.MathematicsGeneration;
using Edulytics.Core.Recovery;
using Edulytics.Services.MathematicsGeneration;

namespace Edulytics.Services.Recovery;

/// <summary>
/// Generates an equivalent reassessment that is genuinely fresh relative to the
/// student's prior exposure while preserving the strict Phase 36 validation gate.
/// </summary>
public sealed class EquivalentReassessmentGenerator
{
    private const int MaxGenerationAttempts = 128;

    private static readonly string[] ReviewedPromptPrefixes =
    [
        "Work out the following:",
        "Calculate the following:",
        "Find the answer:",
        "Determine the result:",
        "Solve this problem:",
        "Evaluate the following:",
        "Find the value:",
        "Compute the result:",
        "Complete this calculation:",
        "Answer the following:",
        "Determine the answer:",
        "Work through this problem:",
        "Find the result:",
        "Calculate the value:",
        "Solve the following:",
        "Give the result for this problem:"
    ];

    private readonly MathematicsQuestionGenerationEngine _generator;
    private readonly WeaknessRecoveryEngine _recovery;

    public EquivalentReassessmentGenerator(
        MathematicsQuestionGenerationEngine generator,
        WeaknessRecoveryEngine recovery)
    {
        _generator = generator ?? throw new ArgumentNullException(nameof(generator));
        _recovery = recovery ?? throw new ArgumentNullException(nameof(recovery));
    }

    public MathematicsGenerationBatch Generate(
        WeaknessRecoveryPlan plan,
        IReadOnlyList<MathematicsOutcomeGenerationProfile> outcomeProfiles,
        int seed = 0)
    {
        ArgumentNullException.ThrowIfNull(plan);
        ArgumentNullException.ThrowIfNull(outcomeProfiles);

        var previousFingerprints = plan.ExcludedExposureFingerprints
            .ToHashSet(StringComparer.Ordinal);
        var previousShapes = plan.PreviousPromptShapes
            .ToHashSet(StringComparer.Ordinal);
        var selectedFingerprints = new HashSet<string>(StringComparer.Ordinal);
        var selectedShapes = new HashSet<string>(StringComparer.Ordinal);
        var selected = new GeneratedMathematicsItem?[plan.EquivalentReassessmentBlueprint.QuestionCount];

        for (var attempt = 0; attempt < MaxGenerationAttempts && selected.Any(x => x is null); attempt++)
        {
            var batch = _generator.Generate(new MathematicsGenerationRequest(
                plan.EquivalentReassessmentBlueprint,
                outcomeProfiles,
                unchecked(seed + attempt)));

            if (batch.Items.Count != selected.Length)
            {
                throw new InvalidOperationException(
                    "Generated equivalent reassessment batch does not match the blueprint item count.");
            }

            for (var index = 0; index < batch.Items.Count; index++)
            {
                if (selected[index] is not null)
                {
                    continue;
                }

                var candidate = EnsureFreshPromptShape(
                    batch.Items[index],
                    plan.EquivalentReassessmentBlueprint,
                    previousShapes,
                    selectedShapes,
                    attempt,
                    index);

                if (!IsFresh(
                        candidate,
                        previousFingerprints,
                        previousShapes,
                        selectedFingerprints,
                        selectedShapes))
                {
                    continue;
                }

                selected[index] = candidate;
                selectedFingerprints.Add(candidate.Item.ExposureFingerprint);
                selectedShapes.Add(
                    WeaknessRecoveryEngine.NormalizePromptShape(
                        candidate.Item.Prompt));
            }
        }

        if (selected.Any(x => x is null))
        {
            throw new InvalidOperationException(
                "Unable to generate a fresh equivalent reassessment within the retry budget.");
        }

        var result = new MathematicsGenerationBatch(
            plan.SchoolId,
            plan.CurriculumAdoptionId,
            plan.CurriculumLevelKey,
            selected.Select(x => x!).ToArray(),
            MathematicsQuestionGenerationEngine.GeneratorVersion);

        // Do not weaken or bypass the Phase 36 acceptance contract. Every selected
        // item came from a fully validated Phase 33 batch at the same blueprint slot;
        // the Phase 36 validator remains authoritative for scope, difficulty,
        // coverage, exposure and equivalence semantics across the recomposed batch.
        _recovery.ValidateEquivalentReassessment(plan, result);
        return result;
    }

    private static GeneratedMathematicsItem EnsureFreshPromptShape(
        GeneratedMathematicsItem candidate,
        AssessmentBlueprint blueprint,
        IReadOnlySet<string> previousShapes,
        IReadOnlySet<string> selectedShapes,
        int attempt,
        int index)
    {
        var item = candidate.Item;
        var currentShape = WeaknessRecoveryEngine.NormalizePromptShape(item.Prompt);
        if (!previousShapes.Contains(currentShape) &&
            !selectedShapes.Contains(currentShape))
        {
            return candidate;
        }

        var originalPrompt = item.Prompt;
        var start = Math.Abs(unchecked(attempt * 31 + index * 17)) % ReviewedPromptPrefixes.Length;

        for (var offset = 0; offset < ReviewedPromptPrefixes.Length; offset++)
        {
            var prefix = ReviewedPromptPrefixes[(start + offset) % ReviewedPromptPrefixes.Length];
            var variedPrompt = $"{prefix} {originalPrompt}";
            var variedShape = WeaknessRecoveryEngine.NormalizePromptShape(variedPrompt);

            if (previousShapes.Contains(variedShape) ||
                selectedShapes.Contains(variedShape))
            {
                continue;
            }

            item.Prompt = variedPrompt;
            item.ExposureFingerprint = RecalculateExposureFingerprint(
                blueprint,
                candidate.OutcomeLink.LearningOutcomeId,
                item,
                variedPrompt);
            return candidate;
        }

        return candidate;
    }

    internal static string RecalculateExposureFingerprint(
        AssessmentBlueprint blueprint,
        Guid outcomeId,
        AssessmentItem item,
        string prompt)
    {
        ArgumentNullException.ThrowIfNull(blueprint);
        ArgumentNullException.ThrowIfNull(item);

        if (!Enum.TryParse<MathematicsGeneratorFamily>(
                item.GenerationFamily,
                out var family))
        {
            throw new InvalidOperationException(
                "Equivalent reassessment item has an invalid Mathematics generation family.");
        }

        var material = string.Join(
            '|',
            MathematicsQuestionGenerationEngine.GeneratorVersion,
            blueprint.SchoolId.ToString("N"),
            blueprint.CurriculumAdoptionId.ToString("N"),
            blueprint.CurriculumLevelKey.Trim(),
            blueprint.CurriculumTopicId?.ToString("N") ?? string.Empty,
            blueprint.CurriculumPedagogicalLessonId?.ToString("N") ?? string.Empty,
            outcomeId.ToString("N"),
            family,
            item.Difficulty,
            item.ItemType,
            item.GenerationParametersJson,
            prompt,
            item.CorrectAnswer);

        var bytes = SHA256.HashData(
            Encoding.UTF8.GetBytes(material));
        return Convert.ToHexString(bytes)
            .ToLowerInvariant();
    }

    private static bool IsFresh(
        GeneratedMathematicsItem candidate,
        IReadOnlySet<string> previousFingerprints,
        IReadOnlySet<string> previousShapes,
        IReadOnlySet<string> selectedFingerprints,
        IReadOnlySet<string> selectedShapes)
    {
        var fingerprint = candidate.Item.ExposureFingerprint;
        if (previousFingerprints.Contains(fingerprint) ||
            selectedFingerprints.Contains(fingerprint))
        {
            return false;
        }

        var promptShape = WeaknessRecoveryEngine.NormalizePromptShape(candidate.Item.Prompt);
        return !previousShapes.Contains(promptShape) &&
               !selectedShapes.Contains(promptShape);
    }
}
