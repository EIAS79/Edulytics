using Edulytics.Core.Analytics;
using Edulytics.Core.Entities;
using Edulytics.Core.Enums;
using Edulytics.Services.Analytics;

namespace Edulytics.Tests.Phase09;

public sealed class AnalyticsProjectionBuilderTests
{
    [Fact]
    public void FormalAnswers_CreateMastery_AndAssessmentTrend()
    {
        var source = BuildFormalAssessmentSource(
            AssessmentStatus.Open,
            percentage: 70m);

        var result = new AnalyticsProjectionBuilder().Build(
            source,
            new DateTime(2026, 8, 15, 12, 0, 0, DateTimeKind.Utc));

        var mastery = Assert.Single(result.StudentOutcomeMasteries);
        Assert.Equal(70m, mastery.MasteryPercentage);
        Assert.Equal(1, mastery.EvidenceCount);

        var classOutcome = Assert.Single(result.ClassOutcomeSummaries);
        Assert.Equal(70m, classOutcome.AverageMasteryPercentage);
        Assert.Single(result.ClassTopicSummaries);

        var trend = Assert.Single(result.ClassAssessmentTrends);
        Assert.Equal(70m, trend.AveragePercentage);
    }

    [Fact]
    public void DraftAssessment_IsExcluded()
    {
        var source = BuildFormalAssessmentSource(
            AssessmentStatus.Draft,
            percentage: 100m);

        var result = new AnalyticsProjectionBuilder().Build(
            source,
            DateTime.UtcNow);

        Assert.Empty(result.StudentOutcomeMasteries);
        Assert.Empty(result.ClassOutcomeSummaries);
        Assert.Empty(result.ClassTopicSummaries);
        Assert.Empty(result.ClassAssessmentTrends);
    }

    [Fact]
    public void FormalMultiOutcomeMapping_SplitsWeightWithoutDoubleCounting()
    {
        var source = BuildFormalAssessmentSource(
            AssessmentStatus.Open,
            percentage: 80m,
            mapTwoOutcomes: true);

        var result = new AnalyticsProjectionBuilder().Build(
            source,
            new DateTime(2026, 8, 15, 12, 0, 0, DateTimeKind.Utc));

        var masteries = result.StudentOutcomeMasteries
            .OrderBy(x => x.LearningOutcomeId)
            .ToArray();

        Assert.Equal(2, masteries.Length);
        Assert.All(masteries, x => Assert.Equal(80m, x.MasteryPercentage));
        Assert.All(masteries, x => Assert.Equal(1, x.EvidenceCount));
        Assert.Equal(1m, masteries.Sum(x => x.PossibleScore));
        Assert.Equal(0.8m, masteries.Sum(x => x.EarnedScore));
        Assert.Equal(2, result.ClassOutcomeSummaries.Count);
        Assert.Single(result.ClassAssessmentTrends);
    }

    [Theory]
    [InlineData(0, MasteryBand.CriticalGap)]
    [InlineData(39.99, MasteryBand.CriticalGap)]
    [InlineData(40, MasteryBand.Weak)]
    [InlineData(59.99, MasteryBand.Weak)]
    [InlineData(60, MasteryBand.Developing)]
    [InlineData(74.99, MasteryBand.Developing)]
    [InlineData(75, MasteryBand.Secure)]
    [InlineData(89.99, MasteryBand.Secure)]
    [InlineData(90, MasteryBand.Strong)]
    [InlineData(100, MasteryBand.Strong)]
    public void MasteryBandBoundaries_AreDeterministic(
        double value,
        MasteryBand expected)
    {
        Assert.Equal(
            expected,
            AnalyticsProjectionBuilder.BandFor((decimal)value));
    }

