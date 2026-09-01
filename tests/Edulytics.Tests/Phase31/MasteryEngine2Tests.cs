using Edulytics.Core.Analytics;
using Edulytics.Core.Entities;
using Edulytics.Core.Enums;
using Edulytics.Services.Analytics;

namespace Edulytics.Tests.Phase31;

public sealed class MasteryEngine2Tests
{
    private static readonly DateTime Now =
        new(2026, 9, 1, 7, 0, 0, DateTimeKind.Utc);

    [Fact]
    public void DifficultyWeighting_IsDeterministic()
    {
        var source = BuildSource(
            [
                Evidence(true, 1m, AssessmentItemDifficulty.Easy, Now.AddDays(-1)),
                Evidence(false, 0m, AssessmentItemDifficulty.Challenging, Now.AddDays(-1))
            ]);

        var mastery = Assert.Single(
            new AnalyticsProjectionBuilder().Build(source, Now).StudentOutcomeMasteries);

        Assert.Equal(42.5m, mastery.MasteryPercentage);
        Assert.Equal(2, mastery.EvidenceCount);
        Assert.Equal(MasteryBand.Weak, mastery.Band);
    }

    [Fact]
    public void RecencyWeighting_GivesNewerEvidenceMoreInfluence()
    {
        var source = BuildSource(
            [
                Evidence(true, 1m, AssessmentItemDifficulty.Medium, Now.AddDays(-1)),
                Evidence(false, 0m, AssessmentItemDifficulty.Medium, Now.AddDays(-250))
            ]);

        var mastery = Assert.Single(
            new AnalyticsProjectionBuilder().Build(source, Now).StudentOutcomeMasteries);

        Assert.Equal(68.97m, mastery.MasteryPercentage);
        Assert.Equal(MasteryBand.Developing, mastery.Band);
    }

    [Fact]
    public void FormalAssessment_DoesNotContaminateMastery_ButStillBuildsTrend()
    {
        var source = BuildSource([], includeFormalAssessment: true);
        var result = new AnalyticsProjectionBuilder().Build(source, Now);

        Assert.Empty(result.StudentOutcomeMasteries);
        Assert.Empty(result.ClassOutcomeSummaries);
        var trend = Assert.Single(result.ClassAssessmentTrends);
        Assert.Equal(100m, trend.AveragePercentage);
    }

    [Fact]
    public void LearningProfile_IsScopedToStudentAndCurriculumAdoption()
    {
        var source = BuildSource(
            [
                Evidence(true, 1m, AssessmentItemDifficulty.Easy, Now.AddDays(-2)),
                Evidence(true, 1m, AssessmentItemDifficulty.Medium, Now.AddDays(-1))
            ]);

        var fixture = FixtureIds.Last;
        var profile = new AnalyticsProjectionBuilder().BuildStudentLearningProfile(
            source,
            fixture.StudentId,
            fixture.AdoptionId,
            Now);

        Assert.Equal(fixture.StudentId, profile.StudentProfileId);
        Assert.Equal(fixture.AdoptionId, profile.CurriculumAdoptionId);
        Assert.Equal(100m, profile.OverallMasteryPercentage);
        Assert.Equal(2, profile.EvidenceCount);
        Assert.Equal(40m, profile.ConfidencePercentage);
        Assert.Equal("phase31-v1", profile.FormulaVersion);
        var row = Assert.Single(profile.Outcomes);
        Assert.Equal(1, row.EasyEvidenceCount);
        Assert.Equal(1, row.MediumEvidenceCount);
        Assert.Equal(0, row.ChallengingEvidenceCount);
        Assert.Equal(40m, row.ConfidencePercentage);
    }

