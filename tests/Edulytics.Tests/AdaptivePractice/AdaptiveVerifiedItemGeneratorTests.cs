using Edulytics.Core.AdaptivePractice;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.AdaptivePractice;

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
}
