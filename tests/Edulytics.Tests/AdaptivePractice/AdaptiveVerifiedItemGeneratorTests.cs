using Edulytics.Core.AdaptivePractice;
using Edulytics.Core.Enums;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.AdaptivePractice;
using Edulytics.Services.Practice;

namespace Edulytics.Tests.MathematicsIntelligence.AdaptivePractice;

public sealed class AdaptiveVerifiedItemGeneratorTests
{
    [Fact]
    public void GeneratesExactlySelectedReadyVerifiedFamily()
    {
        const string lessonCode =
            "PED:CAMBRIDGE-INTL-MATH:S5:5F-2:BUILD";

        Assert.True(
            LessonPracticeCapabilityResolver.TryResolve(
                lessonCode,
                out var contract));
        Assert.NotNull(contract);

        const string family =
            "fractions.equivalent.missing_value";

        var decision = new AdaptiveNextItemDecision(
            TargetSkillId: contract!.SkillId,
            TargetComplexityScore: 42,
            TargetQuestionFamily: family,
            TargetRepresentation: "symbolic",
            MisconceptionFocusId: null,
            ReasonCode:
                AdaptivePracticeDecisionReasonCodes.SessionBaseline,
            RequiresFreshExposure: true,
            RemediationLockActive: false,
            ConfirmationRequired: false,
            IsIndependentConfirmation: false,
            ProgressionEligible: false,
            EngineVersion:
                AdaptivePracticeV2Versions.EngineVersion,
            PolicyVersion:
                AdaptivePracticeV2Versions.PolicyVersion);

        var item = new AdaptiveVerifiedItemGenerator()
            .GenerateOne(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                contract,
                decision,
                seed: 731,
                excludedExposureFingerprints: []);

        Assert.Equal(family, item.GenerationFamily);
        Assert.False(
            string.IsNullOrWhiteSpace(
                item.ExposureFingerprint));
        Assert.Contains(
            "solverVerified",
            item.ValidationMetadataJson ?? string.Empty,
            StringComparison.Ordinal);
    }

    [Fact]
    public void FiniteSemanticFamily_SustainsMaximumAdaptiveBudgetWithRecentFreshness()
    {
        const string family =
            "probability.theoretical.two_coins_exactly_one";

        var contract =
            LessonPracticeContractRegistry.All.First(
                x => x.AllowedQuestionFamilies.Contains(
                    family,
                    StringComparer.Ordinal));

        var generator =
            new AdaptiveVerifiedItemGenerator();
        var exposures =
            new List<string>();
        var semantics =
            new List<string>();

        for (var sequence = 1;
             sequence <= AdaptivePracticeV2Behavior.MaximumSessionItems;
             sequence++)
        {
            var decision =
                Decision(
                    contract,
                    family,
                    complexity: 42);

            var item =
                generator.GenerateOne(
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    contract,
                    decision,
                    seed:
                        unchecked(
                            20260929 +
                            sequence * 997),
                    excludedExposureFingerprints:
                        exposures,
                    excludedSemanticIdentityKeys:
                        semantics);

            Assert.Equal(
                family,
                item.GenerationFamily);
            Assert.DoesNotContain(
                item.ExposureFingerprint,
                exposures,
                StringComparer.Ordinal);

            RememberRecent(
                exposures,
                item.ExposureFingerprint,
                AdaptivePracticeV2Behavior
                    .RecentExposureFreshnessWindow);

            RememberRecent(
                semantics,
                AdaptivePracticeSemanticIdentity.Resolve(
                    item),
                AdaptivePracticeV2Behavior
                    .RecentSemanticFreshnessWindow);
        }
    }

    [Fact]
    public void CapabilityPartitionedFamily_RetriesWithoutFalseDifficultyLabel()
    {
        const string family =
            "supporting.circle.angle_semicircle";

        var contract =
            LessonPracticeContractRegistry.All.First(
                x => x.AllowedQuestionFamilies.Contains(
                    family,
                    StringComparer.Ordinal));

        var generator =
            new AdaptiveVerifiedItemGenerator();
        var exposures =
            new List<string>();
        var semantics =
            new List<string>();

        for (var sequence = 1;
             sequence <= AdaptivePracticeV2Behavior.MaximumSessionItems;
             sequence++)
        {
            var decision =
                Decision(
                    contract,
                    family,
                    AdaptiveNextItemDecisionEngine
                        .MaximumComplexityScore);

            var item =
                generator.GenerateOne(
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    contract,
                    decision,
                    seed:
                        unchecked(
                            60260929 +
                            sequence * 1301),
                    excludedExposureFingerprints:
                        exposures,
                    excludedSemanticIdentityKeys:
                        semantics);

            Assert.Equal(
                family,
                item.GenerationFamily);

            RememberRecent(
                exposures,
                item.ExposureFingerprint,
                AdaptivePracticeV2Behavior
                    .RecentExposureFreshnessWindow);

            RememberRecent(
                semantics,
                AdaptivePracticeSemanticIdentity.Resolve(
                    item),
                AdaptivePracticeV2Behavior
                    .RecentSemanticFreshnessWindow);
        }
    }