    [Fact]
    public void CrossAdoptionEvidence_FailsClosed()
    {
        var source = BuildSource(
            [Evidence(true, 1m, AssessmentItemDifficulty.Medium, Now.AddDays(-1))]);
        var evidence = Assert.Single(source.LearningEvidence!);
        var attempt = Assert.Single(source.PracticeAttempts!);
        source.LearningOutcomes[0].CurriculumAdoptionId = Guid.NewGuid();

        var error = Assert.Throws<InvalidOperationException>(
            () => new AnalyticsProjectionBuilder().Build(source, Now));

        Assert.Contains("curriculum adoption", error.Message, StringComparison.OrdinalIgnoreCase);
        Assert.Equal(evidence.PracticeAttemptId, attempt.Id);
    }

    [Fact]
    public void SameEvidence_ProducesSameMasteryValues()
    {
        var source = BuildSource(
            [
                Evidence(true, 1m, AssessmentItemDifficulty.Challenging, Now.AddDays(-10)),
                Evidence(false, 0m, AssessmentItemDifficulty.Easy, Now.AddDays(-70))
            ]);
        var builder = new AnalyticsProjectionBuilder();

        var first = Assert.Single(builder.Build(source, Now).StudentOutcomeMasteries);
        var second = Assert.Single(builder.Build(source, Now).StudentOutcomeMasteries);

        Assert.Equal(first.EarnedScore, second.EarnedScore);
        Assert.Equal(first.PossibleScore, second.PossibleScore);
        Assert.Equal(first.MasteryPercentage, second.MasteryPercentage);
        Assert.Equal(first.EvidenceCount, second.EvidenceCount);
        Assert.Equal(first.Band, second.Band);
    }

    private static LearningEvidence Evidence(
        bool correct,
        decimal score,
        AssessmentItemDifficulty difficulty,
        DateTime occurredAtUtc) =>
        new()
        {
            Id = Guid.NewGuid(),
            EvidenceType = LearningEvidenceType.Practice,
            Difficulty = difficulty,
            IsCorrect = correct,
            Score = score,
            MaxScore = 1m,
            OccurredAtUtc = occurredAtUtc
        };

    private static AnalyticsSourceSnapshot BuildSource(
        IReadOnlyList<LearningEvidence> evidence,
        bool includeFormalAssessment = false)
    {
        var ids = new FixtureIds(
            Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),
            Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),
            Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
        FixtureIds.Last = ids;

        var attempts = new[]
        {
            new PracticeAttempt
            {
                Id = ids.AttemptId,
                SchoolId = ids.SchoolId,
                StudentProfileId = ids.StudentId,
                CurriculumAdoptionId = ids.AdoptionId,
                Status = PracticeAttemptStatus.Submitted,
                StartedAtUtc = Now.AddDays(-300),
                SubmittedAtUtc = Now
            }
        };

        foreach (var item in evidence)
        {
            item.SchoolId = ids.SchoolId;
            item.StudentProfileId = ids.StudentId;
            item.LearningOutcomeId = ids.OutcomeId;
            item.PracticeAttemptId = ids.AttemptId;
            item.AssessmentItemId = Guid.NewGuid();
        }

        var assessmentId = Guid.NewGuid();
        var resultId = Guid.NewGuid();
        var questionId = Guid.NewGuid();

