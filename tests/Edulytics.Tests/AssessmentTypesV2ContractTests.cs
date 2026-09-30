using System.Reflection;
using Edulytics.Core.Entities;
using Edulytics.Core.Enums;
using Edulytics.Data.Contexts;
using Edulytics.Services.Assessments;
using Edulytics.Services.StudentPortal;
using Microsoft.EntityFrameworkCore;

namespace Edulytics.Tests;

public sealed class AssessmentTypesV2ContractTests
{
    [Fact]
    public void LegacyAssessment_DefaultsToExam()
    {
        var assessment = new Assessment();

        Assert.Equal(AssessmentType.Exam, assessment.AssessmentType);
        Assert.True(assessment.AssessmentType == AssessmentType.Exam);
    }

    [Fact]
    public void HomeworkAndWorksheetResponseContract_HasNoNumericScore()
    {
        var properties = typeof(AssessmentTaskResponse)
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Select(x => x.Name)
            .ToArray();

        Assert.DoesNotContain("Score", properties);
        Assert.DoesNotContain("Percentage", properties);
        Assert.DoesNotContain("MaxScore", properties);
    }

    [Fact]
    public void AssessmentAttempt_IsUniquePerStudentAndAssessment()
    {
        using var db = CreateDb();

        var entity = db.Model.FindEntityType(typeof(AssessmentAttempt));
        Assert.NotNull(entity);

        Assert.Contains(
            entity!.GetIndexes(),
            index =>
                index.IsUnique &&
                index.Properties
                    .Select(x => x.Name)
                    .SequenceEqual(
                        new[]
                        {
                            nameof(AssessmentAttempt.SchoolId),
                            nameof(AssessmentAttempt.AssessmentId),
                            nameof(AssessmentAttempt.StudentProfileId)
                        }));
    }

    [Fact]
    public void Migration_BackfillsHistoricalAssessmentsAsExam()
    {
        var source = File.ReadAllText(
            Path.Combine(
                Root(),
                "src/Edulytics.Data/Migrations/20260930223000_AddAssessmentTypesAndAttempts.cs"));

        Assert.Contains("name: \"AssessmentType\"", source);
        Assert.Contains("defaultValue: 1", source);
    }

    [Fact]
    public void OfficialEvaluation_HardFiltersToExamEvidence()
    {
        var root = Root();

        foreach (var relativePath in new[]
        {
            "src/Edulytics.Services/Analytics/EvaluationEvidenceNormalizer.cs",
            "src/Edulytics.Services/Analytics/MasteryEvidenceEngine.cs"
        })
        {
            var source = File.ReadAllText(Path.Combine(root, relativePath));

            Assert.Contains(
                "x.AssessmentType == AssessmentType.Exam",
                source);
        }
    }

    [Fact]
    public void WorksheetPdfRenderer_CanOmitAllMarks()
    {
        var root = Root();
        var renderer = File.ReadAllText(
            Path.Combine(
                root,
                "src/Edulytics.Web/Printing/AssessmentPdfRenderer.cs"));
        var controller = File.ReadAllText(
            Path.Combine(
                root,
                "src/Edulytics.Web/Controllers/AssessmentBuilderController.cs"));

        Assert.Contains("bool showMarks = true", renderer);
        Assert.Contains("if (showMarks)", renderer);
        Assert.Contains(
            "showMarks: result.Value.Details.Assessment.AssessmentType == AssessmentType.Exam",
            controller);
    }

