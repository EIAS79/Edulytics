using Edulytics.Web.Presentation;

namespace Edulytics.Tests.Acceptance;

public sealed class Round6ManualAcceptanceRegressionTests
{
    [Fact]
    public void Cambridge_Nc_reference_labels_are_not_generic_number_labels()
    {
        const string referenceOnly = "Edulytics reference-only curriculum locator";

        Assert.Equal(
            "Number calculations · 01",
            LearningOutcomePresentation.DisplayTitle("1Nc.01", referenceOnly));
        Assert.NotEqual(
            "Number · 01",
            LearningOutcomePresentation.DisplayTitle("1Nc.01", referenceOnly));
    }

    [Fact]
    public void Student_placement_picker_is_searchable_bounded_and_compact()
    {
        var source = ReadRepositoryFile(
            "src", "Edulytics.Web", "TagHelpers", "AcademicStructureNormalUiTagHelper.cs");

        Assert.Contains("Search by student name or number", source, StringComparison.Ordinal);
        Assert.Contains("max-height:360px", source, StringComparison.Ordinal);
        Assert.Contains("Select visible", source, StringComparison.Ordinal);
        Assert.Contains("Clear selection", source, StringComparison.Ordinal);
        Assert.Contains("selected ·", source, StringComparison.Ordinal);
    }

    [Fact]
    public void Approval_forms_use_stable_recovery_routes()
    {
        var tagHelper = ReadRepositoryFile(
            "src", "Edulytics.Web", "TagHelpers", "AssessmentBuilderApprovalFormTagHelper.cs");
        var controller = ReadRepositoryFile(
            "src", "Edulytics.Web", "Controllers", "AssessmentApprovalRecoveryController.cs");
        var bulkService = ReadRepositoryFile(
            "src", "Edulytics.Services", "Assessments", "AssessmentBuilderBulkApprovalService.cs");

        Assert.Contains("/builder/approval/drafts", tagHelper, StringComparison.Ordinal);
        Assert.Contains("/builder/approval/question/", tagHelper, StringComparison.Ordinal);
        Assert.Contains("HttpPost(\"drafts\")", controller, StringComparison.Ordinal);
        Assert.Contains("HttpGet(\"drafts\")", controller, StringComparison.Ordinal);
        Assert.Contains("bulkApproval.ApproveAllDraftQuestionsAsync", controller, StringComparison.Ordinal);
        Assert.DoesNotContain("ReadyForApproval", controller, StringComparison.Ordinal);
        Assert.Contains("repository.SaveAsync", bulkService, StringComparison.Ordinal);
    }

    [Fact]
    public void Approval_only_persistence_does_not_force_assessment_row_update()
    {
        var repository = ReadRepositoryFile(
            "src", "Edulytics.Data", "Repositories", "AssessmentBuilderRepository.cs");

        Assert.Contains("approvalOnly", repository, StringComparison.Ordinal);
        Assert.Contains("AssessmentItem.ValidationMetadataJson", repository, StringComparison.Ordinal);
        Assert.Contains("UpdatedAtUtc).IsModified = false", repository, StringComparison.Ordinal);
    }

    [Fact]
    public void Student_private_practice_renders_multiple_choice_as_stacked_radio_cards()
    {
        var view = ReadRepositoryFile(
            "src", "Edulytics.Web", "Views", "StudentPractice", "Attempt.cshtml");

        Assert.Contains("practice-choice-list", view, StringComparison.Ordinal);
        Assert.Contains("type=\"radio\" name=\"answer\"", view, StringComparison.Ordinal);
        Assert.Contains("practice-choice-letter", view, StringComparison.Ordinal);
        Assert.Contains("grid", view, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Finishing_private_AI_practice_does_not_write_official_mastery_evidence()
    {
        var service = ReadRepositoryFile(
            "src", "Edulytics.Services", "Practice", "PracticeService.cs");

        Assert.Contains("if (!attempt.IsPrivate)", service, StringComparison.Ordinal);
        Assert.Contains("Student private AI practice is deliberately separated from official", service, StringComparison.Ordinal);
        Assert.Contains("IReadOnlyList<LearningEvidence> evidence = [];", service, StringComparison.Ordinal);
        Assert.Contains("CompleteAttemptAsync(attempt, evidence", service, StringComparison.Ordinal);
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
        var pathSegments = new[] { root }.Concat(relativeSegments).ToArray();
        return File.ReadAllText(Path.Combine(pathSegments));
    }
}