        return new AnalyticsSourceSnapshot(
            [new AcademicYear { Id = ids.YearId, SchoolId = ids.SchoolId, Name = "2026/2027" }],
            [new ClassGroup
            {
                Id = ids.ClassId,
                SchoolId = ids.SchoolId,
                AcademicYearId = ids.YearId,
                AcademicProgramId = ids.ProgramId,
                GradeLevelId = ids.GradeId,
                CurriculumAdoptionId = ids.AdoptionId,
                Name = "6A",
                Code = "6A",
                NormalizedCode = "6A"
            }],
            [new Subject
            {
                Id = ids.SubjectId,
                SchoolId = ids.SchoolId,
                Name = "Mathematics",
                Code = "MATH",
                NormalizedCode = "MATH"
            }],
            [new StudentProfile
            {
                Id = ids.StudentId,
                SchoolId = ids.SchoolId,
                StudentNumber = "S1",
                NormalizedStudentNumber = "S1",
                DisplayName = "Student One"
            }],
            [new StudentEnrollment
            {
                Id = Guid.NewGuid(),
                SchoolId = ids.SchoolId,
                StudentProfileId = ids.StudentId,
                ClassGroupId = ids.ClassId,
                AcademicYearId = ids.YearId,
                EnrolledAtUtc = Now.AddMonths(-1)
            }],
            [],
            [new CurriculumTopic
            {
                Id = ids.TopicId,
                SchoolId = ids.SchoolId,
                FrameworkVersionId = ids.FrameworkId,
                AcademicProgramId = ids.ProgramId,
                SubjectId = ids.SubjectId,
                GradeLevelId = ids.GradeId,
                CurriculumAdoptionId = ids.AdoptionId,
                Name = "Numbers",
                Order = 1
            }],
            [new LearningOutcome
            {
                Id = ids.OutcomeId,
                SchoolId = ids.SchoolId,
                AcademicProgramId = ids.ProgramId,
                FrameworkVersionId = ids.FrameworkId,
                SubjectId = ids.SubjectId,
                GradeLevelId = ids.GradeId,
                CurriculumAdoptionId = ids.AdoptionId,
                TopicId = ids.TopicId,
                Code = "N1",
                Description = "Number outcome",
                Weight = 1m,
                Order = 1
            }],
            includeFormalAssessment
                ? [new Assessment
                {
                    Id = assessmentId,
                    SchoolId = ids.SchoolId,
                    AcademicYearId = ids.YearId,
                    ClassGroupId = ids.ClassId,
                    SubjectId = ids.SubjectId,
                    TermId = Guid.NewGuid(),
                    Title = "Formal",
                    AssessmentDate = new DateOnly(2026, 9, 1),
                    MaxScore = 10m,
                    Status = AssessmentStatus.Open,
                    CreatedByUserId = Guid.NewGuid()
                }]
                : [],
            includeFormalAssessment
                ? [new AssessmentQuestion
                {
                    Id = questionId,
                    SchoolId = ids.SchoolId,
                    AssessmentId = assessmentId,
                    Prompt = "Formal question",
                    MaxScore = 10m,
                    Order = 1
                }]
                : [],
            includeFormalAssessment
                ? [new QuestionLearningOutcome
                {
                    Id = Guid.NewGuid(),
                    SchoolId = ids.SchoolId,
                    AssessmentQuestionId = questionId,
                    LearningOutcomeId = ids.OutcomeId
                }]
                : [],
            includeFormalAssessment
                ? [new AssessmentResult
                {
                    Id = resultId,
                    SchoolId = ids.SchoolId,
                    AssessmentId = assessmentId,
                    StudentProfileId = ids.StudentId,
                    Score = 10m,
                    Percentage = 100m,
                    EnteredByUserId = Guid.NewGuid(),
                    UpdatedAtUtc = Now
                }]
                : [],
            includeFormalAssessment
                ? [new StudentAnswer
                {
                    Id = Guid.NewGuid(),
                    SchoolId = ids.SchoolId,
                    AssessmentResultId = resultId,
                    AssessmentQuestionId = questionId,
                    Score = 10m,
                    UpdatedAtUtc = Now
                }]
                : [],
            attempts,
            evidence);
    }

    private sealed record FixtureIds(
        Guid SchoolId,
        Guid YearId,
        Guid ProgramId,
        Guid GradeId,
        Guid ClassId,
        Guid SubjectId,
        Guid StudentId,
        Guid FrameworkId,
        Guid TopicId,
        Guid OutcomeId,
        Guid AdoptionId)
    {
        public static FixtureIds Last { get; set; } = null!;
        public Guid AttemptId { get; } = Guid.NewGuid();
    }
}
