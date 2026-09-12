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
    public void ResultsView_SeparatesQuestionStudentAnswerExpectedAnswerAndScore()
    {
        var root = FindRoot();
        var view = File.ReadAllText(Path.Combine(
            root,
            "src/Edulytics.Web/Views/Assessments/Results.cshtml"));

        Assert.Contains("assessment-breakdown-table", view, StringComparison.Ordinal);
        Assert.Contains("student.QuestionResponses.TryGetValue", view, StringComparison.Ordinal);
        Assert.Contains("question.CorrectAnswer", view, StringComparison.Ordinal);
        Assert.Contains("assessment-score-badge", view, StringComparison.Ordinal);
        Assert.Contains("— / @question.MaxScore", view, StringComparison.Ordinal);
        Assert.Contains("No submitted answer recorded", view, StringComparison.Ordinal);
        Assert.Contains("No expected answer recorded", view, StringComparison.Ordinal);
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
