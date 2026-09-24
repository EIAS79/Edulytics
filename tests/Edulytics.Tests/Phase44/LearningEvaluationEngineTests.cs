using System.Text.Json;
using Edulytics.Core.Analytics;
using Edulytics.Core.Entities;
using Edulytics.Core.Enums;
using Edulytics.Services.Analytics;

namespace Edulytics.Tests.Phase44;

public sealed class LearningEvaluationEngineTests
{
    private static readonly DateTime Now =
        new(2026, 9, 24, 10, 0, 0, DateTimeKind.Utc);

    [Fact]
    public void MissingEvidence_IsNotConvertedToZeroMastery()
    {
        var fixture = BuildFixture(
            assessmentScores: [],
            practiceScores: []);

        var evaluation = NewEngine().BuildStudentSubject(
            fixture.Snapshot,
            fixture.StudentId,
            fixture.YearId,
            fixture.ClassId,
            fixture.SubjectId,
            Now);

        Assert.Null(evaluation.CurrentMasteryPercentage);
        Assert.Equal(0m, evaluation.CurriculumCoveragePercentage);
        Assert.Equal(1, evaluation.ExpectedSkillCount);
        Assert.Equal(0, evaluation.EvaluatedSkillCount);

        var skill = Assert.Single(evaluation.Skills);
        Assert.Equal(
            EvaluationSkillStatus.NotYetAssessed,
            skill.Status);
        Assert.Null(skill.CurrentMasteryPercentage);
        Assert.Equal(
            "fractions.equivalent",
            skill.SkillKey);
    }

    [Fact]
    public void AssessmentAndPractice_AreKeptSeparate_WithTransferGap()
    {
        var fixture = BuildFixture(
            assessmentScores: [0.4m, 0.4m],
            practiceScores: [0.8m, 0.8m]);

        var evaluation = NewEngine().BuildStudentSubject(
            fixture.Snapshot,
            fixture.StudentId,
            fixture.YearId,
            fixture.ClassId,
            fixture.SubjectId,
            Now);

        Assert.Equal(40m, evaluation.AssessmentMasteryPercentage);
        Assert.Equal(80m, evaluation.PracticeMasteryPercentage);
        Assert.Equal(-40m, evaluation.PracticeToAssessmentGapPercentagePoints);
        Assert.Equal(100m, evaluation.CurriculumCoveragePercentage);

        var skill = Assert.Single(evaluation.Skills);
        Assert.True(skill.TargetedCheckAvailable);
        Assert.Equal(4, skill.Evidence.TotalEvidence);
        Assert.Equal(2, skill.Evidence.AssessmentEvidence);
        Assert.Equal(2, skill.Evidence.PracticeEvidence);
        Assert.Equal(
            EvaluationConfidenceBand.VeryStrong,
            skill.ConfidenceBand);
    }

    [Fact]
    public void ExactSkillMetadata_OverridesOutcomeProxy()
    {
        var fixture = BuildFixture(
            assessmentScores: [1m, 1m],
            practiceScores: [],
            outcomeCode: "UNMAPPED-OUTCOME",
            exactSkillId: "fractions.add_subtract");

        var evaluation = NewEngine().BuildStudentSubject(
            fixture.Snapshot,
            fixture.StudentId,
            fixture.YearId,
            fixture.ClassId,
            fixture.SubjectId,
            Now);

        var skill = Assert.Single(evaluation.Skills);
        Assert.Equal(
            "fractions.add_subtract",
            skill.SkillKey);
        Assert.Equal(
            EvaluationSkillResolutionKind.ExactSkillContract,
            skill.SkillResolutionKind);
        Assert.False(skill.TargetedCheckAvailable);
        Assert.Contains(
            "fractions.equivalent",
            skill.PrerequisiteSkillKeys);
    }

