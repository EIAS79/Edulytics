namespace Edulytics.Tests.Acceptance;

public sealed class AssessmentBulkApprovalAndResultFilteringContractTests
{
    [Fact]
    public void LegacyBulkApprovalRoute_UsesSingleAtomicBulkService()
    {
        var root = FindRoot();
        var controller = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Controllers/AssessmentApprovalRecoveryController.cs"));
        var bulkService = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Services/Assessments/AssessmentBuilderBulkApprovalService.cs"));

        Assert.Contains("[HttpGet(\"drafts\")]", controller, StringComparison.Ordinal);
        Assert.Contains("bulkApproval.ApproveAllDraftQuestionsAsync", controller, StringComparison.Ordinal);
        Assert.DoesNotContain("foreach (var questionId in draftQuestionIds)", controller, StringComparison.Ordinal);
        Assert.DoesNotContain("ReadyForApproval", controller, StringComparison.Ordinal);

        Assert.Contains("draftItems", bulkService, StringComparison.Ordinal);
        Assert.Contains("repository.SaveAsync", bulkService, StringComparison.Ordinal);
        Assert.Contains("No partial bulk approval", controller, StringComparison.Ordinal);
        Assert.Contains("AssessmentPersistenceError.Conflict", bulkService, StringComparison.Ordinal);
    }

    [Fact]
    public void ApprovalOnlyPersistence_KeepsAssessmentConcurrencyAnchorInAtomicSave()
    {
        var root = FindRoot();
        var repository = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Data/Repositories/AssessmentBuilderRepository.cs"));
        var dbContext = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Data/Contexts/EdulyticsDbContext.cs"));

        Assert.Contains("assessmentEntry.Property(x => x.RowVersion).OriginalValue = expectedRowVersion", repository, StringComparison.Ordinal);
        Assert.DoesNotContain("Property(x => x.UpdatedAtUtc).IsModified = false", repository, StringComparison.Ordinal);
        Assert.Contains("await db.SaveChangesAsync(cancellationToken)", repository, StringComparison.Ordinal);
        Assert.Contains("PrepareApplicationManagedConcurrencyTokens", dbContext, StringComparison.Ordinal);
        Assert.Contains("RandomNumberGenerator.GetBytes", dbContext, StringComparison.Ordinal);
    }

    [Fact]
    public void ResultsPage_UsesStudentRosterFiltersAndPerStudentPaperLinks()
    {
        var root = FindRoot();
        var fallback = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/wwwroot/js/assessment-results-filter-fallback-v35.js"));
        var view = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Views/Assessments/Results.cshtml"));
        var layout = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Views/Shared/_Layout.cshtml"));
        var styles = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/wwwroot/css/round2-product-fixes.css"));

        Assert.Contains("assessment-results-filter-fallback-v35.js", layout, StringComparison.Ordinal);
        Assert.Contains(".assessment-results-roster", fallback, StringComparison.Ordinal);
        Assert.Contains(".assessment-result-roster-row", fallback, StringComparison.Ordinal);
        Assert.Contains("Search by student name", fallback, StringComparison.Ordinal);
        Assert.Contains("with-result", fallback, StringComparison.Ordinal);
        Assert.Contains("without-result", fallback, StringComparison.Ordinal);
        Assert.Contains("pageSize.value = \"5\"", fallback, StringComparison.Ordinal);
        Assert.Contains("assessment-results-filter-summary", fallback, StringComparison.Ordinal);
        Assert.Contains("data-has-result", view, StringComparison.Ordinal);
        Assert.Contains("asp-route-studentProfileId", view, StringComparison.Ordinal);
        Assert.Contains("assessment-student-link", view, StringComparison.Ordinal);
        Assert.Contains("assessment-selected-paper", view, StringComparison.Ordinal);
        Assert.Contains("View paper", view, StringComparison.Ordinal);
        Assert.Contains("assessment-results-filter-bar", styles, StringComparison.Ordinal);
        Assert.Contains("assessment-results-pager", styles, StringComparison.Ordinal);
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

        throw new DirectoryNotFoundException("Edulytics solution root not found.");
    }
}
