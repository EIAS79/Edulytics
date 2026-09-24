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

        Assert.Contains("evaluation-students-table", students);
        Assert.Contains("Model.Students", students);
        Assert.DoesNotContain("RiskStudents", students);

        Assert.Contains("evaluation-score-ring", student);
        Assert.Contains("EvidenceDetails", student);
        Assert.Contains("WeakPrerequisiteSkillKeys", student);
        Assert.Contains("PracticeToAssessmentGapPercentagePoints", student);

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
        Assert.Contains(".evaluation-hero-grid", css);
        Assert.Contains(".evaluation-topic-analytics-grid", css);
        Assert.Contains("@media (max-width: 640px)", css);
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