    [Fact]
    public void ConfidenceRequiresEvidence_NotJustHighPercentage()
    {
        var fixture = BuildFixture(
            assessmentScores: [1m],
            practiceScores: []);

        var evaluation = NewEngine().BuildStudentSubject(
            fixture.Snapshot,
            fixture.StudentId,
            fixture.YearId,
            fixture.ClassId,
            fixture.SubjectId,
            Now);

        var skill = Assert.Single(evaluation.Skills);
        Assert.Equal(100m, skill.CurrentMasteryPercentage);
        Assert.Equal(
            EvaluationSkillStatus.InsufficientEvidence,
            skill.Status);
        Assert.Equal(
            EvaluationConfidenceBand.Limited,
            skill.ConfidenceBand);
    }


    [Fact]
    public void SubjectConfidence_IsCoverageAdjusted()
    {
        var fixture = BuildFixture(
            assessmentScores: [0.7m, 0.7m],
            practiceScores: [0.7m, 0.7m],
            includeUnassessedOutcome: true);

        var evaluation = NewEngine().BuildStudentSubject(
            fixture.Snapshot,
            fixture.StudentId,
            fixture.YearId,
            fixture.ClassId,
            fixture.SubjectId,
            Now);

        Assert.Equal(2, evaluation.ExpectedSkillCount);
        Assert.Equal(1, evaluation.EvaluatedSkillCount);
        Assert.Equal(50m, evaluation.CurriculumCoveragePercentage);
        Assert.Equal(42.5m, evaluation.ConfidencePercentage);
        Assert.Equal(
            EvaluationConfidenceBand.Moderate,
            evaluation.ConfidenceBand);

        var evaluatedSkill = evaluation.Skills.Single(
            x => x.CurrentMasteryPercentage.HasValue);
        Assert.Equal(85m, evaluatedSkill.ConfidencePercentage);
        Assert.Equal(
            EvaluationConfidenceBand.VeryStrong,
            evaluatedSkill.ConfidenceBand);
    }

    private static LearningEvaluationEngine NewEngine() =>
        new(new EvaluationEvidenceNormalizer());

