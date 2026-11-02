using Edulytics.Core.AdaptivePractice;
using Edulytics.Services.AdaptivePractice;
using Edulytics.Services.Mathematics.Difficulty;

namespace Edulytics.Tests.MathematicsIntelligence.AdaptivePractice;

public sealed class AdaptiveNextItemDecisionEngineTests
{
    private readonly AdaptiveNextItemDecisionEngine engine =
        new(new MathematicsDifficultyEngine());

    [Fact]
    public void IncorrectResponseCanNeverIncreaseComplexity()
    {
        var state = State(
            currentComplexity: 60,
            responses:
            [
                new AdaptivePracticeResponseObservation(
                    1,
                    IsCorrect: false,
                    ComplexityScore: 60,
                    QuestionFamily: "fractions.equivalent.missing_value",
                    MisconceptionId: "fraction.reciprocal")
            ],
            misconceptions:
            [
                new AdaptivePracticeMisconceptionEvidence(
                    "fraction.reciprocal",
                    ObservationCount: 2,
                    IsBlocking: true,
                    QuestionFamily:
                        "fractions.equivalent.missing_value",
                    LastObservedSequence: 1)
            ]);

        var decision = engine.Decide(state);

        Assert.True(decision.RemediationLockActive);
        Assert.True(decision.ConfirmationRequired);
        Assert.False(decision.ProgressionEligible);
        Assert.True(
            decision.TargetComplexityScore <= 60,
            "An incorrect answer must never increase next-item complexity.");
        Assert.Equal(
            AdaptivePracticeDecisionReasonCodes.MisconceptionRemediation,
            decision.ReasonCode);
        Assert.Equal(
            "fraction.reciprocal",
            decision.MisconceptionFocusId);
    }

    [Fact]
    public void CorrectRemedialAnswerStillRequiresFreshConfirmation()
    {
        var state = State(
            currentComplexity: 48,
            responses:
            [
                new AdaptivePracticeResponseObservation(
                    1,
                    IsCorrect: false,
                    ComplexityScore: 60,
                    QuestionFamily:
                        "fractions.equivalent.missing_value"),
                new AdaptivePracticeResponseObservation(
                    2,
                    IsCorrect: true,
                    ComplexityScore: 48,
                    QuestionFamily:
                        "fractions.equivalent.missing_value")
            ],
            remediation:
                new AdaptivePracticeRemediationState(
                    IsLocked: true,
                    ConfirmationRequired: true,
                    LockComplexityScore: 60,
                    PreferredQuestionFamily:
                        "fractions.equivalent.missing_value"));

        var decision = engine.Decide(state);

        Assert.True(decision.RemediationLockActive);
        Assert.True(decision.ConfirmationRequired);
        Assert.False(decision.ProgressionEligible);
        Assert.True(decision.TargetComplexityScore <= 60);
        Assert.Equal(
            AdaptivePracticeDecisionReasonCodes
                .MisconceptionConfirmationRequired,
            decision.ReasonCode);
    }

    [Fact]
    public void UnlockedHighReadinessStateMayProgressOnlyByBoundedStep()
    {
        var state = State(
            skillMastery: 0.95m,
            prerequisiteMastery: 0.95m,
            currentComplexity: 42,
            recentSuccessfulItems: 8,
            responses:
            [
                new AdaptivePracticeResponseObservation(
                    1,
                    IsCorrect: true,
                    ComplexityScore: 42,
                    QuestionFamily:
                        "fractions.equivalent.missing_value",
                    IsIndependentConfirmation: true)
            ]);

        var decision = engine.Decide(state);

        Assert.False(decision.RemediationLockActive);
        Assert.False(decision.ConfirmationRequired);
        Assert.True(decision.ProgressionEligible);
        Assert.Equal(54, decision.TargetComplexityScore);
        Assert.Equal(
            AdaptivePracticeDecisionReasonCodes.ComplexityProgress,
            decision.ReasonCode);
    }

