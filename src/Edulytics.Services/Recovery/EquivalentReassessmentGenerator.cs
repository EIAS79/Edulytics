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

                var candidate = batch.Items[index];
                if (!IsFresh(
                        candidate,
                        previousFingerprints,
                        previousShapes,
                        selectedFingerprints))
                {
                    continue;
                }

                selected[index] = candidate;
                selectedFingerprints.Add(candidate.Item.ExposureFingerprint);
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

    private static bool IsFresh(
        GeneratedMathematicsItem candidate,
        IReadOnlySet<string> previousFingerprints,
        IReadOnlySet<string> previousShapes,
        IReadOnlySet<string> selectedFingerprints)
    {
        var fingerprint = candidate.Item.ExposureFingerprint;
        if (previousFingerprints.Contains(fingerprint) || selectedFingerprints.Contains(fingerprint))
        {
            return false;
        }

        var promptShape = WeaknessRecoveryEngine.NormalizePromptShape(candidate.Item.Prompt);
        return !previousShapes.Contains(promptShape);
    }
}
