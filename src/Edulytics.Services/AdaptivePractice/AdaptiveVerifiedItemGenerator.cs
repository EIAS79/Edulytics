using Edulytics.Core.AdaptivePractice;
using Edulytics.Core.Entities;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Practice;

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
        IReadOnlyCollection<string> excludedExposureFingerprints)
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
        var preferredVariant = capability.VariantSlots[
            Math.Abs(seed == int.MinValue ? 0 : seed) %
            capability.VariantSlots.Count];

        var item = new Stage18SkillContractPracticeEngine()
            .Generate(
                schoolId,
                curriculumAdoptionId,
                lessonId,
                narrowedContract,
                difficulty,
                questionCount: 1,
                seed,
                excludedExposureFingerprints,
                createdByUserId,
                preferredVariant)
            .Single();

        ValidateTruthfulGeneratedForm(
            item,
            effectiveCognitive);

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

    private static void ValidateTruthfulGeneratedForm(
        AssessmentItem item,
        PracticeCognitiveDifficulty effectiveDifficulty)
    {
        if (string.IsNullOrWhiteSpace(item.GenerationFamily) ||
            string.IsNullOrWhiteSpace(item.GenerationParametersJson))
        {
            throw new InvalidOperationException(
                "Adaptive Practice V2 generated item is missing form provenance.");
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

            if (actual is null ||
                effectiveDifficulty < actual.MinimumDifficulty ||
                effectiveDifficulty > actual.MaximumDifficulty)
            {
                throw new InvalidOperationException(
                    "Adaptive Practice V2 rejected a falsely-labelled cognitive form.");
            }
        }
        catch (System.Text.Json.JsonException exception)
        {
            throw new InvalidOperationException(
                "Adaptive Practice V2 could not validate cognitive form provenance.",
                exception);
        }
    }
}