    [Fact]
    public void WeakPrerequisiteBlocksProgression()
    {
        var state = State(
            skillMastery: 0.90m,
            prerequisiteMastery: 0.30m,
            currentComplexity: 60,
            recentSuccessfulItems: 8,
            responses:
            [
                new AdaptivePracticeResponseObservation(
                    1,
                    IsCorrect: true,
                    ComplexityScore: 60,
                    QuestionFamily:
                        "fractions.equivalent.missing_value")
            ]);

        var decision = engine.Decide(state);

        Assert.False(decision.ProgressionEligible);
        Assert.True(decision.TargetComplexityScore < 60);
        Assert.Equal(
            AdaptivePracticeDecisionReasonCodes.PrerequisiteRecovery,
            decision.ReasonCode);
    }

    [Fact]
    public void RemediationFamilyCannotInheritMisconceptionFromAnotherFamily()
    {
        var state = State(
            currentComplexity: 48,
            responses:
            [
                new AdaptivePracticeResponseObservation(
                    2,
                    IsCorrect: false,
                    ComplexityScore: 48,
                    QuestionFamily:
                        "fractions.equivalent.recognize",
                    MisconceptionId: "fraction.reciprocal")
            ],
            misconceptions:
            [
                new AdaptivePracticeMisconceptionEvidence(
                    "fraction.reciprocal",
                    3,
                    IsBlocking: true,
                    QuestionFamily:
                        "fractions.equivalent.missing_value",
                    LastObservedSequence: 1)
            ],
            remediation:
                new AdaptivePracticeRemediationState(
                    IsLocked: true,
                    ConfirmationRequired: true,
                    LockComplexityScore: 48,
                    BlockingMisconceptionId: "fraction.reciprocal",
                    PreferredQuestionFamily:
                        "fractions.equivalent.recognize",
                    PreferredRepresentation: "symbolic"));

        var decision = engine.Decide(state);

        Assert.Equal(
            "fractions.equivalent.recognize",
            decision.TargetQuestionFamily);
        Assert.Null(decision.MisconceptionFocusId);
        Assert.NotEqual(
            AdaptivePracticeDecisionReasonCodes.MisconceptionRemediation,
            decision.ReasonCode);
    }

    [Fact]
    public void BlockingMisconceptionFamilyIsPreferredWhenAllowed()
    {
        var state = State(
            currentComplexity: 48,
            responses:
            [
                new AdaptivePracticeResponseObservation(
                    1,
                    IsCorrect: false,
                    ComplexityScore: 48,
                    QuestionFamily:
                        "fractions.equivalent.recognize")
            ],
            misconceptions:
            [
                new AdaptivePracticeMisconceptionEvidence(
                    "fraction.reciprocal",
                    3,
                    IsBlocking: true,
                    QuestionFamily:
                        "fractions.equivalent.missing_value",
                    LastObservedSequence: 1)
            ]);

        var decision = engine.Decide(state);

        Assert.Equal(
            "fractions.equivalent.missing_value",
            decision.TargetQuestionFamily);
        Assert.Equal(
            "fraction.reciprocal",
            decision.MisconceptionFocusId);
    }

    private static AdaptivePracticeLearningState State(
        decimal skillMastery = 0.70m,
        decimal prerequisiteMastery = 0.70m,
        int currentComplexity = 42,
        int recentSuccessfulItems = 3,
        IReadOnlyList<AdaptivePracticeResponseObservation>? responses = null,
        IReadOnlyList<AdaptivePracticeMisconceptionEvidence>? misconceptions = null,
        AdaptivePracticeRemediationState? remediation = null) =>
        new(
            SkillId: "fractions.equivalent",
            SkillMastery: skillMastery,
            PrerequisiteMastery: prerequisiteMastery,
            CurrentComplexityScore: currentComplexity,
            RecentSuccessfulItems: recentSuccessfulItems,
            RecentResponses: responses ?? [],
            Misconceptions: misconceptions ?? [],
            RepresentationFluency:
            [
                new AdaptivePracticeRepresentationFluency(
                    "symbolic",
                    0.70m,
                    [
                        "fractions.equivalent.missing_value",
                        "fractions.equivalent.recognize"
                    ])
            ],
            AllowedQuestionFamilies:
            [
                "fractions.equivalent.missing_value",
                "fractions.equivalent.recognize"
            ],
            Remediation:
                remediation ??
                AdaptivePracticeRemediationState.None);
}