    private static AnalyticsSourceSnapshot BuildFormalAssessmentSource(
        AssessmentStatus status,
        decimal percentage,
        bool mapTwoOutcomes = false)
    {
        var school = Guid.NewGuid();
        var year = Guid.NewGuid();
        var grade = Guid.NewGuid();
        var cls = Guid.NewGuid();
        var subject = Guid.NewGuid();
        var student = Guid.NewGuid();
        var framework = Guid.NewGuid();
        var topic = Guid.NewGuid();
        var outcome1 = Guid.NewGuid();
        var outcome2 = Guid.NewGuid();
        var assessment = Guid.NewGuid();
        var question = Guid.NewGuid();
        var result = Guid.NewGuid();
        var evidenceAt = new DateTime(2026, 8, 15, 11, 0, 0, DateTimeKind.Utc);

        var outcomes = new List<LearningOutcome>
        {
            new()
            {
                Id = outcome1,
                SchoolId = school,
                FrameworkVersionId = framework,
                SubjectId = subject,
                GradeLevelId = grade,
                TopicId = topic,
                Code = "N1",
                Description = "One",
                Weight = 1m,
                Order = 1
            }
        };

        if (mapTwoOutcomes)
        {
            outcomes.Add(new LearningOutcome
            {
                Id = outcome2,
                SchoolId = school,
                FrameworkVersionId = framework,
                SubjectId = subject,
                GradeLevelId = grade,
                TopicId = topic,
                Code = "N2",
                Description = "Two",
                Weight = 1m,
                Order = 2
            });
        }

        var mappings = new List<QuestionLearningOutcome>
        {
            new()
            {
                Id = Guid.NewGuid(),
                SchoolId = school,
                AssessmentQuestionId = question,
                LearningOutcomeId = outcome1
            }
        };

        if (mapTwoOutcomes)
        {
            mappings.Add(new QuestionLearningOutcome
            {
                Id = Guid.NewGuid(),
                SchoolId = school,
                AssessmentQuestionId = question,
                LearningOutcomeId = outcome2
            });
        }

        return new AnalyticsSourceSnapshot(
            [new AcademicYear
            {
                Id = year,
                SchoolId = school,
                Name = "2026/2027"
            }],
            [new ClassGroup
            {
                Id = cls,
                SchoolId = school,
                AcademicYearId = year,
                GradeLevelId = grade,
                Name = "6A",
                Code = "6A",
                NormalizedCode = "6A"
            }],
            [new Subject
            {
                Id = subject,
                SchoolId = school,
                Name = "Mathematics",
                Code = "MATH",
                NormalizedCode = "MATH"
            }],
            [new StudentProfile
            {
                Id = student,
                SchoolId = school,
                StudentNumber = "S1",
                NormalizedStudentNumber = "S1",
                DisplayName = "A Student"
            }],
            [new StudentEnrollment
            {
                Id = Guid.NewGuid(),
                SchoolId = school,
                AcademicYearId = year,
                ClassGroupId = cls,
                StudentProfileId = student,
                EnrolledAtUtc = new DateTime(2026, 8, 1, 0, 0, 0, DateTimeKind.Utc)
            }],
            [],
            [new CurriculumTopic
            {
                Id = topic,
                SchoolId = school,
                FrameworkVersionId = framework,
                SubjectId = subject,
                GradeLevelId = grade,
                Name = "Numbers",
                Order = 1
            }],
            outcomes,
            [new Assessment
            {
                Id = assessment,
                SchoolId = school,
                SubjectId = subject,
                ClassGroupId = cls,
                AcademicYearId = year,
                TermId = Guid.NewGuid(),
                Title = "Assessment",
                AssessmentDate = new DateOnly(2026, 9, 20),
                MaxScore = 10m,
                Status = status,
                CreatedByUserId = Guid.NewGuid()
            }],
            [new AssessmentQuestion
            {
                Id = question,
                SchoolId = school,
                AssessmentId = assessment,
                Prompt = "Question",
                MaxScore = 10m,
                Order = 1
            }],
            mappings,
            [new AssessmentResult
            {
                Id = result,
                SchoolId = school,
                AssessmentId = assessment,
                StudentProfileId = student,
                Score = percentage / 10m,
                Percentage = percentage,
                EnteredByUserId = Guid.NewGuid(),
                UpdatedAtUtc = evidenceAt
            }],
            [new StudentAnswer
            {
                Id = Guid.NewGuid(),
                SchoolId = school,
                AssessmentResultId = result,
                AssessmentQuestionId = question,
                Score = percentage / 10m,
                UpdatedAtUtc = evidenceAt
            }]);
    }
}
