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

        var difficulty = DifficultyFor(
            decision.TargetComplexityScore);

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
                createdByUserId)
            .Single();

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

    private static StudentPrivatePracticeDifficulty DifficultyFor(
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
            <= 55 => StudentPrivatePracticeDifficulty.MyLevel,
            <= 67 => StudentPrivatePracticeDifficulty.Stretch,
            _ => StudentPrivatePracticeDifficulty.Challenge
        };
    }
}
