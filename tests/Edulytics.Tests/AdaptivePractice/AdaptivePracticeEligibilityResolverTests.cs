using Edulytics.Core.AdaptivePractice;
using Edulytics.Core.Curriculum;
using Edulytics.Services.AdaptivePractice;

namespace Edulytics.Tests.MathematicsIntelligence.AdaptivePractice;

public sealed class AdaptivePracticeEligibilityResolverTests
{
    private const string ReadyPrimaryLesson =
        "PED:CAMBRIDGE-INTL-MATH:S2:2AS-1:BUILD";

    [Fact]
    public void OffPolicyAlwaysFailsClosed()
    {
        var level = PrimaryLevel(2);
        var resolver = new AdaptivePracticeEligibilityResolver(
            AdaptivePracticeV2Policy.Off);

        var decision = resolver.Resolve(
            Request(level.Key, ReadyPrimaryLesson));

        Assert.False(decision.IsEligible);
        Assert.False(decision.IsLearnerFacing);
        Assert.Equal(
            AdaptivePracticeEligibilityReasonCodes.FeatureDisabled,
            decision.ReasonCode);
    }

    [Fact]
    public void ExplicitPrimaryAllowListAndReadyVerifiedLessonAreRequired()
    {
        var level = PrimaryLevel(2);
        var schoolId = Guid.NewGuid();
        var resolver = new AdaptivePracticeEligibilityResolver(
            Policy(
                AdaptivePracticeV2Mode.Canary,
                [level.Key],
                [ReadyPrimaryLesson],
                [schoolId]));

        var decision = resolver.Resolve(
            new AdaptivePracticeEligibilityRequest(
                schoolId,
                level.Key,
                ReadyPrimaryLesson,
                IsMathematics: true));

        Assert.True(decision.IsEligible);
        Assert.True(decision.IsLearnerFacing);
        Assert.Equal(
            AdaptivePracticeEligibilityReasonCodes.Eligible,
            decision.ReasonCode);
        Assert.Equal(level.Key, decision.CurriculumLevelKey);
        Assert.Equal(2, decision.LogicalLevel);
        Assert.NotNull(decision.SkillId);
        Assert.NotNull(decision.ContractVersion);
    }

    [Fact]
    public void PrimaryLevelIsNotEnoughWithoutExplicitAllowList()
    {
        var level = PrimaryLevel(2);
        var schoolId = Guid.NewGuid();
        var resolver = new AdaptivePracticeEligibilityResolver(
            Policy(
                AdaptivePracticeV2Mode.Canary,
                [],
                [ReadyPrimaryLesson],
                [schoolId]));

        var decision = resolver.Resolve(
            new AdaptivePracticeEligibilityRequest(
                schoolId,
                level.Key,
                ReadyPrimaryLesson,
                IsMathematics: true));

        Assert.False(decision.IsEligible);
        Assert.Equal(
            AdaptivePracticeEligibilityReasonCodes.CurriculumLevelNotAllowed,
            decision.ReasonCode);
    }

    [Fact]
    public void CanaryWithoutExplicitSchoolAllowListFailsClosed()
    {
        var level = PrimaryLevel(2);
        var resolver = new AdaptivePracticeEligibilityResolver(
            Policy(
                AdaptivePracticeV2Mode.Canary,
                [level.Key],
                [ReadyPrimaryLesson]));

        var decision = resolver.Resolve(
            Request(level.Key, ReadyPrimaryLesson));

        Assert.False(decision.IsEligible);
        Assert.False(decision.IsLearnerFacing);
        Assert.Equal(
            AdaptivePracticeEligibilityReasonCodes.SchoolNotAllowed,
            decision.ReasonCode);
    }

    [Fact]
    public void CanaryWithoutExplicitLessonAllowListFailsClosed()
    {
        var level = PrimaryLevel(2);
        var schoolId = Guid.NewGuid();
        var resolver = new AdaptivePracticeEligibilityResolver(
            Policy(
                AdaptivePracticeV2Mode.Canary,
                [level.Key],
                [],
                [schoolId]));

        var decision = resolver.Resolve(
            new AdaptivePracticeEligibilityRequest(
                schoolId,
                level.Key,
                ReadyPrimaryLesson,
                IsMathematics: true));

        Assert.False(decision.IsEligible);
        Assert.False(decision.IsLearnerFacing);
        Assert.Equal(
            AdaptivePracticeEligibilityReasonCodes.LessonNotAllowed,
            decision.ReasonCode);
    }

