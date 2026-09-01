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
    private const int MaxGenerationAttempts = 64;

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

        for (var attempt = 0; attempt < MaxGenerationAttempts; attempt++)
        {
            var batch = _generator.Generate(new MathematicsGenerationRequest(
                plan.EquivalentReassessmentBlueprint,
                outcomeProfiles,
                unchecked(seed + attempt)));

            if (!IsFresh(batch, previousFingerprints, previousShapes))
            {
                continue;
            }

            // Do not weaken or bypass the Phase 36 acceptance contract. Once a
            // fresh candidate is found, the existing validator remains authoritative
            // for scope, difficulty, coverage, exposure and equivalence semantics.
            _recovery.ValidateEquivalentReassessment(plan, batch);
            return batch;
        }

        throw new InvalidOperationException(
            "Unable to generate a fresh equivalent reassessment within the retry budget.");
    }

    private static bool IsFresh(
        MathematicsGenerationBatch batch,
        IReadOnlySet<string> previousFingerprints,
        IReadOnlySet<string> previousShapes)
    {
        var generatedFingerprints = new HashSet<string>(StringComparer.Ordinal);

        foreach (var generated in batch.Items)
        {
            if (previousFingerprints.Contains(generated.Item.ExposureFingerprint)
                || !generatedFingerprints.Add(generated.Item.ExposureFingerprint))
            {
                return false;
            }

            var promptShape = WeaknessRecoveryEngine.NormalizePromptShape(generated.Item.Prompt);
            if (previousShapes.Contains(promptShape))
            {
                return false;
            }
        }

        return true;
    }
}
