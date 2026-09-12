namespace Edulytics.Tests.Acceptance;

public sealed class AssessmentBulkApprovalAndResultFilteringContractTests
{
    [Fact]
    public void LegacyBulkApprovalRoute_UsesCurrentWorkspaceForEveryDraft()
    {
        var root = FindRoot();
        var controller = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Controllers/AssessmentApprovalRecoveryController.cs"));

        Assert.Contains("[HttpGet(\"drafts\")]", controller, StringComparison.Ordinal);
        Assert.Contains("foreach (var questionId in draftQuestionIds)", controller, StringComparison.Ordinal);
        Assert.Contains("var current = await service.GetWorkspaceAsync", controller, StringComparison.Ordinal);
        Assert.Contains("current.Value.Details.Assessment.RowVersion", controller, StringComparison.Ordinal);
        Assert.DoesNotContain("ReadyForApproval", controller, StringComparison.Ordinal);
    }

    [Fact]
    public void ResultsPage_ClientFilterLimitsLargeClassesAndSupportsResultStatus()
    {
        var root = FindRoot();
        var script = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/wwwroot/js/site.js"));
        var styles = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/wwwroot/css/round2-product-fixes.css"));

        Assert.Contains("wireAssessmentResultFilters", script, StringComparison.Ordinal);
        Assert.Contains(".assessment-results-grid-v2", script, StringComparison.Ordinal);
        Assert.Contains("Search by student name or number", script, StringComparison.Ordinal);
        Assert.Contains("with-result", script, StringComparison.Ordinal);
        Assert.Contains("without-result", script, StringComparison.Ordinal);
        Assert.Contains("pageSize.value = \"5\"", script, StringComparison.Ordinal);
        Assert.Contains("assessment-results-filter-summary", script, StringComparison.Ordinal);
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