    [Fact]
    public void SecondaryLevelCannotBeEnabledByConfigurationAlone()
    {
        var level = Level(7);
        var resolver = new AdaptivePracticeEligibilityResolver(
            Policy(
                AdaptivePracticeV2Mode.On,
                [level.Key],
                []));

        var decision = resolver.Resolve(
            Request(level.Key, ReadyPrimaryLesson));

        Assert.False(decision.IsEligible);
        Assert.Equal(
            AdaptivePracticeEligibilityReasonCodes.OutsidePrimaryRollout,
            decision.ReasonCode);
    }

    [Fact]
    public void MissingReadyVerifiedPracticeCapabilityFailsClosed()
    {
        var level = PrimaryLevel(2);
        var schoolId = Guid.NewGuid();
        var resolver = new AdaptivePracticeEligibilityResolver(
            Policy(
                AdaptivePracticeV2Mode.Canary,
                [level.Key],
                ["PED:DOES-NOT-EXIST"],
                [schoolId]));

        var decision = resolver.Resolve(
            new AdaptivePracticeEligibilityRequest(
                schoolId,
                level.Key,
                "PED:DOES-NOT-EXIST",
                IsMathematics: true));

        Assert.False(decision.IsEligible);
        Assert.Equal(
            AdaptivePracticeEligibilityReasonCodes.PracticeCapabilityMissing,
            decision.ReasonCode);
    }

    [Fact]
    public void NonMathematicsScopeFailsClosed()
    {
        var level = PrimaryLevel(2);
        var resolver = new AdaptivePracticeEligibilityResolver(
            Policy(
                AdaptivePracticeV2Mode.Canary,
                [level.Key],
                [ReadyPrimaryLesson]));

        var decision = resolver.Resolve(
            new AdaptivePracticeEligibilityRequest(
                Guid.NewGuid(),
                level.Key,
                ReadyPrimaryLesson,
                IsMathematics: false));

        Assert.False(decision.IsEligible);
        Assert.Equal(
            AdaptivePracticeEligibilityReasonCodes.InvalidScope,
            decision.ReasonCode);
    }

    [Fact]
    public void OnModeRetainsOptionalSchoolAndLessonScope()
    {
        var level = PrimaryLevel(2);
        var resolver = new AdaptivePracticeEligibilityResolver(
            Policy(
                AdaptivePracticeV2Mode.On,
                [level.Key],
                []));

        var decision = resolver.Resolve(
            Request(level.Key, ReadyPrimaryLesson));

        Assert.True(decision.IsEligible);
        Assert.True(decision.IsLearnerFacing);
        Assert.Equal(
            AdaptivePracticeEligibilityReasonCodes.Eligible,
            decision.ReasonCode);
    }

    [Fact]
    public void ShadowModeNeverBecomesLearnerFacing()
    {
        var level = PrimaryLevel(2);
        var resolver = new AdaptivePracticeEligibilityResolver(
            Policy(
                AdaptivePracticeV2Mode.Shadow,
                [level.Key],
                [ReadyPrimaryLesson]));

        var decision = resolver.Resolve(
            Request(level.Key, ReadyPrimaryLesson));

        Assert.True(decision.IsEligible);
        Assert.True(decision.IsShadow);
        Assert.False(decision.IsLearnerFacing);
    }

    private static AdaptivePracticeEligibilityRequest Request(
        string levelKey,
        string lessonCode) =>
        new(
            Guid.NewGuid(),
            levelKey,
            lessonCode,
            IsMathematics: true);

    private static AdaptivePracticeV2Policy Policy(
        AdaptivePracticeV2Mode mode,
        IReadOnlyCollection<string> levels,
        IReadOnlyCollection<string> lessons,
        IReadOnlyCollection<Guid>? schools = null) =>
        new(
            Enabled: true,
            Mode: mode,
            AllowedCurriculumLevelKeys:
                levels.ToHashSet(StringComparer.OrdinalIgnoreCase),
            AllowedLessonCodes:
                lessons.ToHashSet(StringComparer.Ordinal),
            AllowedSchoolIds:
                (schools ?? []).ToHashSet(),
            ShadowSamplingPercentage: 100,
            MaxLessonQuestions: 8,
            EnableMisconceptionLoop: false,
            EnableDirectNextSteps: false,
            EnableQuestionLog: false,
            EnableLiveClassroom: false,
            EnableDiagnosticV2: false);

    private static CurriculumLevelIdentity PrimaryLevel(int logicalLevel) =>
        Level(logicalLevel);

    private static CurriculumLevelIdentity Level(int logicalLevel) =>
        CurriculumLevelIdentityRegistry.All.First(
            x => x.LogicalLevel == logicalLevel);
}
