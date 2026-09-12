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
    public void ResultsPaper_IsStrictlyReadOnlyAndTeacherScoreWriteRouteIsRemoved()
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
        Assert.DoesNotContain("SaveStudentResultAsync", controller, StringComparison.Ordinal);
        Assert.DoesNotContain("{id:guid}/results/{studentProfileId:guid}", controller, StringComparison.Ordinal);
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
