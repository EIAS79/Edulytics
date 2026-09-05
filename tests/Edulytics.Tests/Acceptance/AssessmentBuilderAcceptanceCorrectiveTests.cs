namespace Edulytics.Tests.Acceptance;

public sealed class AssessmentBuilderAcceptanceCorrectiveTests
{
    [Fact]
    public void AssessmentAndQuestionMarks_AreIntegerOnlyInBackendAndUi()
    {
        var assessmentSupport = ReadRepositoryFile(
            "src", "Edulytics.Services", "Assessments", "AssessmentService.Support.cs");
        var builderService = ReadRepositoryFile(
            "src", "Edulytics.Services", "Assessments", "AssessmentBuilderService.cs");
        var assessmentIndex = ReadRepositoryFile(
            "src", "Edulytics.Web", "Views", "Assessments", "Index.cshtml");
        var assessmentEdit = ReadRepositoryFile(
            "src", "Edulytics.Web", "Views", "Assessments", "Edit.cshtml");
        var builderView = ReadRepositoryFile(
            "src", "Edulytics.Web", "Views", "AssessmentBuilder", "Index.cshtml");

        Assert.Contains("decimal.Truncate(value) == value", assessmentSupport, StringComparison.Ordinal);
        Assert.Contains("decimal.Truncate(value) == value", builderService, StringComparison.Ordinal);
        Assert.Contains("min=\"1\" max=\"10000\"", assessmentIndex, StringComparison.Ordinal);
        Assert.Contains("min=\"1\" max=\"10000\" step=\"1\"", assessmentEdit, StringComparison.Ordinal);
        Assert.Contains("name=\"maxScore\" type=\"number\" min=\"1\" max=\"10000\" step=\"1\"", builderView, StringComparison.Ordinal);
        Assert.Contains("name=\"maxScorePerQuestion\" type=\"number\" min=\"1\" step=\"1\"", builderView, StringComparison.Ordinal);
        Assert.DoesNotContain("step=\"0.01\"", builderView, StringComparison.Ordinal);
    }

    [Fact]
    public void ManualSolution_IsOptionalWhileStillStored()
    {
        var builderService = ReadRepositoryFile(
            "src", "Edulytics.Services", "Assessments", "AssessmentBuilderService.cs");
        var builderView = ReadRepositoryFile(
            "src", "Edulytics.Web", "Views", "AssessmentBuilder", "Index.cshtml");

        Assert.Contains("solution.Length <= 4000", builderService, StringComparison.Ordinal);
        Assert.DoesNotContain("solution.Length is >= 1", builderService, StringComparison.Ordinal);
        Assert.Contains("Solution = solution", builderService, StringComparison.Ordinal);
        Assert.Contains("<textarea name=\"solution\" maxlength=\"4000\" rows=\"3\"></textarea>", builderView, StringComparison.Ordinal);
    }

    [Fact]
    public void FailedManualQuestionSave_PreservesEnteredValuesUntilSuccess()
    {
        var controller = ReadRepositoryFile(
            "src", "Edulytics.Web", "Controllers", "AssessmentBuilderController.cs");
        var builderView = ReadRepositoryFile(
            "src", "Edulytics.Web", "Views", "AssessmentBuilder", "Index.cshtml");

        Assert.Contains("TempData[\"ManualQuestionSaved\"] = true", controller, StringComparison.Ordinal);
        Assert.Contains("data-manual-saved", builderView, StringComparison.Ordinal);
        Assert.Contains("sessionStorage.setItem(key", builderView, StringComparison.Ordinal);
        Assert.Contains("sessionStorage.getItem(key", builderView, StringComparison.Ordinal);
        Assert.Contains("sessionStorage.removeItem(key)", builderView, StringComparison.Ordinal);
        Assert.Contains("outcomeIds", builderView, StringComparison.Ordinal);
    }

    [Fact]
    public void CreateAssessment_ShowsClassNameWithoutInternalClassCode()
    {
        var view = ReadRepositoryFile(
            "src", "Edulytics.Web", "Views", "Assessments", "Index.cshtml");

        Assert.Contains("<option value=\"@item.Id\">@item.Name</option>", view, StringComparison.Ordinal);
        Assert.DoesNotContain("@item.Name (@item.Code)", view, StringComparison.Ordinal);
    }

    private static string ReadRepositoryFile(params string[] relativeSegments)
    {
        var root = FindRoot();
        return File.ReadAllText(Path.Combine([root, .. relativeSegments]));
    }

    private static string FindRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null &&
               !File.Exists(Path.Combine(directory.FullName, "Edulytics.sln")))
        {
            directory = directory.Parent;
        }

        return directory?.FullName
            ?? throw new InvalidOperationException("Repository root not found.");
    }
}
