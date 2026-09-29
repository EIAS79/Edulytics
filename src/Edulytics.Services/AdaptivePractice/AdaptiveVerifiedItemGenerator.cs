using Edulytics.Core.AdaptivePractice;
using Edulytics.Core.Entities;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Services.AdaptivePractice;

/// <summary>
/// Safe V2 adapter over the existing Stage 18 exact Practice generator.
///
/// The adaptive engine chooses the target; this adapter does not invent new
/// mathematics. It narrows the already-approved lesson contract to exactly one
/// approved family and delegates generation/solver/verifier work to the current
/// production kernel.
/// </summary>
public sealed class AdaptiveVerifiedItemGenerator
{
    public AssessmentItem GenerateOne(
        Guid schoolId,
        Guid curriculumAdoptionId,
        Guid lessonId,
        Guid createdByUserId,
        LessonPracticeContract contract,
        AdaptiveNextItemDecision decision,
        int seed,
        IReadOnlyCollection<string> excludedExposureFingerprints,
        IReadOnlyCollection<string>? excludedSemanticIdentityKeys = null)
    {
        ArgumentNullException.ThrowIfNull(contract);
        ArgumentNullException.ThrowIfNull(decision);
        ArgumentNullException.ThrowIfNull(excludedExposureFingerprints);

        if (schoolId == Guid.Empty ||
            curriculumAdoptionId == Guid.Empty ||
            lessonId == Guid.Empty ||
            createdByUserId == Guid.Empty ||
            !string.Equals(
                contract.Readiness,
                LessonPracticeCapabilityResolver.ReadyVerified,
                StringComparison.Ordinal) ||
            !string.Equals(
                contract.SkillId,
                decision.TargetSkillId,
                StringComparison.Ordinal) ||
            !contract.AllowedQuestionFamilies.Contains(
                decision.TargetQuestionFamily,
                StringComparer.Ordinal))
        {
            throw new InvalidOperationException(
                "Adaptive Practice V2 generation requires a READY_VERIFIED " +
                "lesson contract matching the exact adaptive target.");
        }

        var narrowedContract =
            new Stage18PracticeSkillContract(
                contract.LessonCode,
                contract.SkillId,
                contract.Mechanic,
                [decision.TargetQuestionFamily])
            {
                SkillIds = contract.SkillIds
            };

        var requestedCognitive = CognitiveDifficultyFor(
            decision.TargetComplexityScore);
        var capability = ResolveTruthfulCapability(
            decision.TargetQuestionFamily,
            requestedCognitive);
        var effectiveCognitive = requestedCognitive < capability.MinimumDifficulty
            ? capability.MinimumDifficulty
            : requestedCognitive > capability.MaximumDifficulty
                ? capability.MaximumDifficulty
                : requestedCognitive;
        var difficulty = DifficultyFor(effectiveCognitive);
        var semanticExclusions =
            (excludedSemanticIdentityKeys ?? [])
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToHashSet(StringComparer.Ordinal);

        AssessmentItem? item = null;
        const int maxSemanticRetries = 32;

        // Prefer a mathematically new semantic instance. Some verified
        // identification/classification families have deliberately finite
        // semantic state (for example a theorem with a fixed answer). In that
        // case, after exhausting strong semantic freshness, fall back to a
        // still-fresh exposure rather than pausing the learner session. Exact
        // exposure repetition remains prohibited by the caller's recent
        // exposure window.
        for (var semanticPass = 0;
             semanticPass < 2 && item is null;
             semanticPass++)
        {
            var enforceSemanticFreshness =
                semanticPass == 0 &&
                semanticExclusions.Count > 0;

            for (var retry = 0;
                 retry < maxSemanticRetries && item is null;
                 retry++)
            {
                var roundSeed = unchecked(
                    (seed == 0 ? 1 : seed) ^
                    ((retry + 1 + semanticPass * maxSemanticRetries) * 104729));
                var preferredVariant = capability.VariantSlots[
                    Math.Abs(
                        roundSeed == int.MinValue
                            ? 0
                            : roundSeed) %
                    capability.VariantSlots.Count];

                AssessmentItem candidate;
                try
                {
                    candidate = new Stage18SkillContractPracticeEngine()
                        .Generate(
                            schoolId,
                            curriculumAdoptionId,
                            lessonId,
                            narrowedContract,
                            difficulty,
                            questionCount: 1,
                            roundSeed,
                            excludedExposureFingerprints,
                            createdByUserId,
                            preferredVariant)
                        .Single();
                }
                catch (ExactSkillQuestionPoolExhaustedException)
                {
                    continue;
                }

                // The exact generator may advance preferred variant slots while
                // avoiding an excluded fingerprint. Reject that candidate and
                // retry if it crossed out of the capability selected by the
                // adaptive difficulty contract.
                if (!IsTruthfulGeneratedForm(
                        candidate,
                        effectiveCognitive))
                {
                    continue;
                }

                var semanticIdentity =
                    AdaptivePracticeSemanticIdentity.Resolve(
                        candidate);

                if (enforceSemanticFreshness &&
                    semanticExclusions.Contains(
                        semanticIdentity))
                {
                    continue;
                }

                item = candidate;
            }
        }

        if (item is null)
        {
            throw new InvalidOperationException(
                "Adaptive Practice V2 exhausted verified freshness retries.");
        }

        if (!string.Equals(
                item.GenerationFamily,
                decision.TargetQuestionFamily,
                StringComparison.Ordinal) ||
            !Stage18SkillContractPracticeEngine.VerifyPersistedItem(
                narrowedContract,
                item))
        {
            throw new InvalidOperationException(
                "Adaptive Practice V2 rejected an item that did not preserve " +
                "the selected verified family/solver contract.");
        }

        return item;
    }