    [Fact]
    public void StudentPortal_SeparatesThreeTaskTypes()
    {
        var source = File.ReadAllText(
            Path.Combine(
                Root(),
                "src/Edulytics.Web/Views/StudentPortal/Assessments.cshtml"));

        Assert.Contains("AssessmentType.Exam", source);
        Assert.Contains("AssessmentType.Homework", source);
        Assert.Contains("AssessmentType.Worksheet", source);
        Assert.Contains("no deadline", source, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void StudentResultRelease_RequiresExplicitPublication()
    {
        Assert.False(
            OfficialAssessmentResultReleasePolicy.CanStudentView(
                AssessmentStatus.Open,
                AssessmentResultReleaseStatus.Published));

        Assert.False(
            OfficialAssessmentResultReleasePolicy.CanStudentView(
                AssessmentStatus.Closed,
                AssessmentResultReleaseStatus.Withheld));

        Assert.True(
            OfficialAssessmentResultReleasePolicy.CanStudentView(
                AssessmentStatus.Closed,
                AssessmentResultReleaseStatus.Published));
    }

    [Fact]
    public void LearningTaskSubmission_DoesNotCreateNumericAssessmentResult()
    {
        var source = File.ReadAllText(
            Path.Combine(
                Root(),
                "src/Edulytics.Services/Assessments/StudentAssessmentDeliveryService.cs"));

        var start = source.LastIndexOf(
            "private async Task<StudentAssessmentDeliveryResult<StudentAssessmentSubmission>> SubmitLearningTaskAsync(",
            StringComparison.Ordinal);
        var end = source.IndexOf(
            "private async Task<ResolvedDelivery> ResolveAsync(",
            start,
            StringComparison.Ordinal);

        Assert.True(start >= 0);
        Assert.True(end > start);

        var method = source[start..end];

        Assert.Contains("AssessmentTaskResponse", method);
        Assert.Contains("AssessmentAttemptStatus.Submitted", method);
        Assert.Contains("AssessmentAttemptStatus.Completed", method);
        Assert.DoesNotContain("new AssessmentResult", method);
        Assert.DoesNotContain("new StudentAnswer", method);
    }

    [Fact]
    public void LearningTaskProgressSave_DoesNotSubmitCompleteOrScore()
    {
        var source = File.ReadAllText(
            Path.Combine(
                Root(),
                "src/Edulytics.Services/Assessments/StudentAssessmentDeliveryService.cs"));

        var start = source.IndexOf(
            "SaveProgressAsync(",
            StringComparison.Ordinal);
        var end = source.IndexOf(
            "public async Task<StudentAssessmentDeliveryResult<StudentAssessmentSubmission>> SubmitAsync(",
            start,
            StringComparison.Ordinal);

        Assert.True(start >= 0);
        Assert.True(end > start);

        var method = source[start..end];

        Assert.Contains("AssessmentTaskResponse", method);
        Assert.Contains("ProgressSaved", method);
        Assert.DoesNotContain(
            "attempt.Status = AssessmentAttemptStatus.Submitted",
            method);
        Assert.DoesNotContain(
            "attempt.Status = AssessmentAttemptStatus.Completed",
            method);
        Assert.DoesNotContain("new AssessmentResult", method);
    }

    [Fact]
    public void ScheduledExam_ServerGuardsStartWindowAndAttemptDeadline()
    {
        var source = File.ReadAllText(
            Path.Combine(
                Root(),
                "src/Edulytics.Services/Assessments/StudentAssessmentDeliveryService.cs"));

        Assert.Contains(
            "nowUtc < assessment.AvailableFromUtc.Value",
            source);
        Assert.Contains(
            "nowUtc >= assessment.DueAtUtc.Value",
            source);
        Assert.Contains(
            "ResolveEffectiveAttemptDeadline",
            source);
        Assert.Contains(
            "StudentAssessmentDeliveryErrorCode.AttemptExpired",
            source);
    }

    [Fact]
    public void ScheduledExam_RemainsEditableOnlyBeforeStartWithoutAttemptsOrResults()
    {
        var source = File.ReadAllText(
            Path.Combine(
                Root(),
                "src/Edulytics.Services/Assessments/AssessmentService.Support.cs"));

        Assert.Contains(
            "DateTime.UtcNow >= assessment.AvailableFromUtc.Value",
            source);
        Assert.Contains(
            "!snapshot.AssessmentAttempts.Any",
            source);
        Assert.Contains(
            "!snapshot.Results.Any",
            source);
    }

    [Fact]
    public void StudentUx_ProvidesProgressSaveAndTimedAutoSubmit()
    {
        var source = File.ReadAllText(
            Path.Combine(
                Root(),
                "src/Edulytics.Web/Views/StudentPortal/TakeAssessment.cshtml"));

        Assert.Contains("SaveAssessmentProgress", source);
        Assert.Contains("assessment-countdown", source);
        Assert.Contains("requestSubmit", source);
    }

    [Fact]
    public void ResultReleaseMigration_PreservesHistoricalClosedResultVisibility()
    {
        var source = File.ReadAllText(
            Path.Combine(
                Root(),
                "src/Edulytics.Data/Migrations/20260930225500_AddExplicitAssessmentResultRelease.cs"));

        Assert.Contains("\\\"Status\\\" = 3", source);
        Assert.Contains("\\\"ResultReleaseStatus\\\" = 2", source);
    }

    [Fact]
    public void ObservabilityContract_CoversAssessmentLifecycle()
    {
        var root = Root();
        var metrics = File.ReadAllText(
            Path.Combine(
                root,
                "src/Edulytics.Services/Assessments/AssessmentMetrics.cs"));
        var commands = File.ReadAllText(
            Path.Combine(
                root,
                "src/Edulytics.Services/Assessments/AssessmentService.Commands.cs"));
        var delivery = File.ReadAllText(
            Path.Combine(
                root,
                "src/Edulytics.Services/Assessments/StudentAssessmentDeliveryService.cs"));

        foreach (var metricName in new[]
                 {
                     "exam_scheduled_open_success",
                     "exam_access_denied_before_start",
                     "homework_submitted_before_due",
                     "homework_missed_due",
                     "worksheet_online_completed",
                     "worksheet_pdf_generated",
                     "non_exam_evaluation_evidence_blocked"
                 })
        {
            Assert.Contains(metricName, metrics);
        }

        Assert.Contains("Assessment.Opened", commands);
        Assert.Contains("Assessment.Closed", commands);
        Assert.Contains("Assessment.ResultsPublished", commands);
        Assert.Contains("StudentHomework.ProgressSaved", delivery);
        Assert.Contains("StudentHomework.Submitted", delivery);
        Assert.Contains("StudentWorksheet.Completed", delivery);
    }

    private static EdulyticsDbContext CreateDb()
    {
        var options = new DbContextOptionsBuilder<EdulyticsDbContext>()
            .UseInMemoryDatabase($"assessment-types-v2-{Guid.NewGuid():N}")
            .Options;

        return new EdulyticsDbContext(options);
    }

    private static string Root()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);

        while (directory is not null)
        {
            if (File.Exists(Path.Combine(directory.FullName, "Edulytics.sln")))
                return directory.FullName;

            directory = directory.Parent;
        }

        throw new DirectoryNotFoundException();
    }
}
