using Edulytics.Core.AdaptiveAssessment;
using Edulytics.Core.Analytics;
using Edulytics.Core.AssessmentIntelligence;
using Edulytics.Core.Enums;
using Edulytics.Services.AdaptiveAssessment;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class Stage20DiagnosticAdaptiveMigrationTests
{
    private readonly AdaptiveDiagnosticAssessmentEngine engine = new();

    [Fact]
    public void ExactOutcomeRequiresCompleteMathematicsAwareState()
    {
        var scope = Scope();
        var profile = Profile(
            scope,
            ("CCSS:4.NF.A.1", 58m, 6, 90m));

        var request = new AdaptiveAssessmentRequest(
            scope.SchoolId,
            scope.AdoptionId,
            "US-G4",
            [scope.OutcomeA],
            profile,
            AssessmentPurpose.StudentPersonalTest,
            []);

        var error = Assert.Throws<InvalidOperationException>(
            () => engine.DecideNext(request));

        Assert.Contains(
            "complete mathematics-aware adaptive state",
            error.Message,
            StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void MathematicsAwareDecisionConsumesAllFiveSignalsAndTargetsActiveMisconception()
    {
        var scope = Scope();
        var profile = Profile(
            scope,
            ("CCSS:4.NF.A.1", 58m, 6, 90m));

        var state = new AdaptiveMathematicsSkillState(
            scope.OutcomeA,
            "CCSS:4.NF.A.1",
            "fractions.equivalent",
            SkillMastery: 0.58m,
            PrerequisiteMastery: 0.40m,
            CurrentComplexityScore: 60,
            RecentSuccessfulItems: 2,
            MisconceptionHistory:
            [
                new AdaptiveMisconceptionEvidence(
                    "fraction.reciprocal",
                    Count: 3,
                    LastObservedSequence: 8,
                    QuestionFamily: "fractions.equivalent.reduce_common_factor")
            ],
            RepresentationFluency:
            [
                new AdaptiveRepresentationFluency(
                    "symbolic",
                    0.80m,
                    [
                        "fractions.equivalent.missing_value",
                        "fractions.equivalent.recognize"
                    ]),
                new AdaptiveRepresentationFluency(
                    "number-line",
                    0.30m,
                    ["fractions.equivalent.number_line"]),
                new AdaptiveRepresentationFluency(
                    "reduction",
                    0.40m,
                    ["fractions.equivalent.reduce_common_factor"])
            ]);

        var decision = engine.DecideNext(new AdaptiveAssessmentRequest(
            scope.SchoolId,
            scope.AdoptionId,
            "US-G4",
            [scope.OutcomeA],
            profile,
            AssessmentPurpose.StudentPersonalTest,
            [
                new AdaptiveResponseEvidence(
                    scope.OutcomeA,
                    AssessmentItemDifficulty.Challenging,
                    IsCorrect: false,
                    ScorePercentage: 0m,
                    Sequence: 9,
                    MathematicsComplexityScore: 60,
                    QuestionFamily: "fractions.equivalent.reduce_common_factor",
                    Representation: "reduction",
                    MisconceptionId: "fraction.reciprocal")
            ],
            [state]));

        Assert.True(decision.MathematicsAware);
        Assert.Equal(
            AdaptiveDiagnosticAssessmentEngine.MathematicsEngineVersion,
            decision.FormulaVersion);
        Assert.Equal(scope.OutcomeA, decision.TargetLearningOutcomeId);
        Assert.Equal("fractions.equivalent", decision.TargetSkillId);
        Assert.Equal(48, decision.TargetComplexityScore);
        Assert.Equal(AssessmentItemDifficulty.Medium, decision.NextDifficulty);
        Assert.True(decision.DifficultyReduced);
        Assert.Equal("reduction", decision.TargetRepresentation);
        Assert.Equal(
            "fractions.equivalent.reduce_common_factor",
            decision.TargetQuestionFamily);
        Assert.Equal("fraction.reciprocal", decision.MisconceptionFocusId);
        Assert.Contains("Skill mastery=0.58", decision.Reason, StringComparison.Ordinal);
        Assert.Contains("prerequisite mastery=0.40", decision.Reason, StringComparison.Ordinal);
        Assert.Contains("representation fluency=0.50", decision.Reason, StringComparison.Ordinal);
        Assert.Contains("Recent misconception count=3", decision.Reason, StringComparison.Ordinal);
        Assert.Contains("complexity moves from 60 to 48", decision.Reason, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void WeakestRepresentationDrivesFamilyWhenThereIsNoActiveMisconception()
    {
        var scope = Scope();
        var profile = Profile(
            scope,
            ("CCSS:3.NF.A.3", 90m, 8, 90m));

        var state = new AdaptiveMathematicsSkillState(
            scope.OutcomeA,
            "CCSS:3.NF.A.3",
            "fractions.equivalent",
            SkillMastery: 0.90m,
            PrerequisiteMastery: 0.90m,
            CurrentComplexityScore: 42,
            RecentSuccessfulItems: 8,
            MisconceptionHistory: [],
            RepresentationFluency:
            [
                new AdaptiveRepresentationFluency(
                    "symbolic",
                    0.90m,
                    [
                        "fractions.equivalent.missing_value",
                        "fractions.equivalent.generate_multiple"
                    ]),
                new AdaptiveRepresentationFluency(
                    "number-line",
                    0.20m,
                    ["fractions.equivalent.number_line"])
            ]);

        var decision = engine.DecideNext(new AdaptiveAssessmentRequest(
            scope.SchoolId,
            scope.AdoptionId,
            "US-G3",
            [scope.OutcomeA],
            profile,
            AssessmentPurpose.Practice,
            [
                new AdaptiveResponseEvidence(
                    scope.OutcomeA,
                    AssessmentItemDifficulty.Medium,
                    IsCorrect: true,
                    ScorePercentage: 100m,
                    Sequence: 1,
                    MathematicsComplexityScore: 42)
            ],
            [state]));

        Assert.True(decision.MathematicsAware);
        Assert.Equal("number-line", decision.TargetRepresentation);
        Assert.Equal(
            "fractions.equivalent.number_line",
            decision.TargetQuestionFamily);
        Assert.Equal(54, decision.TargetComplexityScore);
        Assert.Equal(AssessmentItemDifficulty.Medium, decision.NextDifficulty);
        Assert.Null(decision.MisconceptionFocusId);
    }

    [Fact]
    public void MixedExactAndNonExactOutcomeScopeFailsClosed()
    {
        var scope = Scope();
        var profile = Profile(
            scope,
            ("CCSS:4.NF.A.1", 58m, 6, 90m),
            ("UNMIGRATED:OUTCOME", 72m, 6, 90m));

        var exactState = BasicState(
            scope.OutcomeA,
            "CCSS:4.NF.A.1",
            "fractions.equivalent",
            0.58m);

        var error = Assert.Throws<InvalidOperationException>(() =>
            engine.DecideNext(new AdaptiveAssessmentRequest(
                scope.SchoolId,
                scope.AdoptionId,
                "US-G4",
                [scope.OutcomeA, scope.OutcomeB],
                profile,
                AssessmentPurpose.StudentPersonalTest,
                [],
                [exactState])));

        Assert.Contains(
            "exact and non-exact Outcomes are mixed",
            error.Message,
            StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void DisallowedRepresentationFamilyFailsClosed()
    {
        var scope = Scope();
        var profile = Profile(
            scope,
            ("CCSS:6.RP.A.2", 70m, 6, 90m));

        var state = new AdaptiveMathematicsSkillState(
            scope.OutcomeA,
            "CCSS:6.RP.A.2",
            "ratio.unit_rate",
            0.70m,
            0.65m,
            42,
            4,
            [],
            [
                new AdaptiveRepresentationFluency(
                    "invalid",
                    0.40m,
                    ["fractions.equivalent.number_line"])
            ]);

        Assert.Throws<InvalidOperationException>(() =>
            engine.DecideNext(new AdaptiveAssessmentRequest(
                scope.SchoolId,
                scope.AdoptionId,
                "US-G6",
                [scope.OutcomeA],
                profile,
                AssessmentPurpose.StudentPersonalTest,
                [],
                [state])));
    }

    [Fact]
    public void SkillMasteryMustAgreeWithAuthoritativeMasteryProfile()
    {
        var scope = Scope();
        var profile = Profile(
            scope,
            ("CCSS:6.RP.A.3", 70m, 6, 90m));

        var state = BasicState(
            scope.OutcomeA,
            "CCSS:6.RP.A.3",
            "ratio.unit_rate",
            skillMastery: 0.20m);

        var error = Assert.Throws<InvalidOperationException>(() =>
            engine.DecideNext(new AdaptiveAssessmentRequest(
                scope.SchoolId,
                scope.AdoptionId,
                "US-G6",
                [scope.OutcomeA],
                profile,
                AssessmentPurpose.StudentPersonalTest,
                [],
                [state])));

        Assert.Contains(
            "authoritative student mastery profile",
            error.Message,
            StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void LegacyUnmigratedOutcomeKeepsPhase35Behavior()
    {
        var scope = Scope();
        var profile = Profile(
            scope,
            ("LEGACY:A", 58m, 6, 90m));

        var decision = engine.DecideNext(new AdaptiveAssessmentRequest(
            scope.SchoolId,
            scope.AdoptionId,
            "LEGACY",
            [scope.OutcomeA],
            profile,
            AssessmentPurpose.Practice,
            []));

        Assert.False(decision.MathematicsAware);
        Assert.Equal(
            AdaptiveDiagnosticAssessmentEngine.EngineVersion,
            decision.FormulaVersion);
        Assert.Null(decision.TargetSkillId);
        Assert.Null(decision.TargetComplexityScore);
        Assert.Null(decision.TargetQuestionFamily);
        Assert.Null(decision.TargetRepresentation);
    }

    private static AdaptiveMathematicsSkillState BasicState(
        Guid outcomeId,
        string outcomeCode,
        string skillId,
        decimal skillMastery) =>
        new(
            outcomeId,
            outcomeCode,
            skillId,
            skillMastery,
            PrerequisiteMastery: 0.65m,
            CurrentComplexityScore: 42,
            RecentSuccessfulItems: 4,
            MisconceptionHistory: [],
            RepresentationFluency:
            [
                new AdaptiveRepresentationFluency(
                    "symbolic",
                    0.70m,
                    skillId == "ratio.unit_rate"
                        ? ["ratio.unit_rate.direct"]
                        : ["fractions.equivalent.missing_value"])
            ]);

    private static StudentLearningProfile Profile(
        TestScope scope,
        params (string Code, decimal Mastery, int Evidence, decimal Confidence)[] rows)
    {
        var ids = new[] { scope.OutcomeA, scope.OutcomeB };
        var outcomeRows = rows
            .Select((row, index) =>
                new StudentOutcomeLearningProfile(
                    ids[index],
                    row.Code,
                    row.Code,
                    row.Mastery,
                    MasteryBand.Developing,
                    row.Evidence,
                    row.Confidence,
                    DateTime.UtcNow.AddMinutes(-index - 1),
                    2,
                    2,
                    2,
                    4m,
                    "phase31-v2"))
            .ToArray();

        return new StudentLearningProfile(
            scope.SchoolId,
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            scope.AdoptionId,
            outcomeRows.Average(x => x.MasteryPercentage),
            MasteryBand.Developing,
            outcomeRows.Sum(x => x.EvidenceCount),
            outcomeRows.Min(x => x.ConfidencePercentage),
            DateTime.UtcNow,
            outcomeRows,
            "phase31-v2");
    }

    private static TestScope Scope() =>
        new(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid());

    private sealed record TestScope(
        Guid SchoolId,
        Guid AdoptionId,
        Guid OutcomeA,
        Guid OutcomeB);
}