    private static Fixture BuildFixture(
        IReadOnlyList<decimal> assessmentScores,
        IReadOnlyList<decimal> practiceScores,
        string outcomeCode = "CCSS:4.NF.A.1",
        string exactSkillId = "fractions.equivalent",
        bool includeUnassessedOutcome = false)
    {
        var schoolId = Guid.NewGuid();
        var yearId = Guid.NewGuid();
        var programId = Guid.NewGuid();
        var gradeId = Guid.NewGuid();
        var classId = Guid.NewGuid();
        var subjectId = Guid.NewGuid();
        var studentId = Guid.NewGuid();
        var topicId = Guid.NewGuid();
        var outcomeId = Guid.NewGuid();
        var adoptionId = Guid.NewGuid();
        var termId = Guid.NewGuid();

        var year = new AcademicYear
        {
            Id = yearId,
            SchoolId = schoolId,
            Name = "2026/2027",
            StartsOn = new DateOnly(2026, 9, 1),
            EndsOn = new DateOnly(2027, 6, 30),
            Status = AcademicStructureStatus.Active
        };
        var classGroup = new ClassGroup
        {
            Id = classId,
            SchoolId = schoolId,
            AcademicYearId = yearId,
            AcademicProgramId = programId,
            GradeLevelId = gradeId,
            CurriculumAdoptionId = adoptionId,
            Name = "4A",
            Code = "4A",
            NormalizedCode = "4A",
            Status = AcademicStructureStatus.Active
        };
        var subject = new Subject
        {
            Id = subjectId,
            SchoolId = schoolId,
            Name = "Mathematics",
            Code = "MATH",
            NormalizedCode = "MATH",
            Status = AcademicStructureStatus.Active
        };
        var student = new StudentProfile
        {
            Id = studentId,
            SchoolId = schoolId,
            StudentNumber = "ST-1",
            NormalizedStudentNumber = "ST-1",
            DisplayName = "Student One",
            Status = AcademicStructureStatus.Active
        };
        var topic = new CurriculumTopic
        {
            Id = topicId,
            SchoolId = schoolId,
            AcademicProgramId = programId,
            FrameworkVersionId = Guid.NewGuid(),
            SubjectId = subjectId,
            GradeLevelId = gradeId,
            CurriculumAdoptionId = adoptionId,
            Name = "Fractions",
            Order = 1
        };
        var outcome = new LearningOutcome
        {
            Id = outcomeId,
            SchoolId = schoolId,
            AcademicProgramId = programId,
            FrameworkVersionId = topic.FrameworkVersionId,
            SubjectId = subjectId,
            GradeLevelId = gradeId,
            CurriculumAdoptionId = adoptionId,
            TopicId = topicId,
            Code = outcomeCode,
            Description = "Equivalent fractions",
            Weight = 1m,
            Order = 1
        };

        var additionalOutcome = new LearningOutcome
        {
            Id = Guid.NewGuid(),
            SchoolId = schoolId,
            AcademicProgramId = programId,
            FrameworkVersionId = topic.FrameworkVersionId,
            SubjectId = subjectId,
            GradeLevelId = gradeId,
            CurriculumAdoptionId = adoptionId,
            TopicId = topicId,
            Code = "UNASSESSED-OUTCOME",
            Description = "Unassessed skill",
            Weight = 1m,
            Order = 2
        };
        var enrollment = new StudentEnrollment
        {
            Id = Guid.NewGuid(),
            SchoolId = schoolId,
            StudentProfileId = studentId,
            ClassGroupId = classId,
            AcademicYearId = yearId,
            EnrolledAtUtc = Now.AddDays(-20)
        };
        var term = new Term
        {
            Id = termId,
            SchoolId = schoolId,
            AcademicYearId = yearId,
            Name = "Term 1",
            StartsOn = year.StartsOn,
            EndsOn = new DateOnly(2026, 12, 20),
            Status = AcademicStructureStatus.Active
        };

        var assessments = new List<Assessment>();
        var questions = new List<AssessmentQuestion>();
        var mappings = new List<QuestionLearningOutcome>();
        var items = new List<AssessmentItem>();
        var assessmentResults = new List<AssessmentResult>();
        var answers = new List<StudentAnswer>();

        for (var i = 0; i < assessmentScores.Count; i++)
        {
            var assessmentId = Guid.NewGuid();
            var questionId = Guid.NewGuid();
            var resultId = Guid.NewGuid();
            var occurred = Now.AddDays(-10 + i);

            assessments.Add(new Assessment
            {
                Id = assessmentId,
                SchoolId = schoolId,
                SubjectId = subjectId,
                ClassGroupId = classId,
                AcademicYearId = yearId,
                TermId = termId,
                Title = $"Assessment {i + 1}",
                AssessmentDate = DateOnly.FromDateTime(occurred),
                MaxScore = 10m,
                Status = AssessmentStatus.Open,
                CreatedByUserId = Guid.NewGuid()
            });
            questions.Add(new AssessmentQuestion
            {
                Id = questionId,
                SchoolId = schoolId,
                AssessmentId = assessmentId,
                Prompt = "Question",
                MaxScore = 10m,
                Order = 1
            });
            mappings.Add(new QuestionLearningOutcome
            {
                Id = Guid.NewGuid(),
                SchoolId = schoolId,
                AssessmentQuestionId = questionId,
                LearningOutcomeId = outcomeId
            });
            items.Add(new AssessmentItem
            {
                Id = questionId,
                SchoolId = schoolId,
                CurriculumAdoptionId = adoptionId,
                CurriculumTopicId = topicId,
                Difficulty = AssessmentItemDifficulty.Medium,
                Prompt = "Question",
                CorrectAnswer = "1",
                Solution = "1",
                GenerationFamily = "fractions.equivalent.recognize",
                GenerationParametersJson = JsonSerializer.Serialize(
                    new { exactSkillId }),
                ExposureFingerprint = Guid.NewGuid().ToString("N"),
                CreatedAtUtc = occurred
            });
            assessmentResults.Add(new AssessmentResult
            {
                Id = resultId,
                SchoolId = schoolId,
                AssessmentId = assessmentId,
                StudentProfileId = studentId,
                Score = assessmentScores[i] * 10m,
                Percentage = assessmentScores[i] * 100m,
                EnteredByUserId = Guid.NewGuid(),
                EnteredAtUtc = occurred,
                UpdatedAtUtc = occurred
            });
            answers.Add(new StudentAnswer
            {
                Id = Guid.NewGuid(),
                SchoolId = schoolId,
                AssessmentResultId = resultId,
                AssessmentQuestionId = questionId,
                Score = assessmentScores[i] * 10m,
                UpdatedAtUtc = occurred
            });
        }

        var attempts = new List<PracticeAttempt>();
        var learningEvidence = new List<LearningEvidence>();

        for (var i = 0; i < practiceScores.Count; i++)
        {
            var attemptId = Guid.NewGuid();
            var itemId = Guid.NewGuid();
            var occurred = Now.AddDays(-5 + i);

            attempts.Add(new PracticeAttempt
            {
                Id = attemptId,
                SchoolId = schoolId,
                StudentProfileId = studentId,
                CurriculumAdoptionId = adoptionId,
                IsPrivate = false,
                Status = PracticeAttemptStatus.Submitted,
                StartedAtUtc = occurred.AddMinutes(-10),
                SubmittedAtUtc = occurred,
                Score = practiceScores[i],
                MaxScore = 1m,
                Percentage = practiceScores[i] * 100m
            });
            items.Add(new AssessmentItem
            {
                Id = itemId,
                SchoolId = schoolId,
                CurriculumAdoptionId = adoptionId,
                CurriculumTopicId = topicId,
                Difficulty = AssessmentItemDifficulty.Medium,
                Prompt = "Practice",
                CorrectAnswer = "1",
                Solution = "1",
                GenerationFamily = "fractions.equivalent.recognize",
                GenerationParametersJson = JsonSerializer.Serialize(
                    new { exactSkillId }),
                ExposureFingerprint = Guid.NewGuid().ToString("N"),
                CreatedAtUtc = occurred
            });
            learningEvidence.Add(new LearningEvidence
            {
                Id = Guid.NewGuid(),
                SchoolId = schoolId,
                StudentProfileId = studentId,
                LearningOutcomeId = outcomeId,
                PracticeAttemptId = attemptId,
                AssessmentItemId = itemId,
                EvidenceType = LearningEvidenceType.Practice,
                Difficulty = AssessmentItemDifficulty.Medium,
                IsCorrect = practiceScores[i] >= 1m,
                Score = practiceScores[i],
                MaxScore = 1m,
                OccurredAtUtc = occurred
            });
        }

        var snapshot = new AnalyticsProjectionSnapshot(
            [year],
            [classGroup],
            [subject],
            [student],
            [],
            [topic],
            includeUnassessedOutcome
                ? [outcome, additionalOutcome]
                : [outcome],
            [],
            [],
            [],
            [],
            [])
        {
            Terms = [term],
            StudentEnrollments = [enrollment],
            Assessments = assessments,
            AssessmentQuestions = questions,
            OutcomeMappings = mappings,
            AssessmentItems = items,
            AssessmentResults = assessmentResults,
            StudentAnswers = answers,
            PracticeAttempts = attempts,
            LearningEvidence = learningEvidence
        };

        return new Fixture(
            snapshot,
            studentId,
            yearId,
            classId,
            subjectId);
    }

    private sealed record Fixture(
        AnalyticsProjectionSnapshot Snapshot,
        Guid StudentId,
        Guid YearId,
        Guid ClassId,
        Guid SubjectId);
}
