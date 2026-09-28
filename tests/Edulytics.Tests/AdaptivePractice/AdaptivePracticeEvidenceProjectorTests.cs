using Edulytics.Core.AdaptivePractice;
using Edulytics.Core.Entities;
using Edulytics.Core.Enums;
using Edulytics.Services.AdaptivePractice;

namespace Edulytics.Tests.MathematicsIntelligence.AdaptivePractice;

public sealed class AdaptivePracticeEvidenceProjectorTests
{
    private readonly AdaptivePracticeEvidenceProjector projector =
        new(new AdaptiveMisconceptionClassifier());

    [Fact]
    public void FirstDeterministicErrorIsSuspectedAndUpdatesRepresentationFluency()
    {
        var now = DateTime.UtcNow;
        var session = Session();
        var turn = Turn(
            correct: false,
            answer: "4/3",
            representation: "symbolic",
            now: now);

        var update = projector.Project(
            session,
            turn,
            new AssessmentItem
            {
                CorrectAnswer = "3/4"
            },
            [],
            [],
            misconceptionLoopEnabled: true,
            occurredAtUtc: now);

        Assert.NotNull(update.MisconceptionState);
        Assert.Equal(
            "fraction.reciprocal",
            update.MisconceptionState!.MisconceptionId);
        Assert.Equal(
            AdaptiveMisconceptionStatus.Suspected,
            update.MisconceptionState.Status);
        Assert.Equal(1, update.MisconceptionState.ObservationCount);

        Assert.NotNull(update.RepresentationState);
        Assert.Equal(1, update.RepresentationState!.EvidenceCount);
        Assert.Equal(0, update.RepresentationState.SuccessCount);
        Assert.Equal(0m, update.RepresentationState.WeightedFluency);
    }

    [Fact]
    public void SecondDeterministicObservationActivatesMisconception()
    {
        var now = DateTime.UtcNow;
        var session = Session();
        var existing = new StudentMisconceptionState
        {
            Id = Guid.NewGuid(),
            SchoolId = session.SchoolId,
            StudentProfileId = session.StudentProfileId,
            CurriculumAdoptionId = session.CurriculumAdoptionId,
            SkillId = session.PrimarySkillId,
            MisconceptionId = "fraction.reciprocal",
            QuestionFamily = "fractions.equivalent.missing_value",
            Status = AdaptiveMisconceptionStatus.Suspected,
            Confidence = 0.95m,
            ObservationCount = 1,
            FirstObservedAtUtc = now.AddMinutes(-2),
            LastObservedAtUtc = now.AddMinutes(-2),
            EngineVersion = AdaptivePracticeV2Versions.EngineVersion
        };

        var update = projector.Project(
            session,
            Turn(
                correct: false,
                answer: "4/3",
                representation: "symbolic",
                now: now),
            new AssessmentItem
            {
                CorrectAnswer = "3/4"
            },
            [existing],
            [],
            misconceptionLoopEnabled: true,
            occurredAtUtc: now);

        Assert.NotNull(update.MisconceptionState);
        Assert.Equal(
            AdaptiveMisconceptionStatus.Active,
            update.MisconceptionState!.Status);
        Assert.Equal(2, update.MisconceptionState.ObservationCount);
    }

    [Fact]
    public void IndependentCorrectConfirmationResolvesFocusedMisconception()
    {
        var now = DateTime.UtcNow;
        var session = Session();
        var existing = new StudentMisconceptionState
        {
            Id = Guid.NewGuid(),
            SchoolId = session.SchoolId,
            StudentProfileId = session.StudentProfileId,
            CurriculumAdoptionId = session.CurriculumAdoptionId,
            SkillId = session.PrimarySkillId,
            MisconceptionId = "fraction.reciprocal",
            QuestionFamily = "fractions.equivalent.missing_value",
            Status = AdaptiveMisconceptionStatus.Active,
            Confidence = 0.95m,
            ObservationCount = 2,
            FirstObservedAtUtc = now.AddMinutes(-5),
            LastObservedAtUtc = now.AddMinutes(-4),
            EngineVersion = AdaptivePracticeV2Versions.EngineVersion
        };

        var turn = Turn(
            correct: true,
            answer: "3/4",
            representation: "symbolic",
            now: now);
        turn.MisconceptionFocusId = existing.MisconceptionId;
        turn.IsIndependentConfirmation = true;

        var update = projector.Project(
            session,
            turn,
            new AssessmentItem
            {
                CorrectAnswer = "3/4"
            },
            [existing],
            [],
            misconceptionLoopEnabled: true,
            occurredAtUtc: now);

        Assert.NotNull(update.MisconceptionState);
        Assert.Equal(
            AdaptiveMisconceptionStatus.Resolved,
            update.MisconceptionState!.Status);
        Assert.Equal(now, update.MisconceptionState.ResolvedAtUtc);
    }

    private static AdaptivePracticeSession Session() =>
        new()
        {
            Id = Guid.NewGuid(),
            SchoolId = Guid.NewGuid(),
            StudentProfileId = Guid.NewGuid(),
            CurriculumAdoptionId = Guid.NewGuid(),
            CurriculumPedagogicalLessonId = Guid.NewGuid(),
            LessonCode = "TEST",
            PrimarySkillId = "fractions.equivalent",
            Purpose = AdaptivePracticePurpose.PrivatePractice,
            Status = AdaptivePracticeSessionStatus.InProgress,
            EngineVersion = AdaptivePracticeV2Versions.EngineVersion,
            PolicyVersion = AdaptivePracticeV2Versions.PolicyVersion,
            CapabilityVersion = "test"
        };

    private static AdaptivePracticeTurn Turn(
        bool correct,
        string answer,
        string representation,
        DateTime now) =>
        new()
        {
            Id = Guid.NewGuid(),
            SchoolId = Guid.NewGuid(),
            SessionId = Guid.NewGuid(),
            Sequence = 1,
            AssessmentItemId = Guid.NewGuid(),
            DecisionSnapshotId = Guid.NewGuid(),
            SkillId = "fractions.equivalent",
            QuestionFamily = "fractions.equivalent.missing_value",
            Representation = representation,
            MathematicalComplexityScore = 42,
            UiDifficultyBand = AssessmentItemDifficulty.Medium,
            PresentedAtUtc = now.AddSeconds(-10),
            AnsweredAtUtc = now,
            SubmittedAnswer = answer,
            IsCorrect = correct,
            Score = correct ? 1m : 0m,
            Feedback = "test",
            ExposureFingerprint = Guid.NewGuid().ToString("N"),
            SemanticIdentityKey = Guid.NewGuid().ToString("N")
        };
}
