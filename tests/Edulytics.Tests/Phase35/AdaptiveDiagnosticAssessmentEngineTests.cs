using Edulytics.Core.AdaptiveAssessment;
using Edulytics.Core.Analytics;
using Edulytics.Core.AssessmentIntelligence;
using Edulytics.Core.Enums;
using Edulytics.Services.AdaptiveAssessment;

namespace Edulytics.Tests.Phase35;

public sealed class AdaptiveDiagnosticAssessmentEngineTests
{
    private readonly AdaptiveDiagnosticAssessmentEngine _engine = new();

    [Fact]
    public void MissingProfile_StartsDiagnosticCalibrationAtMediumDifficulty()
    {
        var scope = Scope();
        var decision = _engine.DecideNext(new AdaptiveAssessmentRequest(
            scope.SchoolId,
            scope.AdoptionId,
            "US-G7",
            [scope.OutcomeA, scope.OutcomeB],
            null,
            AssessmentPurpose.StudentPersonalTest,
            []));

        Assert.Equal(AdaptiveAssessmentMode.Diagnostic, decision.Mode);
        Assert.Equal(AssessmentItemDifficulty.Medium, decision.NextDifficulty);
        Assert.True(decision.RequiresFreshExposure);
        Assert.Equal("phase35-v1", decision.FormulaVersion);
    }

    [Fact]
    public void LowConfidenceProfile_RemainsDiagnosticUntilCalibrated()
    {
        var scope = Scope();
        var profile = Profile(scope, confidenceA: 30m, evidenceA: 3, confidenceB: 90m, evidenceB: 5);

        var decision = _engine.DecideNext(new AdaptiveAssessmentRequest(
            scope.SchoolId,
            scope.AdoptionId,
            "US-G7",
            [scope.OutcomeA, scope.OutcomeB],
            profile,
            AssessmentPurpose.Diagnostic,
            []));

        Assert.Equal(AdaptiveAssessmentMode.Diagnostic, decision.Mode);
        Assert.Equal(scope.OutcomeA, decision.TargetLearningOutcomeId);
    }

    [Fact]
    public void IncorrectResponse_ReducesDifficultyAndCannotInflateMasteryCredit()
    {
        var scope = Scope();
        var profile = Profile(scope, confidenceA: 90m, evidenceA: 6, confidenceB: 90m, evidenceB: 6);

        var decision = _engine.DecideNext(new AdaptiveAssessmentRequest(
            scope.SchoolId,
            scope.AdoptionId,
            "US-G7",
            [scope.OutcomeA, scope.OutcomeB],
            profile,
            AssessmentPurpose.StudentPersonalTest,
            [new AdaptiveResponseEvidence(scope.OutcomeA, AssessmentItemDifficulty.Medium, false, 0m, 1)]));

        Assert.Equal(AdaptiveAssessmentMode.Adaptive, decision.Mode);
        Assert.Equal(scope.OutcomeA, decision.TargetLearningOutcomeId);
        Assert.Equal(AssessmentItemDifficulty.Easy, decision.NextDifficulty);
        Assert.True(decision.DifficultyReduced);
        Assert.Equal(0.55m, decision.EvidenceCreditMultiplier);
        Assert.True(decision.RequiresFreshExposure);
    }

    [Fact]
    public void CorrectResponse_IncreasesDifficultyWithinBounds()
    {
        var scope = Scope();
        var profile = Profile(scope, confidenceA: 90m, evidenceA: 6, confidenceB: 90m, evidenceB: 6);

        var decision = _engine.DecideNext(new AdaptiveAssessmentRequest(
            scope.SchoolId,
            scope.AdoptionId,
            "US-G7",
            [scope.OutcomeA, scope.OutcomeB],
            profile,
            AssessmentPurpose.Practice,
            [new AdaptiveResponseEvidence(scope.OutcomeA, AssessmentItemDifficulty.Easy, true, 100m, 1)]));

        Assert.Equal(AssessmentItemDifficulty.Medium, decision.NextDifficulty);
        Assert.False(decision.DifficultyReduced);
        Assert.Equal(0.80m, decision.EvidenceCreditMultiplier);
    }

    [Fact]
    public void FormalTeacherAssessment_IsNotAdaptedAfterReviewedBlueprint()
    {
        var scope = Scope();
        var profile = Profile(scope, confidenceA: 90m, evidenceA: 6, confidenceB: 90m, evidenceB: 6);

        Assert.Throws<InvalidOperationException>(() => _engine.DecideNext(
            new AdaptiveAssessmentRequest(
                scope.SchoolId,
                scope.AdoptionId,
                "US-G7",
                [scope.OutcomeA, scope.OutcomeB],
                profile,
                AssessmentPurpose.TeacherAssessment,
                [])));
    }

    private static StudentLearningProfile Profile(
        TestScope scope,
        decimal confidenceA,
        int evidenceA,
        decimal confidenceB,
        int evidenceB)
    {
        var rows = new[]
        {
            new StudentOutcomeLearningProfile(
                scope.OutcomeA,
                "A",
                "Outcome A",
                58m,
                MasteryBand.Developing,
                evidenceA,
                confidenceA,
                DateTime.UtcNow.AddMinutes(-2),
                2,
                2,
                2,
                4m,
                "phase31-v1"),
            new StudentOutcomeLearningProfile(
                scope.OutcomeB,
                "B",
                "Outcome B",
                72m,
                MasteryBand.Secure,
                evidenceB,
                confidenceB,
                DateTime.UtcNow.AddMinutes(-1),
                2,
                2,
                2,
                4m,
                "phase31-v1")
        };

        return new StudentLearningProfile(
            scope.SchoolId,
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            scope.AdoptionId,
            65m,
            MasteryBand.Developing,
            evidenceA + evidenceB,
            Math.Min(confidenceA, confidenceB),
            DateTime.UtcNow,
            rows,
            "phase31-v1");
    }

    private static TestScope Scope() =>
        new(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

    private sealed record TestScope(
        Guid SchoolId,
        Guid AdoptionId,
        Guid OutcomeA,
        Guid OutcomeB);
}