    private static PracticeCognitiveDifficulty CognitiveDifficultyFor(
        int complexity)
    {
        if (complexity is < 0 or >
            AdaptiveNextItemDecisionEngine.MaximumComplexityScore)
        {
            throw new ArgumentOutOfRangeException(
                nameof(complexity));
        }

        return complexity switch
        {
            <= 55 => PracticeCognitiveDifficulty.Standard,
            <= 67 => PracticeCognitiveDifficulty.Stretch,
            _ => PracticeCognitiveDifficulty.Challenge
        };
    }

    private static StudentPrivatePracticeDifficulty DifficultyFor(
        PracticeCognitiveDifficulty difficulty) =>
        difficulty switch
        {
            PracticeCognitiveDifficulty.Stretch =>
                StudentPrivatePracticeDifficulty.Stretch,
            PracticeCognitiveDifficulty.Challenge =>
                StudentPrivatePracticeDifficulty.Challenge,
            _ =>
                StudentPrivatePracticeDifficulty.MyLevel
        };

    private static PracticeQuestionFormCapability ResolveTruthfulCapability(
        string family,
        PracticeCognitiveDifficulty requested)
    {
        var capabilities =
            PracticeQuestionFormCapabilityRegistry.Resolve(family);

        var direct = capabilities
            .Where(x =>
                requested >= x.MinimumDifficulty &&
                requested <= x.MaximumDifficulty)
            .OrderByDescending(x => x.CognitiveOperation)
            .ThenByDescending(x => x.Form)
            .FirstOrDefault();

        if (direct is not null)
            return direct;

        var bounded = capabilities
            .Where(x => x.MaximumDifficulty <= requested)
            .OrderByDescending(x => x.MaximumDifficulty)
            .ThenByDescending(x => x.CognitiveOperation)
            .FirstOrDefault();

        return bounded ??
            capabilities
                .OrderBy(x => x.MinimumDifficulty)
                .FirstOrDefault() ??
            throw new InvalidOperationException(
                $"Adaptive Practice V2 has no truthful form capability for {family}.");
    }

    private static bool IsTruthfulGeneratedForm(
        AssessmentItem item,
        PracticeCognitiveDifficulty effectiveDifficulty)
    {
        if (string.IsNullOrWhiteSpace(item.GenerationFamily) ||
            string.IsNullOrWhiteSpace(item.GenerationParametersJson))
        {
            return false;
        }

        try
        {
            using var document =
                System.Text.Json.JsonDocument.Parse(
                    item.GenerationParametersJson);
            var parameters =
                document.RootElement.GetProperty("parameters");
            var variant = parameters.TryGetProperty(
                    "variant",
                    out var variantElement)
                ? variantElement.GetInt32()
                : 0;

            var actual =
                PracticeQuestionFormCapabilityRegistry.ResolveForVariant(
                    item.GenerationFamily,
                    variant);

            return actual is not null &&
                   effectiveDifficulty >= actual.MinimumDifficulty &&
                   effectiveDifficulty <= actual.MaximumDifficulty;
        }
        catch (
            Exception exception) when (
            exception is System.Text.Json.JsonException or
            InvalidOperationException)
        {
            return false;
        }
    }
}