    [Fact]
    public void RoundingFamilyProvidesGenuineStandardStretchAndChallengeForms()
    {
        const string family =
            "supporting.number.rounding";

        var contract =
            LessonPracticeContractRegistry.All.First(
                x => x.AllowedQuestionFamilies.Contains(
                    family,
                    StringComparer.Ordinal));

        var generator =
            new AdaptiveVerifiedItemGenerator();

        var standardDecision =
            generator.NormalizeDecisionToTruthfulCapability(
                Decision(
                    contract,
                    family,
                    complexity: 42),
                currentComplexityScore: 42);

        var stretchDecision =
            generator.NormalizeDecisionToTruthfulCapability(
                Decision(
                    contract,
                    family,
                    complexity: 66) with
                {
                    ReasonCode =
                        AdaptivePracticeDecisionReasonCodes
                            .ComplexityProgress,
                    ProgressionEligible = true
                },
                currentComplexityScore: 54);

        var challengeDecision =
            generator.NormalizeDecisionToTruthfulCapability(
                Decision(
                    contract,
                    family,
                    complexity: 90) with
                {
                    ReasonCode =
                        AdaptivePracticeDecisionReasonCodes
                            .ComplexityProgress,
                    ProgressionEligible = true
                },
                currentComplexityScore: 66);

        var standard =
            generator.GenerateOne(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                contract,
                standardDecision,
                seed: 1101,
                excludedExposureFingerprints: []);

        var stretch =
            generator.GenerateOne(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                contract,
                stretchDecision,
                seed: 2202,
                excludedExposureFingerprints: []);

        var challenge =
            generator.GenerateOne(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                contract,
                challengeDecision,
                seed: 3303,
                excludedExposureFingerprints: []);

        Assert.Equal(
            AssessmentItemDifficulty.Medium,
            standard.Difficulty);
        Assert.Equal(
            AssessmentItemDifficulty.Challenging,
            stretch.Difficulty);
        Assert.Equal(
            AssessmentItemDifficulty.Challenging,
            challenge.Difficulty);

        Assert.Contains(
            "Round ",
            standard.Prompt,
            StringComparison.Ordinal);
        Assert.Contains(
            "possible whole number",
            stretch.Prompt,
            StringComparison.Ordinal);
        Assert.Contains(
            "nearest 100 and to",
            challenge.Prompt,
            StringComparison.Ordinal);

        Assert.True(
            Stage18SkillContractPracticeEngine.VerifyPersistedItem(
                new Stage18PracticeSkillContract(
                    contract.LessonCode,
                    contract.SkillId,
                    contract.Mechanic,
                    [family])
                {
                    SkillIds = contract.SkillIds
                },
                standard));
        Assert.True(
            Stage18SkillContractPracticeEngine.VerifyPersistedItem(
                new Stage18PracticeSkillContract(
                    contract.LessonCode,
                    contract.SkillId,
                    contract.Mechanic,
                    [family])
                {
                    SkillIds = contract.SkillIds
                },
                stretch));
        Assert.True(
            Stage18SkillContractPracticeEngine.VerifyPersistedItem(
                new Stage18PracticeSkillContract(
                    contract.LessonCode,
                    contract.SkillId,
                    contract.Mechanic,
                    [family])
                {
                    SkillIds = contract.SkillIds
                },
                challenge));
    }

