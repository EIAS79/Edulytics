using Edulytics.Core.AdaptivePractice;
using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class AdaptivePracticePhase9RolloutTests
{
    [Fact]
    public void RolloutPlan_IsInternallyValid()
    {
        AdaptivePrimaryRolloutPlan.Validate();
    }

    [Fact]
    public void ExistingInternalCanary_RemainsValid()
    {
        Assert.True(
            AdaptivePrimaryRolloutPlan.IsValidCanaryScope(
                ["US-CCSS-MATH:L05:SHARED"],
                ["PED:US-CCSS-MATH:G4:U02:L07"]));
    }

    [Fact]
    public void Phase9_Stages2Through6_AreApprovedAsCompletePairs()
    {
        var phase9 = AdaptivePrimaryRolloutPlan.All
            .Where(x => x.Wave.StartsWith(
                "PHASE9_STAGE",
                StringComparison.Ordinal))
            .ToArray();

        Assert.Equal(5, phase9.Length);

        var logicalLevels = phase9
            .Select(x =>
                CurriculumLevelIdentityRegistry
                    .Find(x.CurriculumLevelKey)!)
            .Select(x => x.LogicalLevel)
            .OrderBy(x => x)
            .ToArray();

        Assert.Equal([2, 3, 4, 5, 6], logicalLevels);

        Assert.True(
            AdaptivePrimaryRolloutPlan.IsValidCanaryScope(
                phase9.Select(x => x.CurriculumLevelKey),
                phase9.Select(x => x.LessonCode)));
    }

    [Fact]
    public void EveryPhase9Lesson_IsReadyVerified()
    {
        foreach (var entry in AdaptivePrimaryRolloutPlan.All)
        {
            Assert.True(
                LessonPracticeCapabilityResolver.TryResolve(
                    entry.LessonCode,
                    out var contract));
            Assert.NotNull(contract);
            Assert.Equal(
                LessonPracticeCapabilityResolver.ReadyVerified,
                contract!.Readiness);
        }
    }

    [Fact]
    public void CanaryScope_RejectsUnreviewedLevelOrLesson()
    {
        Assert.False(
            AdaptivePrimaryRolloutPlan.IsValidCanaryScope(
                ["CAMBRIDGE-INTL-MATH:L07:SHARED"],
                ["PED:CAMBRIDGE-INTL-MATH:S6:6F-3:APPLY"]));

        Assert.False(
            AdaptivePrimaryRolloutPlan.IsValidCanaryScope(
                ["CAMBRIDGE-INTL-MATH:L06:SHARED"],
                ["PED:DOES-NOT-EXIST"]));
    }

    [Fact]
    public void CanaryScope_RejectsPartialOrMismatchedWave()
    {
        Assert.False(
            AdaptivePrimaryRolloutPlan.IsValidCanaryScope(
                ["CAMBRIDGE-INTL-MATH:L02:SHARED"],
                ["PED:CAMBRIDGE-INTL-MATH:S3:3AS-2:APPLY"]));

        Assert.False(
            AdaptivePrimaryRolloutPlan.IsValidCanaryScope(
                [
                    "CAMBRIDGE-INTL-MATH:L02:SHARED",
                    "CAMBRIDGE-INTL-MATH:L03:SHARED"
                ],
                ["PED:CAMBRIDGE-INTL-MATH:S2:2AS-1:APPLY"]));
    }

    [Fact]
    public void Phase9_DoesNotAuthorizeSecondaryOrAdvancedLevels()
    {
        Assert.DoesNotContain(
            AdaptivePrimaryRolloutPlan.All,
            entry =>
                CurriculumLevelIdentityRegistry
                    .Find(entry.CurriculumLevelKey)!
                    .LogicalLevel > 6);

        Assert.DoesNotContain(
            AdaptivePrimaryRolloutPlan.All,
            entry =>
                entry.LessonCode.Contains(
                    "IGCSE",
                    StringComparison.OrdinalIgnoreCase) ||
                entry.LessonCode.Contains(
                    "A-LEVEL",
                    StringComparison.OrdinalIgnoreCase));
    }
}
