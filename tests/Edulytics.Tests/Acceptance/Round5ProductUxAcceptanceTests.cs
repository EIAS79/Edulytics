using Edulytics.Web.Presentation;

namespace Edulytics.Tests.Acceptance;

public sealed class Round5ProductUxAcceptanceTests
{
    [Fact]
    public void Cambridge_reference_only_outcomes_use_human_readable_titles()
    {
        const string referenceOnly = "Edulytics reference-only curriculum locator";

        Assert.Equal(
            "Thinking and Working Mathematically · 01",
            LearningOutcomePresentation.DisplayTitle("TWM.01", referenceOnly));
        Assert.Equal(
            "Whole numbers and place value · 01",
            LearningOutcomePresentation.DisplayTitle("1Ni.01", referenceOnly));
        Assert.Equal(
            "Number and proportional reasoning · 01",
            LearningOutcomePresentation.DisplayTitle("1Np.01", referenceOnly));
    }

    [Fact]
    public void Student_lesson_metadata_has_explicit_left_aligned_context_contract()
    {
        var view = ReadRepositoryFile(
            "src", "Edulytics.Web", "Views", "StudentPortal", "Learning.cshtml");
        var css = ReadRepositoryFile(
            "src", "Edulytics.Web", "wwwroot", "css", "round5-ux-fixes.css");

        Assert.Contains("student-lesson-context", view, StringComparison.Ordinal);
        Assert.Contains("text-align: left", css, StringComparison.Ordinal);
        Assert.Contains("align-items: flex-start", css, StringComparison.Ordinal);
    }

    [Fact]
    public void Class_selection_labels_put_grade_before_curriculum()
    {
        var source = ReadRepositoryFile(
            "src", "Edulytics.Services", "Curriculum", "ExplicitCurriculumLevelUiQuery.cs");

        Assert.Contains("BuildClassCurriculumLabel", source, StringComparison.Ordinal);
        Assert.Contains("$\"Grade {logicalLevel} · {curriculum}\"", source, StringComparison.Ordinal);
        Assert.Contains("return $\"{Name} · {level}\"", source, StringComparison.Ordinal);
    }

    [Fact]
    public void Assessment_maximum_score_is_locked_in_edit_ui_once_questions_exist()
    {
        var view = ReadRepositoryFile(
            "src", "Edulytics.Web", "Views", "Assessments", "Edit.cshtml");

        Assert.Contains("var hasQuestions = Model.Details.Questions.Count > 0", view, StringComparison.Ordinal);
        Assert.Contains("@if (hasQuestions)", view, StringComparison.Ordinal);
        Assert.Contains("type=\"hidden\" name=\"maxScore\"", view, StringComparison.Ordinal);
        Assert.Contains("readonly", view, StringComparison.Ordinal);
    }

    [Fact]
    public void Assessment_maximum_score_lock_is_enforced_server_side()
    {
        var filter = ReadRepositoryFile(
            "src", "Edulytics.Web", "Filters", "AssessmentMaxScoreLockFilter.cs");
        var registration = ReadRepositoryFile(
            "src", "Edulytics.Web", "Extensions", "AssessmentRegistrationExtensions.cs");

        Assert.Contains("details.Value.Questions.Count > 0", filter, StringComparison.Ordinal);
        Assert.Contains("context.ActionArguments[\"maxScore\"]", filter, StringComparison.Ordinal);
        Assert.Contains("details.Value.Assessment.MaxScore", filter, StringComparison.Ordinal);
        Assert.Contains("AddService<AssessmentMaxScoreLockFilter>", registration, StringComparison.Ordinal);
    }

    [Fact]
    public void Bulk_student_placement_allows_same_grade_moves_and_blocks_cross_grade_moves()
    {
        var service = ReadRepositoryFile(
            "src", "Edulytics.Services", "Academics", "StudentPlacementService.cs");
        var ui = ReadRepositoryFile(
            "src", "Edulytics.Web", "TagHelpers", "AcademicStructureNormalUiTagHelper.cs");
        var controller = ReadRepositoryFile(
            "src", "Edulytics.Web", "Controllers", "AcademicStructureBulkController.cs");

        Assert.Contains("sourceClass.GradeLevelId != targetClass.GradeLevelId", service, StringComparison.Ordinal);
        Assert.Contains("CrossGradeMoveNotAllowed", service, StringComparison.Ordinal);
        Assert.Contains("existing.ClassGroupId = targetClass.Id", service, StringComparison.Ordinal);
        Assert.Contains("studentProfileIds", ui, StringComparison.Ordinal);
        Assert.Contains("student-placements/bulk", ui, StringComparison.Ordinal);
        Assert.Contains("PlaceStudentsAsync", controller, StringComparison.Ordinal);
    }

    [Fact]
    public void Approve_all_keeps_valid_partial_successes_and_returns_to_builder()
    {
        var controller = ReadRepositoryFile(
            "src", "Edulytics.Web", "Controllers", "AssessmentBuilderController.cs");

        Assert.Contains("var approvedCount = 0", controller, StringComparison.Ordinal);
        Assert.Contains("var reviewCount = 0", controller, StringComparison.Ordinal);
        Assert.Contains("reviewCount++", controller, StringComparison.Ordinal);
        Assert.Contains("remain for teacher review", controller, StringComparison.Ordinal);
        Assert.DoesNotContain("if (!result.Succeeded)\n            {\n                Feedback(result", controller, StringComparison.Ordinal);
    }

    private static string ReadRepositoryFile(params string[] relativeSegments)
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null &&
               !File.Exists(Path.Combine(directory.FullName, "Edulytics.sln")))
        {
            directory = directory.Parent;
        }

        var root = directory?.FullName
            ?? throw new InvalidOperationException("Repository root not found.");
        return File.ReadAllText(Path.Combine([root, .. relativeSegments]));
    }
}
