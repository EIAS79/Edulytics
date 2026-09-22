namespace Edulytics.Tests.Acceptance;

public sealed class AssessmentResultsBreakdownContractTests
{
    [Fact]
    public void ResultsWorkspace_ExposesStoredStudentResponsesAndExpectedAnswers()
    {
        var root = FindRoot();
        var contracts = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Services/Assessments/AssessmentContracts.cs"));
        var service = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Services/Assessments/AssessmentService.cs"));

        Assert.Contains("QuestionResponses", contracts, StringComparison.Ordinal);
        Assert.Contains("CorrectAnswer", contracts, StringComparison.Ordinal);
        Assert.Contains("_builder.GetContextAsync", service, StringComparison.Ordinal);
        Assert.Contains("x.ResponseText is not null", service, StringComparison.Ordinal);
        Assert.Contains("CorrectAnswer = expected", service, StringComparison.Ordinal);
    }

    [Fact]
    public void ResultsView_ListsStudentsThenShowsOnlySelectedStudentPaper()
    {
        var root = FindRoot();
        var view = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Views/Assessments/Results.cshtml"));

        Assert.Contains("assessment-results-roster", view, StringComparison.Ordinal);
        Assert.Contains("assessment-result-roster-row", view, StringComparison.Ordinal);
        Assert.Contains("assessment-student-link", view, StringComparison.Ordinal);
        Assert.Contains("asp-route-studentProfileId", view, StringComparison.Ordinal);
        Assert.Contains("selectedStudentId", view, StringComparison.Ordinal);
        Assert.Contains("assessment-selected-paper", view, StringComparison.Ordinal);
        Assert.Contains("assessment-breakdown-table", view, StringComparison.Ordinal);
        Assert.Contains("selectedStudent.QuestionResponses.TryGetValue", view, StringComparison.Ordinal);
        Assert.Contains("question.CorrectAnswer", view, StringComparison.Ordinal);
        Assert.Contains("assessment-score-badge", view, StringComparison.Ordinal);
        Assert.Contains("— / @question.MaxScore", view, StringComparison.Ordinal);
        Assert.Contains("No submitted answer recorded", view, StringComparison.Ordinal);
        Assert.Contains("No expected answer recorded", view, StringComparison.Ordinal);
    }

    [Fact]
    public void PublishedOnlineAndOfflineAssessments_KeepTheSameResultsRoute()
    {
        var root = FindRoot();
        var details = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Views/Assessments/Details.cshtml"));

        var publishedStart = details.IndexOf(
            "@if (assessment.Status != AssessmentStatus.Draft)",
            StringComparison.Ordinal);
        var headerEnd = details.IndexOf(
            "</header>",
            publishedStart,
            StringComparison.Ordinal);

        Assert.True(publishedStart >= 0);
        Assert.True(headerEnd > publishedStart);

        var publishedActions = details[publishedStart..headerEnd];

        Assert.Contains(
            "assessment.DeliveryMode == AssessmentDeliveryMode.Offline",
            publishedActions,
            StringComparison.Ordinal);
        Assert.Contains(
            "asp-action=\"Results\"",
            publishedActions,
            StringComparison.Ordinal);
        Assert.DoesNotContain(
            "assessment.DeliveryMode == AssessmentDeliveryMode.Online",
            publishedActions,
            StringComparison.Ordinal);
    }

    [Fact]
    public void ResultsPaper_RemainsReadOnly_AndOfflineEntryRoutesToBulkImport()
    {
        var root = FindRoot();
        var view = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Views/Assessments/Results.cshtml"));
        var controller = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Controllers/AssessmentsController.cs"));

        Assert.DoesNotContain("asp-action=\"SaveResult\"", view, StringComparison.Ordinal);
        Assert.DoesNotContain("name=\"scores\"", view, StringComparison.Ordinal);
        Assert.DoesNotContain("assessment-score-editor", view, StringComparison.Ordinal);
        Assert.DoesNotContain("assessment-save-result-button", view, StringComparison.Ordinal);
        Assert.DoesNotContain("SaveResult(", controller, StringComparison.Ordinal);
        Assert.DoesNotContain("{id:guid}/results/{studentProfileId:guid}", controller, StringComparison.Ordinal);

        Assert.DoesNotContain("DownloadResultsWorkbook", controller, StringComparison.Ordinal);
        Assert.DoesNotContain("ImportResultsWorkbook", controller, StringComparison.Ordinal);
        Assert.DoesNotContain("results-workbook.xlsx", controller, StringComparison.Ordinal);
        Assert.DoesNotContain("asp-action=\"ImportResultsWorkbook\"", view, StringComparison.Ordinal);
        Assert.DoesNotContain("Download Excel results workbook", view, StringComparison.Ordinal);

        Assert.Contains(
            "assessment.DeliveryMode == AssessmentDeliveryMode.Offline",
            view,
            StringComparison.Ordinal);
        Assert.Contains(
            "assessment.Status == AssessmentStatus.Open",
            view,
            StringComparison.Ordinal);
        Assert.Contains(
            "asp-controller=\"Imports\"",
            view,
            StringComparison.Ordinal);
        Assert.Contains(
            "asp-route-assessmentId=\"@assessment.Id\"",
            view,
            StringComparison.Ordinal);
        Assert.Contains(
            "@await Html.PartialAsync(\"_AssessmentPaper\", selectedPaper)",
            view,
            StringComparison.Ordinal);
    }

    [Fact]
    public void SharedAssessmentPaper_IsUsedForResultsAndBulkPreview()
    {
        var root = FindRoot();
        var results = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Views/Assessments/Results.cshtml"));
        var imports = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Views/Imports/Details.cshtml"));
        var partial = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Views/Shared/_AssessmentPaper.cshtml"));

        Assert.Contains("_AssessmentPaper", results, StringComparison.Ordinal);
        Assert.Contains("_AssessmentPaper", imports, StringComparison.Ordinal);
        Assert.Contains("Student answer", partial, StringComparison.Ordinal);
        Assert.Contains("Correct / expected answer", partial, StringComparison.Ordinal);
        Assert.Contains("No submitted answer recorded", partial, StringComparison.Ordinal);
        Assert.Contains("assessment-score-badge", partial, StringComparison.Ordinal);
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
