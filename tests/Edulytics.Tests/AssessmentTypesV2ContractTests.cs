using System.Reflection;
using Edulytics.Core.Entities;
using Edulytics.Core.Enums;
using Edulytics.Data.Contexts;
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