    [Fact]
    public void StandardOnlyFamilyCapsInflatedAdaptiveComplexityHonestly()
    {
        const string family =
            "fractions.equivalent.missing_value";

        var contract =
            LessonPracticeContractRegistry.All.First(
                x => x.AllowedQuestionFamilies.Contains(
                    family,
                    StringComparer.Ordinal));

        var generator =
            new AdaptiveVerifiedItemGenerator();

        var requested =
            Decision(
                contract,
                family,
                AdaptiveNextItemDecisionEngine
                    .MaximumComplexityScore) with
            {
                ReasonCode =
                    AdaptivePracticeDecisionReasonCodes
                        .ComplexityProgress,
                ProgressionEligible = true
            };

        var normalized =
            generator.NormalizeDecisionToTruthfulCapability(
                requested,
                currentComplexityScore: 55);

        Assert.Equal(
            55,
            normalized.TargetComplexityScore);
        Assert.Equal(
            AdaptivePracticeDecisionReasonCodes
                .ComplexityConsolidate,
            normalized.ReasonCode);
        Assert.False(
            normalized.ProgressionEligible);

        var item =
            generator.GenerateOne(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                contract,
                normalized,
                seed: 20260930,
                excludedExposureFingerprints: []);

        Assert.Equal(
            family,
            item.GenerationFamily);
    }

    [Fact]
    public void ChallengeCapableFamilyPreservesChallengeComplexity()
    {
        const string family =
            "supporting.geometry.shape_dimension";

        var contract =
            LessonPracticeContractRegistry.All.First(
                x => x.AllowedQuestionFamilies.Contains(
                    family,
                    StringComparer.Ordinal));

        var generator =
            new AdaptiveVerifiedItemGenerator();

        var requested =
            Decision(
                contract,
                family,
                AdaptiveNextItemDecisionEngine
                    .MaximumComplexityScore) with
            {
                ReasonCode =
                    AdaptivePracticeDecisionReasonCodes
                        .ComplexityProgress,
                ProgressionEligible = true
            };

        var normalized =
            generator.NormalizeDecisionToTruthfulCapability(
                requested,
                currentComplexityScore: 67);

        Assert.Equal(
            AdaptiveNextItemDecisionEngine.MaximumComplexityScore,
            normalized.TargetComplexityScore);
        Assert.Equal(
            AdaptivePracticeDecisionReasonCodes
                .ComplexityProgress,
            normalized.ReasonCode);
        Assert.True(
            normalized.ProgressionEligible);
    }

    [Fact]
    public void RejectsFamilyOutsideLessonContract()
    {
        const string lessonCode =
            "PED:CAMBRIDGE-INTL-MATH:S5:5F-2:BUILD";

        Assert.True(
            LessonPracticeCapabilityResolver.TryResolve(
                lessonCode,
                out var contract));
        Assert.NotNull(contract);

        var decision = new AdaptiveNextItemDecision(
            contract!.SkillId,
            42,
            "ratio.unit_rate.direct",
            null,
            null,
            AdaptivePracticeDecisionReasonCodes.SessionBaseline,
            true,
            false,
            false,
            false,
            false,
            AdaptivePracticeV2Versions.EngineVersion,
            AdaptivePracticeV2Versions.PolicyVersion);

        Assert.Throws<InvalidOperationException>(() =>
            new AdaptiveVerifiedItemGenerator()
                .GenerateOne(
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    Guid.NewGuid(),
                    contract,
                    decision,
                    seed: 731,
                    excludedExposureFingerprints: []));
    }
    private static AdaptiveNextItemDecision Decision(
        LessonPracticeContract contract,
        string family,
        int complexity) =>
        new(
            TargetSkillId:
                contract.SkillId,
            TargetComplexityScore:
                complexity,
            TargetQuestionFamily:
                family,
            TargetRepresentation:
                "symbolic",
            MisconceptionFocusId:
                null,
            ReasonCode:
                AdaptivePracticeDecisionReasonCodes
                    .ComplexityConsolidate,
            RequiresFreshExposure:
                true,
            RemediationLockActive:
                false,
            ConfirmationRequired:
                false,
            IsIndependentConfirmation:
                false,
            ProgressionEligible:
                false,
            EngineVersion:
                AdaptivePracticeV2Versions.EngineVersion,
            PolicyVersion:
                AdaptivePracticeV2Versions.PolicyVersion);

    private static void RememberRecent(
        List<string> values,
        string? value,
        int limit)
    {
        if (string.IsNullOrWhiteSpace(value))
            return;

        values.RemoveAll(
            x => string.Equals(
                x,
                value,
                StringComparison.Ordinal));
        values.Insert(0, value);

        if (values.Count > limit)
        {
            values.RemoveRange(
                limit,
                values.Count - limit);
        }
    }

}
