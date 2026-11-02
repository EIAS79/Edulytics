using Edulytics.Web.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Edulytics.Tests.Phase44;

public sealed class LearningEvaluationUiContractTests
{
    [Fact]
    public void AnalyticsController_ExposesEvaluationDrilldowns()
    {
        var controller = typeof(AnalyticsController);

        Assert.NotNull(controller.GetMethod(nameof(AnalyticsController.Students)));
        Assert.NotNull(controller.GetMethod(nameof(AnalyticsController.Student)));
        Assert.NotNull(controller.GetMethod(nameof(AnalyticsController.TopicsSkills)));

        Assert.Contains(
            controller.GetMethod(nameof(AnalyticsController.Students))!
                .GetCustomAttributes(typeof(HttpGetAttribute), false)
                .Cast<HttpGetAttribute>(),
            x => x.Template == "students");

        Assert.Contains(
            controller.GetMethod(nameof(AnalyticsController.TopicsSkills))!
                .GetCustomAttributes(typeof(HttpGetAttribute), false)
                .Cast<HttpGetAttribute>(),
            x => x.Template == "topics-skills");
    }

    [Fact]
    public void EvaluationViews_ShowAllStudentsStudent360AndExplainableEvidence()
    {
        var root = FindRoot();

        var students = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Views/Analytics/Students.cshtml"));
        var student = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Views/Analytics/Student.cshtml"));
        var topics = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Views/Analytics/TopicsSkills.cshtml"));

        Assert.Contains("evaluation-student-list", students);
        Assert.Contains("evaluation-distribution-donut", students);
        Assert.Contains("evaluation-student-card-row", students);
        Assert.Contains("evaluation-student-card-actions", students);
        Assert.Contains("Mastery grading", students);
        Assert.Contains("Evidence confidence", students);
        Assert.DoesNotContain("evaluation-students-table", students);
        Assert.Contains("Model.Students", students);
        Assert.DoesNotContain("RiskStudents", students);

        Assert.Contains("evaluation-donut", student);
        Assert.Contains("EvidenceDetails", student);
        Assert.Contains("WeakPrerequisiteSkillKeys", student);
        Assert.Contains("PracticeToAssessmentGapPercentagePoints", student);
        Assert.Contains("ComparableSkillGrowth", student);
        Assert.Contains("Model.Terms", student);
        Assert.Contains("Model.Practice", student);
        Assert.Contains("Model.Recommendations", student);
        Assert.Contains("RecommendedPrerequisitePath", student);

        Assert.Contains("AffectedStudentCount", topics);
        Assert.Contains("evaluation-skill-cohort", topics);
        Assert.Contains("ViewEvaluation", topics);
    }

    [Fact]
    public void EvaluationCss_HasResponsiveLayouts()
    {
        var root = FindRoot();
        var css = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/wwwroot/css/site.css"));

        Assert.Contains(
            "Evaluation Engine UI — Student 360 / cohort / topic-skill analytics",
            css);
        Assert.Contains(".evaluation-distribution-grid", css);
        Assert.Contains(".analytics-overview-layout", css);
        Assert.Contains(".evaluation-student-card-row", css);
        Assert.Contains(".evaluation-distribution-donut", css);
        Assert.Contains(".evaluation-student-grade", css);
        Assert.Contains(".evaluation-mastery-track", css);
        Assert.Contains(".evaluation-hero-grid", css);
        Assert.Contains(".evaluation-topic-analytics-grid", css);
        Assert.Contains(".evaluation-practice-grid", css);
        Assert.Contains(".evaluation-recommendation-list", css);
        Assert.Contains(".evaluation-term-table", css);
        Assert.Contains("@media (max-width: 640px)", css);
    }

    [Fact]
    public void EvaluationService_UsesComparableSkillGrowth_NotRawOnlyComparison()
    {
        var root = FindRoot();
        var service = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Services/Analytics/AnalyticsService.cs"));

        Assert.Contains("ComparableSkillDelta", service);
        Assert.Contains("BuildTermEvaluationRows", service);
        Assert.Contains("BuildPracticeSummary", service);
        Assert.Contains("BuildRecommendations", service);
        Assert.Contains("BuildWeakPrerequisitePath", service);
        Assert.Contains("MathematicsSkillMetadataRegistry", service);
    }

    private static string FindRoot()
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
