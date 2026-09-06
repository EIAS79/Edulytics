using Edulytics.Web.Presentation;

namespace Edulytics.Tests.Acceptance;

public sealed class Round4ManualAcceptanceTests
{
    [Fact]
    public void Student_dashboard_bounds_long_assessment_and_result_lists()
    {
        var view = ReadRepositoryFile("src", "Edulytics.Web", "Views", "StudentPortal", "Dashboard.cshtml");
        var css = ReadRepositoryFile("src", "Edulytics.Web", "wwwroot", "css", "round4-ux-display.css");

        Assert.Contains("student-dashboard-list-panel", view, StringComparison.Ordinal);
        Assert.DoesNotContain("Assessments.Take(4)", view, StringComparison.Ordinal);
        Assert.Contains("max-height: 246px", css, StringComparison.Ordinal);
        Assert.Contains("overflow-y: auto", css, StringComparison.Ordinal);
    }

    [Fact]
    public void Student_lesson_cards_remove_math_badge_prioritize_stage_and_align_actions()
    {
        var view = ReadRepositoryFile("src", "Edulytics.Web", "Views", "StudentPortal", "Learning.cshtml");
        var css = ReadRepositoryFile("src", "Edulytics.Web", "wwwroot", "css", "round4-ux-display.css");

        Assert.DoesNotContain("student-subject-code\">@lesson.SubjectCode", view, StringComparison.Ordinal);
        Assert.Contains("@lesson.GradeName · Mathematics", view, StringComparison.Ordinal);
        Assert.Contains("student-lesson-primary-context", view, StringComparison.Ordinal);
        Assert.Contains("margin-top: auto", css, StringComparison.Ordinal);
        Assert.Contains("justify-content: center", css, StringComparison.Ordinal);
        Assert.Contains("align-items: center", css, StringComparison.Ordinal);
    }

    [Fact]
    public void Student_assessment_cards_remove_redundant_subject_badge_and_center_score_columns()
    {
        var view = ReadRepositoryFile("src", "Edulytics.Web", "Views", "StudentPortal", "Assessments.cshtml");
        var css = ReadRepositoryFile("src", "Edulytics.Web", "wwwroot", "css", "round4-ux-display.css");

        Assert.DoesNotContain("student-subject-code\">@item.SubjectName", view, StringComparison.Ordinal);
        Assert.Contains("grid-template-columns: minmax(135px, 1fr) minmax(135px, 1fr)", css, StringComparison.Ordinal);
        Assert.Contains("gap: 28px", css, StringComparison.Ordinal);
        Assert.Contains("text-align: center", css, StringComparison.Ordinal);
    }

    [Theory]
    [InlineData("7Ae.05", "Edulytics reference-only Cambridge Mathematics entry.", "Algebraic expressions · 05")]
    [InlineData("7Gg.04", "Edulytics reference-only Cambridge Mathematics entry.", "Geometry · 04")]
    [InlineData("1Nc.01", "Edulytics reference-only Cambridge Mathematics entry.", "Number calculations · 01")]
    public void Reference_only_outcomes_use_readable_edulytics_titles(string code, string description, string expected)
    {
        Assert.Equal(expected, LearningOutcomePresentation.DisplayTitle(code, description));
    }

    [Fact]
    public void Reference_family_topic_heading_is_readable()
    {
        Assert.Equal(
            "Stage 1 · Number calculations",
            LearningOutcomePresentation.DisplayTopicTitle("0096 Stage 1 reference family Nc"));
    }

    [Fact]
    public void Assessment_builder_offers_concurrency_aware_approve_all_without_changing_ai_product_name()
    {
        var view = ReadRepositoryFile("src", "Edulytics.Web", "Views", "AssessmentBuilder", "Index.cshtml");
        var controller = ReadRepositoryFile("src", "Edulytics.Web", "Controllers", "AssessmentBuilderController.cs");

        Assert.Contains("asp-action=\"ApproveAll\"", view, StringComparison.Ordinal);
        Assert.Contains("Approve all draft questions", view, StringComparison.Ordinal);
        Assert.Contains("@A[\"GenerateQuestionsWithAI\"]", view, StringComparison.Ordinal);
        Assert.Contains("[HttpPost(\"questions/approve-all\")", controller, StringComparison.Ordinal);
        Assert.Contains("SequenceEqual(version)", controller, StringComparison.Ordinal);
        Assert.Contains("AssessmentBuilderQuestionStatus.Draft", controller, StringComparison.Ordinal);
    }

    [Fact]
    public void Polish_public_header_prevents_wrapping_and_collapses_navigation_earlier()
    {
        var css = ReadRepositoryFile("src", "Edulytics.Web", "wwwroot", "css", "round4-ux-display.css");

        Assert.Contains("white-space: nowrap", css, StringComparison.Ordinal);
        Assert.Contains("@media (max-width: 1120px)", css, StringComparison.Ordinal);
        Assert.Contains(".ed-home-links", css, StringComparison.Ordinal);
        Assert.Contains("display: none", css, StringComparison.Ordinal);
    }

    private static string ReadRepositoryFile(params string[] relativeSegments)
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null && !File.Exists(Path.Combine(directory.FullName, "Edulytics.sln")))
            directory = directory.Parent;

        var root = directory?.FullName
            ?? throw new InvalidOperationException("Repository root not found.");
        return File.ReadAllText(Path.Combine([root, .. relativeSegments]));
    }
}
